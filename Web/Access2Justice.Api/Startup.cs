namespace Access2Justice.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;    
    using Swashbuckle.AspNetCore.Swagger;
    using Access2Justice.CognitiveServices;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Access2Justice.CosmosDb;
    using Access2Justice.Shared;
    using Access2Justice.Shared.Interfaces;
    using System;

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


            services.Configure<App>(Configuration.GetSection("App"));


            services.AddSingleton<ILuisHelper, LuisHelper>();
            services.AddTransient<IHttpClientService, HttpClientService>();

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
          
            //Swagger details
            SwaggerConfig.Register(app);

        }
    }
}
