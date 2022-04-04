using DBE.ENERGY.Resources;
using DBE.ENERGY.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace DBE.ENERGY.Web
{
    public static class StartupMvc
    {
        public static IServiceCollection AddEnhancedMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                // Automatically add antiforgery token valdaiton to all post actions.
                // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(ActionLoggerFilter));
            })
             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
             .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider =
                (type, factory) => factory.Create(typeof(ModelDataAnnotationResource)))
             .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
              });

            return services;
        }
    }
}

