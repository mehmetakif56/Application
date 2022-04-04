using DBE.ENERGY.Core.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DBE.ENERGY.Web
{
    public static class StartupAuth
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Error/403";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PersonnelAndCustomer", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.Personnel,
                        RoleConstant.RequestAccept,
                        RoleConstant.Customer
                    });
                });
                options.AddPolicy("Customer", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.Customer,
                    });
                });
                options.AddPolicy("Director", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.Director,
                    });
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.SysAdmin,
                        RoleConstant.AppAdmin,
                         });
                });

                options.AddPolicy("SysAdmin", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.SysAdmin
                         });
                });

                options.AddPolicy("FieldEngineer", policy =>
                {
                    policy.RequireRole(new List<string> {
                        RoleConstant.FieldEngineer,
                        RoleConstant.ProductOwner,
                        RoleConstant.Personnel,
                         });
                });

            });
            return services;
        }
    }


}
