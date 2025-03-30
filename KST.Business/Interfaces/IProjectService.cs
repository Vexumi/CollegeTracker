using KST.Business.Infrastructure;
using KST.Business.ViewModels;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KST.Business.Interfaces;

public interface IProjectService: IBaseService
{
    Task<long> CreateAsync(ProjectModificationDTO dto, CancellationToken cancellationToken);

    Task<Project> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Project> GetAll();

    Task<ProjectSearchResponseDTO> SearchProjects(ProjectSearchParamsDTO searchParams,
        CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Project> UpdateAsync(ProjectModificationDTO group, CancellationToken cancellationToken);

    Task<long> ChangeState(long projectId, ProjectState state, CancellationToken cancellationToken);

    Task<long> Evaluate(long projectId, int mark, CancellationToken cancellationToken);

    IQueryable<ProjectAttachment> GetAttachments(long projectId);
    
    Task<long> AddLink(ProjectAttachment attachment, CancellationToken cancellationToken);

    Task UploadFile(long projectId, IFormFile file, CancellationToken cancellationToken);

    Task<bool> DeleteFile(long attachmentId, CancellationToken cancellationToken);

    Task<FileStreamResult> DownloadFile(long attachmentId, CancellationToken cancellationToken);
}