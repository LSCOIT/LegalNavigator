using Access2Justice.CosmosDb;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
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
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object CosmosDbConfiguration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IConfigurationManager, ConfigurationManager>();
            services.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();

            // configure and inject CosmosDb client
            ICosmosDbConfigurations cosmosDbConfigurations = new CosmosDbConfigurations();
            Configuration.GetSection("cosmosDb").Bind(cosmosDbConfigurations);
            services.AddSingleton<IDocumentClient>(x =>
                new DocumentClient(new Uri(cosmosDbConfigurations.Endpoint), cosmosDbConfigurations.AuthKey));
            services.AddSingleton(typeof(IBackendDatabaseService), typeof(CosmosDbService));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Access2Justice API", Version = "v1" });
                c.TagActionsBy(api => api.GroupName);
                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Access2Justice API V1");
            });
        }
    }
}
