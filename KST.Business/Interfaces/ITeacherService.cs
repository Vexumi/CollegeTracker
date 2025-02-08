using KST.Business.Infrastructure;
using KST.Business.ViewModels;

namespace KST.Business.Interfaces;

public interface ITeacherService: IBaseService
{
    Task<long> CreateAsync(TeacherModificationDTO viewModel, CancellationToken cancellationToken);

    Task<TeacherViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);

    IEnumerable<TeacherViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<TeacherViewModel> UpdateAsync(TeacherModificationDTO entity, CancellationToken cancellationToken);
}