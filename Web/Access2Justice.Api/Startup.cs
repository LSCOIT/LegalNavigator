using Access2Justice.Api.Authentication;
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
using Access2Justice.Shared.KeyVault;
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Access2Justice.Shared.QnAMaker;
using Access2Justice.Shared.Storage;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Access2Justice.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var context = new LoadContext();
            context.LoadUnmanagedLibrary(/*Path.Combine(Directory.GetCurrentDirectory(), */"libwkhtmltox"/*)*/);

            ConfigureSession(services);

            services.AddMvc();

            services.AddSingleton<IKeyVaultSettings>(
                new KeyVaultSettings(Configuration.GetSection("KeyVault")));

            services.AddSingleton<ISecretsService>(
                serviceProvider =>
                    new KeyVaultSecretsService(serviceProvider.GetService<IKeyVaultSettings>()));

            services.AddSingleton<ILuisSettings>(
                serviceProvider =>
                new LuisSettings(
                    Configuration.GetSection("Luis"),
                    serviceProvider.GetService<ISecretsService>()));

            services.AddSingleton<IBingSettings>(
                serviceProvider =>
                    new BingSettings(
                        Configuration.GetSection("Bing"),
                        serviceProvider.GetService<ISecretsService>()));

            IShareSettings shareSettings = new ShareSettings(Configuration.GetSection("Share"));
            services.AddSingleton(shareSettings);


            IAdminSettings adminSettings = new AdminSettings(Configuration.GetSection("Admin"));
            services.AddSingleton(adminSettings);

            services.AddSingleton<IStorageSettings>(new StorageSettings(Configuration.GetSection("Storage")));

            services.AddSingleton<IOnboardingInfoSettings>(
                serviceProvider =>
                    new OnboardingInfoSettings(
                        Configuration.GetSection("EmailService"),
                        serviceProvider.GetService<ISecretsService>()));

            services.AddSingleton<IQnAMakerSettings>(
                serviceProvider =>
                    new QnAMakerSettings(
                        Configuration.GetSection("QnAMaker"),
                        serviceProvider.GetService<ISecretsService>()));

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
            services.AddSingleton<IQnABotBusinessLogic, QnABotBusinessLogic>();

            services.AddScoped<ITemplateService, TemplateService>();
            services.AddSingleton<ITools, PdfTools>();
            services.AddSingleton<IConverter, SynchronizedConverter>();
            services.AddScoped<IPdfService, PdfService>();
            // services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));

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
            var apiEndpoint = new Uri(Configuration.GetSection("Api:Endpoint").Value);
            var url = $"{apiEndpoint.Scheme}://{apiEndpoint.Host}:{apiEndpoint.Port}";
            app.UseCors(builder => builder // .WithOrigins(url)
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // https://github.com/aspnet/AspNetCore/issues/2378
                app.UseForcedCorsHeaders();
            }

            app.UseSession();
            app.UseAuthentication();
            
            //Enabling Framework Security Settings.
            app.UseFrameworkSecurity();

            app.UseMvc();

            ConfigureSwagger(app);
        }

        private void ConfigureCosmosDb(IServiceCollection services)
        {
            services.AddSingleton<ICosmosDbSettings>(
                serviceProvider =>
                    new CosmosDbSettings(
                        Configuration.GetSection("CosmosDb"),
                        serviceProvider.GetService<ISecretsService>()));

            services.AddSingleton<IDocumentClient>(
                serviceProvider =>
                {
                    var cosmosDbSettings = serviceProvider.GetService<ICosmosDbSettings>();
                    return new DocumentClient(
                        cosmosDbSettings.Endpoint,
                        cosmosDbSettings.AuthKey);
                });
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

        class LoadContext : AssemblyLoadContext
        {
            protected override Assembly Load(AssemblyName assemblyName)
            {
                return null;
            }

            public IntPtr LoadUnmanagedLibrary(string absolutePath)
            {
                return LoadUnmanagedDll(absolutePath);
            }

            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
            {
                var arch = Environment.Is64BitProcess ? "x64" : "x86";
                var path = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "wkhtmltox",
                    arch
                );
                string ext;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    ext = "dylib";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    ext = "so";

                }
                else // RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                {
                    ext = "dll";
                }

                return LoadUnmanagedDllFromPath(Path.Combine(path, $"{unmanagedDllName}.{ext}"));
            }
        }
    }
}
