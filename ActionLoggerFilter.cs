using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DBE.ENERGY.Web.Filters
{
    public class ActionLoggerFilter : IActionFilter
    {
        private ILogger<ActionLoggerFilter> _logger;

        public ActionLoggerFilter(ILogger<ActionLoggerFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var method = context.HttpContext.Request.Method;
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            var identity = context.HttpContext.User.Identity;

            _logger.LogInformation($"{identity.Name} executing {controller}/{action} ({method})");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var method = context.HttpContext.Request.Method;
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            var identity = context.HttpContext.User.Identity;
            _logger.LogInformation($"{identity.Name} has finished executing {controller}/{action} ({method})");
        }
    }
}