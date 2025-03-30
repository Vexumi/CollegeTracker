using AutoMapper;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
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
    private readonly IMessageService messageService;

    public ProjectService(
        KSTDbContext dbContext,
        IMapper mapper,
        IFileUploadService fileUploadService,
        IMessageService messageService
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.fileUploadService = fileUploadService;
        this.messageService = messageService;
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

    public async Task<ProjectSearchResponseDTO> SearchProjects(ProjectSearchParamsDTO searchParams, CancellationToken cancellationToken)
    {
        var request = dbContext.Set<Project>().AsNoTracking().AsQueryable();
        request = request.WhereIf(!string.IsNullOrEmpty(searchParams.Title),
            project => EF.Functions.ILike(project.Title, searchParams.Title));
        request = request.WhereIf(!string.IsNullOrEmpty(searchParams.Description),
            project => EF.Functions.ILike(project.Description, searchParams.Description));
        
        request = request.WhereIf(searchParams.State != null,
            project => project.State == searchParams.State);
        request = request.WhereIf(searchParams.SpecialityId != null,
            project => project.SpecialityId == searchParams.SpecialityId);
        
        request = request.WhereIf(searchParams.StartDateFrom != null,
            project => project.StartDate >= searchParams.StartDateFrom);
        request = request.WhereIf(searchParams.StartDateTo != null,
            project => project.StartDate <= searchParams.StartDateTo);
        
        request = request.WhereIf(searchParams.ActualEndDateFrom != null,
            project => project.ActualEndDate >= searchParams.ActualEndDateFrom);
        request = request.WhereIf(searchParams.ActualEndDateTo != null,
            project => project.ActualEndDate <= searchParams.ActualEndDateTo);
        
        request = request.WhereIf(searchParams.DeadlineFrom != null,
            project => project.Deadline >= searchParams.DeadlineFrom);
        request = request.WhereIf(searchParams.DeadlineTo != null,
            project => project.Deadline <= searchParams.DeadlineTo);

        request = request.WhereIf(searchParams.CurrentUserId != null,
            project => project.Teacher.UserInfoId == searchParams.CurrentUserId ||
                       project.Students.Select(x => x.UserInfoId).Contains(searchParams.CurrentUserId.Value));

        var result = await request.Include(x => x.Speciality).GroupBy(p => 1).Select(g => new ProjectSearchResponseDTO()
        {
            Projects = g.Skip(searchParams.Page * searchParams.PageSize).Take(searchParams.PageSize),
            TotalPages = (int)Math.Ceiling((double)g.Count() / searchParams.PageSize)
        }).FirstOrDefaultAsync(cancellationToken);

        return result ?? new ProjectSearchResponseDTO{ Projects = [], TotalPages = 0 };
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Projects.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Projects.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project> UpdateAsync(ProjectModificationDTO dto, CancellationToken cancellationToken)
    {
        var project = await dbContext.Set<Project>().AsTracking().FirstAsync(x => x.Id == dto.Id, cancellationToken);
        project.Title = dto.Title;
        project.Description = dto.Description;
        project.TeacherId = dto.TeacherId;
        project.SpecialityId = dto.SpecialityId;
        project.StartDate = dto.StartDate;
        project.Deadline = dto.Deadline;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await messageService.CreateSystemActionLog(project.Id, MessageFormats.ProjectInfoChanged, cancellationToken);
        return project;
    }

    public async Task<long> ChangeState(long projectId, ProjectState state, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.AsTracking().FirstAsync(x => x.Id == projectId, cancellationToken);
        project.State = state;
        await dbContext.SaveChangesAsync(cancellationToken);

        await messageService.CreateSystemActionLog(projectId, string.Format(
                MessageFormats.ProjectStateChanged, 
                ProjectExtensions.GetLocalizedProjectState(state)),
            cancellationToken);
        return projectId;
    }

    public async Task<long> Evaluate(long projectId, int mark, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.AsTracking().FirstAsync(x => x.Id == projectId, cancellationToken);
        project.Mark = mark;
        project.ActualEndDate = DateOnly.FromDateTime(DateTime.UtcNow);
        project.State = ProjectState.Reviewed;
        await dbContext.SaveChangesAsync(cancellationToken);

        await messageService.CreateSystemActionLog(projectId, string.Format(MessageFormats.Reviewed, mark),
            cancellationToken);
        return projectId;
    }

    public IQueryable<ProjectAttachment> GetAttachments(long projectId)
    {
        return dbContext.ProjectAttachment.Where(x => x.ProjectId == projectId);
    }

    public async Task<long> AddLink(ProjectAttachment attachment, CancellationToken cancellationToken)
    {
        await dbContext.ProjectAttachment.AddAsync(attachment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await messageService.CreateSystemActionLog(attachment.ProjectId, string.Format(
                MessageFormats.AddedLink, 
                attachment.Name),
            cancellationToken);
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
        
        await messageService.CreateSystemActionLog(attachment.ProjectId, string.Format(
                MessageFormats.AddedFile, 
                attachment.Name),
            cancellationToken);
    }
    
    public async Task<bool> DeleteFile(long attachmentId, CancellationToken cancellationToken)
    {
        var attachment = await dbContext.ProjectAttachment.FirstAsync(x => x.Id == attachmentId, cancellationToken);
        dbContext.ProjectAttachment.Remove(attachment);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await messageService.CreateSystemActionLog(attachment.ProjectId, string.Format(
                MessageFormats.RemovedFile, 
                attachment.Name),
            cancellationToken);
        
        return fileUploadService.DeleteFile(attachment.FilePath!);
    }

    public async Task<FileStreamResult> DownloadFile(long attachmentId, CancellationToken cancellationToken)
    {
        var attachment = await dbContext.ProjectAttachment.FirstAsync(x => x.Id == attachmentId, cancellationToken);
        var fileStream = fileUploadService.GetFileStream(attachment.FilePath);
        return new FileStreamResult(fileStream!, attachment.ContentType) { FileDownloadName = attachment.Name };
    }
}