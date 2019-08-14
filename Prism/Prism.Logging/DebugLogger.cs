using Prism.Properties;
using System;
using System.Globalization;

namespace Prism.Logging
{
	public class DebugLogger : ILoggerFacade
	{
		public void Log(string message, Category category, Priority priority)
		{
			string.Format(CultureInfo.InvariantCulture, Resources.DefaultDebugLoggerPattern, DateTime.Now, category.ToString().ToUpper(), message, priority);
		}
	}
}
