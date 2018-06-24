using Access2Justice.Logger.AppException;
using Access2Justice.Logger.LoggingInterface;

namespace Access2Justice.Logger.Logging
{
	class AppSettingHelper
	{
		public static string GetInstrumentionKey()
		{
			string key;
			IAppSettingsProvider appSettings;
            appSettings = new SAAppSettingsProvider();
			key = appSettings.InstrumentationKey;
			if (key == null)
			{				
					throw new InstrumentionKeyNotFound("No Instrumentation Key Found");			
			}
			return key;
		}
	}
}
