using Access2Justice.Logger.LoggingInterface;
using MS.IT.FAME.AppLogging.Constants;
using System.Configuration;

namespace Access2Justice.Logger.Logging
{
	class SAAppSettingsProvider : IAppSettingsProvider
	{
		
		public string InstrumentationKey
		{
			get
			{
				return ConfigurationManager.AppSettings[AppConstants.InstrumentationKey].ToString();

			}
		}
	}
}
