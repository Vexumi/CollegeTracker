using KST.Business.Infrastructure;
using KST.Business.ViewModels;

namespace KST.Business.Interfaces;

public interface IStudentService: IBaseService
{
    Task<long> CreateAsync(StudentModificationDTO entity, CancellationToken cancellationToken);

    Task<StudentViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);

    IEnumerable<StudentViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<StudentViewModel> UpdateAsync(StudentModificationDTO entity, CancellationToken cancellationToken);

    Task ChangeStudentGroup(long studentId, long groupId, CancellationToken cancellationToken);
}