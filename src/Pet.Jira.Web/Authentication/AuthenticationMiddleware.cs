﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Pet.Jira.Application.Authentication;
using Pet.Jira.Application.Storage;
using Pet.Jira.Application.Users;
using Pet.Jira.Web.Common;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pet.Jira.Web.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoginMemoryCache _loginMemoryCache;
        private readonly IMemoryCache<string, UserTheme> _userThemeMemoryCache;

        public AuthenticationMiddleware(
            RequestDelegate next, 
            ILoginMemoryCache loginMemoryCache,
            IMemoryCache<string, UserTheme> userThemeMemoryCache)
        {
            _next = next;
            _loginMemoryCache = loginMemoryCache;
            _userThemeMemoryCache = userThemeMemoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                if (!_loginMemoryCache.TryGetValue(key, out var login))
                {
                    context.Response.Redirect("/error");
                    return;
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.UserData, login.Password)
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(30),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                _loginMemoryCache.TryRemove(key, out _);
                context.Response.Redirect(WebConstants.Pages.Home);
                return;

            }
            else if (context.Request.Path == "/logout")
            {
                _userThemeMemoryCache.TryRemove(context.User.Identity.Name, out _);
                await context.SignOutAsync();
                context.Response.Redirect("/");
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
