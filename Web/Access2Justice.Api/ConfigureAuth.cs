using Access2Justice.CosmosDb;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

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
                        UriCreateUserDetails(user);
                    }
                };
            })
            .AddCookie(cookieOptions => cookieOptions.LoginPath = new PathString("/login"));
        }

        private async void UriCreateUserDetails(dynamic userObject)
        {
            var resource = JsonConvert.SerializeObject(userObject);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);

            UserProfile userProfile = new UserProfile();
            userProfile.OId = ((JToken)userUIDocument).Root.SelectToken("id").Value<string>();
            userProfile.FirstName = ((JToken)userUIDocument).Root.SelectToken("givenName").Value<string>();
            userProfile.LastName = ((JToken)userUIDocument).Root.SelectToken("surname").Value<string>();
            userProfile.EMail = ((JToken)userUIDocument).Root.SelectToken("userPrincipalName").Value<string>();
            userProfile.IsActive = "Yes";
            userProfile.CreatedBy = ((JToken)userUIDocument).Root.SelectToken("displayName").Value<string>();
            userProfile.CreatedTimeStamp = Convert.ToString(DateTime.UtcNow, CultureInfo.InvariantCulture);

            ICosmosDbSettings cosmosDbSettings = new CosmosDbSettings(Configuration.GetSection("CosmosDb"));

            using (var client = new DocumentClient(new Uri(cosmosDbSettings.Endpoint.ToString()), cosmosDbSettings.AuthKey))
            {
                await client.OpenAsync();
                var response = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.UserProfileCollectionId),
                       "select * from c  where  c.oId in ('" + userProfile.OId + "')").ToList();
                if (response.Count == 0)
                {
                    // var document = response.First();
                    var result = await client.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.UserProfileCollectionId),
                        userProfile);
                }
            }
        }


        public void ConfigureRoutes(IApplicationBuilder app)
        {
            app.Map("/api/login", builder =>
            {
                builder.Run(async context =>
                {
                    await context.ChallengeAsync("Microsoft-Auth", new AuthenticationProperties() { RedirectUri = "/" });
                    return;
                });
            });

            app.Map("/api/logout", builder =>
            {
                builder.Run(async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect(Configuration["Api:Endpoint"]);
                });
            });
        }
    }
}