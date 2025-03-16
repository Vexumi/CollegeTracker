using KST.Business.Infrastructure;
using KST.Business.ViewModels;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;

namespace KST.Business.Interfaces;

public interface IMessageService: IBaseService
{
    Task<long> CreateAsync(MessageDTO dto, CancellationToken cancellationToken);
    
    Task<long> CreateSystemActionLog(long projectId, string content, CancellationToken cancellationToken);
    
    IQueryable<Message> GetAll(long projectId);

    Task DeleteAsync(long id, CancellationToken cancellationToken);
}