using AutoMapper;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class ProjectService: BaseService<Project>, IProjectService
{
    private readonly KSTDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IFileUploadService fileUploadService;

    public ProjectService(
        KSTDbContext dbContext,
        IMapper mapper,
        IFileUploadService fileUploadService
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.fileUploadService = fileUploadService;
    }

    public async Task<long> CreateAsync(ProjectModificationDTO dto, CancellationToken cancellationToken)
    {
        var project = mapper.Map<Project>(dto);
        var entity = await dbContext.Projects.AddAsync(project, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }

    public async Task<Project> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Project> GetAll()
        => dbContext.Projects
            .AsNoTracking()
            .Include(x => x.Teacher).ThenInclude(x => x.UserInfo)
            .Include(x => x.Speciality)
            .Include(x => x.Students).ThenInclude(x => x.UserInfo)
            .Include(x => x.Tasks).ThenInclude(x => x.AssignedTo).ThenInclude(x => x.UserInfo)
            .AsSplitQuery()
            .AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Projects.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Projects.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project> UpdateAsync(ProjectModificationDTO dto, CancellationToken cancellationToken)
    {
        var group = mapper.Map<Project>(dto);
        dbContext.Attach(group);
        dbContext.Entry(group).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return group;
    }

    public IQueryable<ProjectAttachment> GetAttachments(long projectId)
    {
        return dbContext.ProjectAttachment.Where(x => x.ProjectId == projectId);
    }

    public async Task<long> AddLink(ProjectAttachment attachment, CancellationToken cancellationToken)
    {
        await dbContext.ProjectAttachment.AddAsync(attachment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return attachment.Id;
    }

    public async Task UploadFile(long projectId, IFormFile file, CancellationToken cancellationToken)
    {
        var filePath = await fileUploadService.UploadFileAsync(projectId, file);
        var attachment = new ProjectAttachment()
        {
            ProjectId = projectId,
            Name = file.FileName,
            ContentType = file.ContentType,
            FilePath = filePath
        };
        await dbContext.ProjectAttachment.AddAsync(attachment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<bool> DeleteFile(long attachmentId, CancellationToken cancellationToken)
    {
        var attachment = await dbContext.ProjectAttachment.FirstAsync(x => x.Id == attachmentId, cancellationToken);
        dbContext.ProjectAttachment.Remove(attachment);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return fileUploadService.DeleteFile(attachment.FilePath!);
    }

    public async Task<FileStreamResult> DownloadFile(long attachmentId, CancellationToken cancellationToken)
    {
        var attachment = await dbContext.ProjectAttachment.FirstAsync(x => x.Id == attachmentId, cancellationToken);
        var fileStream = fileUploadService.GetFileStream(attachment.FilePath);
        return new FileStreamResult(fileStream!, attachment.ContentType) { FileDownloadName = attachment.Name };
    }
}