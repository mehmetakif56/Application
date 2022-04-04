using Microsoft.AspNetCore.Mvc.Filters;


namespace DBE.ENERGY.Web.Filters
{
    public class AuthAdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

        }
    }
}