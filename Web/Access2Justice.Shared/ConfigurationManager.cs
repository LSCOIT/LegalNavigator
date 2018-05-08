using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationBuilder _configurationBuilder;

        public ConfigurationManager(IConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public T Bind<T>(string appsettingsFileDirectory, string sectionName) where T : new()
        {
            var builder = _configurationBuilder
                .SetBasePath(appsettingsFileDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var cosmosDbConfig = new T();
            configuration.GetSection(sectionName).Bind(cosmosDbConfig);

            return cosmosDbConfig;
        }
    }
}