using KST.Business.Infrastructure;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Infrastructure;

public abstract class BaseController<T>(IBaseService baseService): ControllerBase where T: BaseEntity
{
    [HttpPost("{id}")]
    public async Task ChangeActivityState([FromRoute]long id, CancellationToken cancellationToken)
    {
        await baseService.ChangeActivityState(id, cancellationToken);
    }
}