using Access2Justice.Logger.LoggingInterface;
using Access2Justice.Logger.Constants;
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
