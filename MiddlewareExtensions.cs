using DBE.ENERGY.Web.Middlewares;
using Microsoft.AspNetCore.Builder;


namespace DBE.ENERGY.Web.Extensions
{
    public static class MiddlewareExtensions
    {


        /// <summary>
        /// Middleware that add DBE-MAIL header to the request for development enviroment.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAddFakeUserMail(this IApplicationBuilder app, string mail)
        {
            return app.Use(async (context, next) =>
            {
                context.Request.Headers.Add("DBE-MAIL", mail);
                await next();
            });
        }

        /// <summary>
        /// This is a starting point of setting user session data. 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSetUserSession(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserSessionMiddleware>();
        }
    }
}
