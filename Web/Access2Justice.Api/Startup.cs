using Access2Justice.Api.Authentication;
using Access2Justice.Api.Authorization;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
using Access2Justice.CosmosDb;
using Access2Justice.Shared;
using Access2Justice.Shared.Bing;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Luis;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Share;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Access2Justice.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSession(services);

            services.AddMvc();
            string oId = string.Empty;
            ILuisSettings luisSettings = new LuisSettings(Configuration.GetSection("Luis"));
            services.AddSingleton(luisSettings);

            IBingSettings bingSettings = new BingSettings(Configuration.GetSection("Bing"));
            services.AddSingleton(bingSettings);

            IShareSettings shareSettings = new ShareSettings(Configuration.GetSection("Share"));
            services.AddSingleton(shareSettings);

            services.AddSingleton<ILuisProxy, LuisProxy>();
            services.AddSingleton<ILuisBusinessLogic, LuisBusinessLogic>();
            services.AddSingleton<ITopicsResourcesBusinessLogic, TopicsResourcesBusinessLogic>();
            services.AddSingleton<IWebSearchBusinessLogic, WebSearchBusinessLogic>();
            services.AddSingleton<IA2JAuthorBusinessLogic, A2JAuthorBusinessLogic>();
            services.AddSingleton<ICuratedExperienceBusinessLogic, CuratedExperienceBuisnessLogic>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddSingleton<IUserProfileBusinessLogic, UserProfileBusinessLogic>();
            services.AddSingleton<IPersonalizedPlanBusinessLogic, PersonalizedPlanBusinessLogic>();
            services.AddSingleton<IStaticResourceBusinessLogic, StaticResourceBusinessLogic>();
            services.AddSingleton<IShareBusinessLogic, ShareBusinessLogic>();
            services.AddSingleton<IUserRoleBusinessLogic, UserRoleBusinessLogic>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));
            oId = ValidateToken();
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                var AdminRolesPolicy = UserRoles.RoleEnum.GlobalAdmin.ToString() + "," +
                UserRoles.RoleEnum.StateAdmin.ToString();

				var AuthenticatedUserPolicy = UserRoles.RoleEnum.GlobalAdmin.ToString() + "," +
				UserRoles.RoleEnum.StateAdmin.ToString() + "," +
				UserRoles.RoleEnum.Authenticated.ToString();

				options.AddPolicy(UserRoles.PolicyEnum.GlobalAdminPolicy.ToString(), policy =>
                policy.AddRequirements(new AuthorizeUser(oId, UserRoles.RoleEnum.GlobalAdmin.ToString())));

                options.AddPolicy(UserRoles.PolicyEnum.StateAdminPolicy.ToString(), policy =>
                policy.AddRequirements(new AuthorizeUser(oId, UserRoles.RoleEnum.StateAdmin.ToString())));

                options.AddPolicy(UserRoles.PolicyEnum.DeveloperPolicy.ToString(), policy =>
                policy.AddRequirements(new AuthorizeUser(oId, UserRoles.RoleEnum.Developer.ToString())));

                options.AddPolicy(UserRoles.PolicyEnum.AuthenticatedPolicy.ToString(), policy =>
                policy.AddRequirements(new AuthorizeUser(oId, UserRoles.RoleEnum.Authenticated.ToString())));

				options.AddPolicy(UserRoles.PolicyEnum.AnonymousPolicy.ToString(), policy =>
				policy.AddRequirements(new AuthorizeUser(oId, UserRoles.RoleEnum.Anonymous.ToString())));

				options.AddPolicy(UserRoles.PolicyEnum.AdminRolesPolicy.ToString(), policy =>
                policy.AddRequirements(new AuthorizeUser(oId, AdminRolesPolicy)));

				options.AddPolicy(UserRoles.PolicyEnum.AuthenticatedUserPolicy.ToString(), policy =>
				policy.AddRequirements(new AuthorizeUser(oId, AuthenticatedUserPolicy)));

			});
            services.AddSingleton<IAuthorizationHandler, AuthorizeUserHandler>();
            services.AddSingleton<IAuthorizationHandler, PermissionsHandler>();
            ConfigureCosmosDb(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Access2Justice API", Version = "v1" });
                c.TagActionsBy(api => api.GroupName);
                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
                c.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
                c.OperationFilter<FileUploadOperationResource>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var apiEnpoint = new Uri(Configuration.GetSection("Api:Endpoint").Value);
            var url = $"{apiEnpoint.Scheme}://{apiEnpoint.Host}:{apiEnpoint.Port}";         
            app.UseCors(builder => builder.WithOrigins(url)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseSession();

            app.UseAuthentication();
            app.UseMvc();

            ConfigureSwagger(app);
        }

        private void ConfigureCosmosDb(IServiceCollection services)
        {
            ICosmosDbSettings cosmosDbSettings = new CosmosDbSettings(Configuration.GetSection("CosmosDb"));
            services.AddSingleton(cosmosDbSettings);
            services.AddSingleton<IDocumentClient>(x => new DocumentClient(cosmosDbSettings.Endpoint, cosmosDbSettings.AuthKey));
            services.AddSingleton<IBackendDatabaseService, CosmosDbService>();
            services.AddSingleton<IDynamicQueries, CosmosDbDynamicQueries>();
        }

        private void ConfigureSession(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(
                    Configuration.GetValue<int>("Api:SessionDurationInMinutes"));
                options.Cookie.HttpOnly = true;
            });
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Host = httpReq.Host.Value;
                });
            });

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint(Configuration.GetValue<string>("Api:VirtualPath") + "/swagger/v1/swagger.json", "Access2Justice API");
            });

        }

        private string ValidateToken() //Need to pass token
        {
            string encryptedOid = string.Empty;
            Guid oId =  //new Guid("1803a665-8a5e-45a9-848a-1331ade5c152"); //Anonymous
					new Guid("00000000-0000-0000-8f8b-cbb21fe0448c"); //State Admin
																		  //new Guid("cb09b65a-43a6-4525-8b45-ede2c319c75f"); //Global Admin
			encryptedOid = EncryptString(oId.ToString());
            return encryptedOid;
        }

        private string EncryptString(string input)
        {
            string encryptedId = input;
            if (!string.IsNullOrEmpty(input))
            {
                encryptedId = EncryptionUtilities.GenerateSHA512String(input);
            }
            return encryptedId;
        }
    }
}
