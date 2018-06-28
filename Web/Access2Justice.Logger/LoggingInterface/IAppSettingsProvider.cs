using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Logger.LoggingInterface
{
	interface IAppSettingsProvider
	{
		string InstrumentationKey { get; }
	}
}
