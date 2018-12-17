using Access2Justice.Api.Authentication;
using Access2Justice.Api.Authorization;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
using Access2Justice.CosmosDb;
using Access2Justice.Shared;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Admin;
using Access2Justice.Shared.Bing;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Luis;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Share;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using System.IO;

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
            ILuisSettings luisSettings = new LuisSettings(Configuration.GetSection("Luis"));
            services.AddSingleton(luisSettings);

            IBingSettings bingSettings = new BingSettings(Configuration.GetSection("Bing"));
            services.AddSingleton(bingSettings);

            IShareSettings shareSettings = new ShareSettings(Configuration.GetSection("Share"));
            services.AddSingleton(shareSettings);

            IKeyVaultSettings keyVaultSettings = new KeyVaultSettings(Configuration.GetSection("KeyVault"));
            services.AddSingleton(keyVaultSettings);

            IAdminSettings adminSettings = new AdminSettings(Configuration.GetSection("Admin"));
            services.AddSingleton(adminSettings);

            IOnboardingInfoSettings onboardingInfoSettings = new OnboardingInfoSettings(Configuration.GetSection("EmailService"), Configuration.GetSection("KeyVault"));
            services.AddSingleton(onboardingInfoSettings);


            services.AddSingleton<ILuisProxy, LuisProxy>();
            services.AddSingleton<ILuisBusinessLogic, LuisBusinessLogic>();
            services.AddSingleton<ITopicsResourcesBusinessLogic, TopicsResourcesBusinessLogic>();
            services.AddSingleton<IWebSearchBusinessLogic, WebSearchBusinessLogic>();
            services.AddSingleton<ICuratedExperienceConvertor, A2JAuthorBusinessLogic>();
            services.AddSingleton<ICuratedExperienceBusinessLogic, CuratedExperienceBuisnessLogic>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddSingleton<IUserProfileBusinessLogic, UserProfileBusinessLogic>();
            services.AddSingleton<IPersonalizedPlanBusinessLogic, PersonalizedPlanBusinessLogic>();
            services.AddSingleton<IStaticResourceBusinessLogic, StaticResourceBusinessLogic>();
            services.AddSingleton<IShareBusinessLogic, ShareBusinessLogic>();
            services.AddSingleton<ICuratedExperienceConvertor, A2JAuthorBusinessLogic>();
            services.AddSingleton<IPersonalizedPlanEngine, PersonalizedPlanEngine>();
            services.AddSingleton<IA2JAuthorLogicParser, LogicParser>();
            services.AddSingleton<IA2JAuthorLogicInterpreter, LogicInterpreter>();
            services.AddSingleton<IPersonalizedPlanViewModelMapper, PersonalizedPlanViewModelMapper>();
            services.AddSingleton<IUserRoleBusinessLogic, UserRoleBusinessLogic>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<IAdminBusinessLogic, AdminBusinessLogic>();
            services.AddSingleton<IStateProvinceBusinessLogic, StateProvinceBusinessLogic>();
            services.AddSingleton<IOnboardingInfoBusinessLogic, OnboardingInfoBusinessLogic>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });
            ConfigureCosmosDb(services);
        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Access2Justice API", Version = "1.0.0" , Description ="List of all APIs for Access2Justice", TermsOfService = "None"});
                c.TagActionsBy(api => api.GroupName);
                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
                c.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
                c.OperationFilter<FileUploadOperationResource>();
                c.CustomSchemaIds(x => x.FullName);
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
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
            ICosmosDbSettings cosmosDbSettings = new CosmosDbSettings(Configuration.GetSection("CosmosDb"), (Configuration.GetSection("KeyVault")));
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
    }
}
