using Access2Justice.Logger.LoggingInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Access2Justice.Logger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAppFameLogger
	{

		/// <summary>
		/// This method is used to get logger which help user to Event, Error, Trace, Request.
		/// </summary>
		/// <returns></returns>
		ILogger GetFameLogger();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputMessage"></param>
		/// <param name="correlationId"></param>
		/// <returns></returns>
		IDictionary<string, string> SetupBaseLoggingInfo(string inputMessage, string correlationId);
	}
}
