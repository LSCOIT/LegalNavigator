﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Access2Justice.Shared.Models;

namespace Access2Justice.Api
{
    public partial class Startup
    {
        public void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddMicrosoftAccount("Microsoft-Auth", options =>
            {
                options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                options.AuthorizationEndpoint = Configuration["Authentication:Microsoft:Authority"];
                options.Events = new OAuthEvents()
                {
                    OnRemoteFailure = context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(context.Failure.Message);
                    },
                    OnTicketReceived = context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, Configuration["Api:Endpoint"]);
                        if (context.Principal.Identity.IsAuthenticated)
                        {
                            ApplicationUser user = new ApplicationUser();
                            var identity = context.Principal.Identity as ClaimsIdentity;
                            user.UserName = identity.Name;
                            user.UserId = identity.Claims.Where(x => x.Type.Contains("nameidentifier")).Select(x => x.Value).FirstOrDefault();
                            context.Response.Cookies.Append("profileData", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                            context.ReturnUri = Configuration["Api:Endpoint"];
                            context.Response.Redirect(context.ReturnUri, true);
                        }
                        return Task.FromResult(0);
                    },
                    OnCreatingTicket = async context =>
                    {
                        // Retrieve user info
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Add("Authorization", $"Bearer {context.AccessToken}");

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // Extract the user info object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Add the Name Identifier claim
                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the Name claim
                        var email = user.Value<string>("displayName");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, email, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }
                    }
                };
            })
            .AddCookie(o => o.LoginPath = new PathString("/login"));
        }


        public void ConfigureRoutes(IApplicationBuilder app)
        {
            app.Map("/login", builder =>
            {
                builder.Run(async context =>
                {
                    await context.ChallengeAsync("Microsoft-Auth", new AuthenticationProperties() { RedirectUri = "/" });
                    return;
                });
            });

            app.Map("/logout", builder =>
            {
                builder.Run(async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/");
                });
            });
        }

    }
}




