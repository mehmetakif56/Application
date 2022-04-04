using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Interfaces;
using DBE.ENERGY.Core.Services;
using DBE.ENERGY.Web.AuthLogin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DBE.ENERGY.Web.Middlewares
{
    /// <summary>
    /// Set user session and start cookie authentication as soon as user reach to our app. 
    /// </summary>
    public class UserSessionMiddleware
    {
        private RequestDelegate _next;
        public static bool firsLoading = false;

        /// <summary>
        /// Gets user data from storage and creates session data
        /// </summary>
        /// <param name="next"></param>
        public UserSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
            IUserService userService,
            IEmployeeService empService,
            ICustomerService customerService,
            ISessionHelper sessionHelper,
            ILogger<UserSessionMiddleware> logger)
        {

            if (IsExceptionalPage(context))
            {
                await _next.Invoke(context);
            }
            // if user is authenticated (subsequent requests)
            else if (context.User.Identity.IsAuthenticated)
            {
                if (sessionHelper.User == null)
                    sessionHelper.User = userService.GetUserByUserName(context.User.FindFirst(CustomClaimTypes.UserName).Value);

                List<ClaimEntity> roleClaims = userService.GetUserRoleClaims(sessionHelper.User.UserRoles.Select(ur => ur.Role.RoleStatusId).ToArray()).ToList();
                List<CustomClaimEntity> customClaims = sessionHelper.User.CustomClaims.ToList();

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CreateUserClaims(sessionHelper.User, roleClaims, customClaims),
                    CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true, // https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#persistent-cookies
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(90)// https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#absolute-cookie-expiration
                    });
                context.User = claimsPrincipal;
                await _next.Invoke(context);
            }
            //first login
            else
            {
                var mail = "";
                if (sessionHelper != null && sessionHelper.User != null && sessionHelper.User.UserName != null)
                    mail = sessionHelper.User.UserName;              

                if ( mail !=null && mail!="")
                {
                    var user = userService.GetUserByUserName(mail);
                    if (user != null && user.UserRoles != null)
                    {
                        List<ClaimEntity> roleClaims = userService.GetUserRoleClaims(user.UserRoles.Select(ur => ur.Role.RoleStatusId).ToArray()).ToList();
                        List<CustomClaimEntity> customClaims = user.CustomClaims.ToList();

                        #region Create user claims

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(CreateUserClaims(user, roleClaims, customClaims),
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true, // https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#persistent-cookies
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(90)// https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#absolute-cookie-expiration
                        });
                        sessionHelper.User = user;
                        context.User = claimsPrincipal;
                    }
                    else
                    {
                        sessionHelper.User = null;
                        context.User = null;
                    }
                }
                else
                {
                    sessionHelper.User = null;
                    context.User = null;
                
                    if (firsLoading)
                    {
                        firsLoading = false;
                        await context.Response.WriteAsync($"Kullanici bilgileri gelmedi, lutfen cerezleri temizleyip tekrar giris yapin veya sistem yöneticisiyle irtibata geçin!");
                        //return;
                    }

                }
                #endregion
                await _next.Invoke(context);
            }
        }

        private bool IsExceptionalPage(HttpContext context)
        {
            //TODO: Admin privilige control
            return context.Request.Path.StartsWithSegments("/Admin") || context.Request.Path.StartsWithSegments("/logout");
        }

        private IEnumerable<Claim> CreateUserClaims(UserEntity user, IEnumerable<ClaimEntity> roleClaims,
            IEnumerable<CustomClaimEntity> customClaims)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(CustomClaimTypes.UserName, user.UserName??""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            claims.AddRange(user.UserRoles.ToList().Select(o => new Claim(ClaimTypes.Role, o.Role.RoleStatusId.ToString())));
            claims.AddRange(roleClaims.Select(o => new Claim(o.ClaimType, o.ClaimValue)));
            claims.AddRange(customClaims.Select(o => new Claim(o.ClaimType, o.ClaimValue)));
            return claims;
        }
    }
}
