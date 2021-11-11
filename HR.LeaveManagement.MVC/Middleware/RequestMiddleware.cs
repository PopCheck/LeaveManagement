using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HR.LeaveManagement.MVC.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalStorageService _localStorageService;

        public RequestMiddleware(RequestDelegate next, ILocalStorageService localStorageService)
        {
            _next = next;
            _localStorageService = localStorageService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var ep = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                var authAttr = ep.Metadata.GetMetadata<AuthorizeAttribute>();

                if(authAttr != null)
                {
                    var tokenExists = _localStorageService.Exists("token");
                    var tokenIsValid = true;

                    if(tokenExists)
                    {
                        var token = _localStorageService.GetStorageValue<string>("token");
                        var tokenContext = new JwtSecurityTokenHandler().ReadJwtToken(token);
                        var expiry = tokenContext.ValidTo;

                        if(expiry < DateTime.UtcNow)
                        {
                            tokenIsValid = false;
                        }
                    }

                    if(!tokenIsValid || !tokenExists)
                    {
                        await SignOutAndRedirect(httpContext);
                        return;
                    }

                    if (authAttr.Roles != null)
                    {
                        var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                        if (!authAttr.Roles.Contains(userRole))
                        {
                            httpContext.Response.Redirect("/home/notauthorized");
                            return;
                        }
                    }
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async static Task SignOutAndRedirect(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            httpContext.Response.Redirect("/users/login");
        }

        private async static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            switch(exception)
            {
                case ApiException _:
                    await SignOutAndRedirect(context);
                    break;
                default:
                    context.Response.Redirect("/Home/Error");
                    break;
            }
        }
    }
}
