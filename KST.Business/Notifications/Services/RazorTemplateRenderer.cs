using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace KST.Business.Notifications.Services;

public class RazorTemplateRenderer(
    IRazorViewEngine razorViewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider)
{
    public async Task<string> RenderAsync<T>(string viewPath, T model, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(viewPath))
        {
            throw new ArgumentException("View path must be specified.", nameof(viewPath));
        }

        var actionContext = GetActionContext();

        var viewResult = razorViewEngine.GetView(null, viewPath, isMainPage: true);

        if (viewResult.View == null)
        {
            viewResult = razorViewEngine.FindView(actionContext, viewPath, isMainPage: true);

            if (viewResult.View == null)
            {
                throw new ArgumentException($"View '{viewPath}' could not be found.");
            }
        }

        using (var sw = new StringWriter())
        {
            var viewData = new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(actionContext.HttpContext, tempDataProvider);

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions()
            );


            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}