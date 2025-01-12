using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Interfaces;

public interface ISubjectService: IActivityChangeable
{
    Task<long> CreateAsync(Subject entity, CancellationToken cancellationToken);

    Task<Subject> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Subject> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Subject> UpdateAsync(Subject entity, CancellationToken cancellationToken);
}