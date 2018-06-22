using MS.DCU.FAME.AppLogging.LoggingInterface;
using MS.IT.FAME.AppLogging.Constants;
using System.Fabric;
using System.Fabric.Description;

namespace MS.DCU.FAME.AppLogging.Logging
{
	class SFAppSettingsProvider : IAppSettingsProvider
	{
		readonly CodePackageActivationContext activationContext = FabricRuntime.GetActivationContext();

		readonly System.Collections.ObjectModel.KeyedCollection<string, ConfigurationSection> connectionStringParameters;
		public SFAppSettingsProvider()
		{
			ConfigurationPackage configurationPackage = activationContext.GetConfigurationPackageObject("Config");
			connectionStringParameters = configurationPackage.Settings.Sections;
		}


		public string InstrumentationKey
		{
			get
			{
				return connectionStringParameters[AppConstants.ServiceFabricAppInsightContainer].Parameters[AppConstants.InstrumentationKey].Value;
				
			}
		}
	}
}
