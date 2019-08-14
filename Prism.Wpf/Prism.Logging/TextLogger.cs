using Prism.Properties;
using System;
using System.Globalization;
using System.IO;

namespace Prism.Logging
{
	public class TextLogger : ILoggerFacade, IDisposable
	{
		private readonly TextWriter writer;

		public TextLogger()
			: this(Console.Out)
		{
		}

		public TextLogger(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

		public void Log(string message, Category category, Priority priority)
		{
			string value = string.Format(CultureInfo.InvariantCulture, Prism.Properties.Resources.DefaultTextLoggerPattern, DateTime.Now, category.ToString().ToUpper(CultureInfo.InvariantCulture), message, priority.ToString());
			writer.WriteLine(value);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && writer != null)
			{
				writer.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
