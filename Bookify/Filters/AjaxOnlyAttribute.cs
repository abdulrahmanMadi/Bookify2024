using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Bookify.Filters
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var IsAjax = routeContext.HttpContext.Request.Headers["x-requested-with"]=="XMLHttpRequest";
            return IsAjax;

        }
    }
}
