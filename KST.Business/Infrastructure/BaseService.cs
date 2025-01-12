using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Infrastructure;

public abstract class BaseService<T>(KSTDbContext dbContext): IActivityChangeable where T: BaseEntity
{
    public async Task ChangeActivityState(long entityId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<T>().FirstAsync(x => x.Id == entityId, cancellationToken);
        entity.IsActive = !entity.IsActive;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}