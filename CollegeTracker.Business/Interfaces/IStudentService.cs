using CollegeTracker.Business.ViewModels;

namespace CollegeTracker.Business.Interfaces;

public interface IStudentService
{
    Task<long> CreateAsync(StudentModificationDTO entity, CancellationToken cancellationToken);

    Task<StudentViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);

    IEnumerable<StudentViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<StudentViewModel> UpdateAsync(StudentModificationDTO entity, CancellationToken cancellationToken);

    Task ChangeStudentGroup(long studentId, long groupId, CancellationToken cancellationToken);
}