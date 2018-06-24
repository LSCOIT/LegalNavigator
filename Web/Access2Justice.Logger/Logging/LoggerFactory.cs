using Access2Justice.Logger.LoggingInterface;
using Access2Justice.Logger.AppEnum;
using Access2Justice.Logger.Logging;

namespace Access2Justice.Logger.Logging
{
	/// <summary>
	/// Entry point for logger
	/// </summary>
	public static class LoggerFactory
	{
		/// <summary>
		/// Current supported logger is as AppInsightLogger. 
		/// To get AppInsightLogger you need to pass LoggerType.AppInsight (default to LoggerType.AppInsight and only supported) and InstrumentionKey (Optional)
		/// InstrumentionKey can be pick from app.config or settings.xml based onto the application type.
		/// for Standalone appliction - app.config file and Variable name must be InstrumentationKey
		/// for service fabric application - setting.xml and Section name must be ApplicationInsight and Variable name must be InstrumentationKey
		/// </summary>
		/// <param name="loggerType">Only support availabe for AppInsigh</param>
		/// <param name="key">InstrumentionKey can be pick from app.config or settings.xml based onto the application type.</param>
		/// <returns name ="ILogger"></returns>
		public static ILogger GetLogger(LoggerType loggerType = LoggerType.AppInsight, string key=null)
        {
            if(loggerType == LoggerType.AppInsight)
            {
				if (key == null) {
					key = AppSettingHelper.GetInstrumentionKey();
				}
				return AppInsightsLogger.GetInstance(key);
            }
            else
            {
                return null;
            }
            
           
        }
    }
}