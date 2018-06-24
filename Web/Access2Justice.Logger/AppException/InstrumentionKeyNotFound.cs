using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Logger.AppException
{
	class InstrumentionKeyNotFound : Exception
	{
		public InstrumentionKeyNotFound()
		{

		}
		public InstrumentionKeyNotFound(string apptype)
			: base(String.Format("InstrumentionKey is not available from config files of application type : {0}. Please add config details to setting.xml or app.config as per doumentation.", apptype))
		{
		}
	}
}
