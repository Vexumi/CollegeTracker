using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.Notifications.Services;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class ProjectService(
    KSTDbContext dbContext,
    IMapper mapper,
    IFileUploadService fileUploadService,
    IMessageService messageService,
    IHangfireNotificationService hangfireNotificationService
    ): BaseService<Project>(dbContext), IProjectService
{
    public async Task<long> CreateAsync(ProjectCreateDTO dto, CancellationToken cancellationToken)
    {
        var project = mapper.Map<Project>(dto);
        await dbContext.Projects.AddAsync(project, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await dbContext.Set<Student>()
            .Where(x => dto.GroupIds.Contains(x.GroupId) || dto.StudentIds.Contains(x.Id))
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.ProjectId, project.Id), cancellationToken);

        return project.Id;
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
        var request = dbContext.Set<Project>().AsNoTracking().AsQueryable().ApplySearchFilter(searchParams);
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
        var project = await dbContext.Projects
            .AsTracking()
            .Include(x => x.Students).ThenInclude(x => x.UserInfo)
            .Include(x => x.Teacher).ThenInclude(x => x.UserInfo)
            .FirstAsync(x => x.Id == projectId, cancellationToken);
        project.State = state;
        await dbContext.SaveChangesAsync(cancellationToken);

        await messageService.CreateSystemActionLog(projectId, string.Format(
                MessageFormats.ProjectStateChanged, 
                ProjectExtensions.GetLocalizedProjectState(state)),
            cancellationToken);
        
        hangfireNotificationService.SendProjectStateChangedNotification(project.Id);
        return projectId;
    }

    public async Task<long> Evaluate(long projectId, int mark, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects
            .Include(x => x.Students).ThenInclude(x => x.UserInfo)
            .Include(x => x.Teacher).ThenInclude(x => x.UserInfo)
            .AsTracking()
            .FirstAsync(x => x.Id == projectId, cancellationToken);
        project.Mark = mark;
        project.ActualEndDate = DateOnly.FromDateTime(DateTime.UtcNow);
        project.State = ProjectState.Reviewed;
        await dbContext.SaveChangesAsync(cancellationToken);

        await messageService.CreateSystemActionLog(projectId, string.Format(MessageFormats.Reviewed, mark),
            cancellationToken);
        
        hangfireNotificationService.SendProjectMarkAddedNotification(project.Id);
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