using KST.Business.Infrastructure;
using KST.Business.ViewModels;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;

namespace KST.Business.Interfaces;

public interface IProjectTaskService: IBaseService
{
    Task<long> CreateAsync(ProjectTaskModificationDTO dto, CancellationToken cancellationToken);

    Task<ProjectTask> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<ProjectTask> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<ProjectTask> UpdateAsync(ProjectTaskModificationDTO group, CancellationToken cancellationToken);
    
    Task<long> ChangeState(long id, TaskState state, CancellationToken cancellationToken);
}