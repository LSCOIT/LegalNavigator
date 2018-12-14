using Access2Justice.CosmosDb;
using Access2Justice.Integration.Api.BusinessLogic;
using Access2Justice.Integration.Api.IntegrationAdapters;
using Access2Justice.Integration.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Access2Justice.Integration.Api
{
    /// <summary>
    /// start up class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// run time configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSession(services);

            services.AddMvc();

            IKeyVaultSettings keyVaultSettings = new KeyVaultSettings(Configuration.GetSection("KeyVault"));
            services.AddSingleton(keyVaultSettings);

            services.AddSingleton<IHttpClientService, HttpClientService>();
            services.AddSingleton<IServiceProvidersBusinessLogic, ServiceProvidersBusinessLogic>();            
            ConfigureCosmosDb(services);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Access2Justice Integration API", Version = "1.0.0", Description = "Access2Justice APIs for integration with external partners.", TermsOfService = "None" });
                c.TagActionsBy(api => api.GroupName);
                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
            services.AddSingleton<IServiceProvidersOrchestrator, ServiceProvidersOrchestrator>();

            // Inject the Service Providers at runtime
            var assembly = Assembly.LoadFrom(Assembly.GetExecutingAssembly().Location);
            var integrationAdaptersClassNames = Configuration.GetSection("IntegrationAdaptersFullyQualifiedClassNames").Get<List<string>>();
            foreach (var className in integrationAdaptersClassNames)
            {
                services.AddSingleton(typeof(IServiceProviderAdapter), assembly.GetType(className));
            }
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

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration.GetValue<string>("Api:VirtualPath") + "/swagger/v1/swagger.json", "Access2Justice Integration API");
            });

        }
    }
}