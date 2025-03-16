using AutoMapper;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class MessageService: BaseService<ProjectTask>, IMessageService
{
    private readonly KSTDbContext dbContext;
    private readonly IMapper mapper;

    public MessageService(
        KSTDbContext dbContext,
        IMapper mapper
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(MessageDTO dto, CancellationToken cancellationToken)
    {
        var message = mapper.Map<Message>(dto);
        message.SendDate = DateTime.UtcNow;
        var entity = await dbContext.Set<Message>().AddAsync(message, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }
    
    public async Task<long> CreateSystemActionLog(long projectId, string content, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            ProjectId = projectId,
            Content = content,
            SendDate = DateTime.UtcNow,
            SystemEvent = true
        };
        var entity = await dbContext.Set<Message>().AddAsync(message, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }

    public IQueryable<Message> GetAll(long projectId)
        => dbContext.Set<Message>()
            .AsNoTracking()
            .Include(x => x.Sender)
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.SendDate)
            .AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Message>().FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Set<Message>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}