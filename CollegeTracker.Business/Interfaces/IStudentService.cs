using CollegeTracker.Business.ViewModels;

namespace CollegeTracker.Business.Interfaces;

public interface IStudentService
{
    Task<long> CreateAsync(StudentViewModel entity, CancellationToken cancellationToken);

    Task<StudentViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);

    IEnumerable<StudentViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<StudentViewModel> UpdateAsync(StudentViewModel entity, CancellationToken cancellationToken);

    Task ChangeStudentGroup(long studentId, long groupId, CancellationToken cancellationToken);
}