using KST.Business.Infrastructure;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Interfaces;

public interface IProjectService: IBaseService
{
    Task<long> CreateAsync(ProjectModificationDTO dto, CancellationToken cancellationToken);

    Task<Project> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Project> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Project> UpdateAsync(ProjectModificationDTO group, CancellationToken cancellationToken);
}