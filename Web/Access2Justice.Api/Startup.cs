using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.CosmosDbService;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConfigurationManager, ConfigurationManager>();
            services.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();

            // configure CosmosDb service
            ICosmosDbConfigurations cosmosDbConfigurations = new CosmosDbConfigurations();
            Configuration.GetSection("cosmosDb").Bind(cosmosDbConfigurations);
            IDocumentClient documentClient = new DocumentClient(new Uri(cosmosDbConfigurations.Endpoint), cosmosDbConfigurations.AuthKey);
            services.AddSingleton(documentClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
