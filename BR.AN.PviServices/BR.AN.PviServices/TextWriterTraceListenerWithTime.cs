using System;
using System.Diagnostics;
using System.IO;

namespace BR.AN.PviServices
{
	public class TextWriterTraceListenerWithTime : TextWriterTraceListener
	{
		public TextWriterTraceListenerWithTime()
		{
		}

		public TextWriterTraceListenerWithTime(Stream stream)
			: base(stream)
		{
		}

		public TextWriterTraceListenerWithTime(string path)
			: base(path)
		{
		}

		public TextWriterTraceListenerWithTime(TextWriter writer)
			: base(writer)
		{
		}

		public TextWriterTraceListenerWithTime(Stream stream, string name)
			: base(stream, name)
		{
		}

		public TextWriterTraceListenerWithTime(string path, string name)
			: base(path, name)
		{
		}

		public TextWriterTraceListenerWithTime(TextWriter writer, string name)
			: base(writer, name)
		{
		}

		public override void WriteLine(string message)
		{
			Write(DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss.ffff "));
			base.WriteLine(message);
		}
	}
}
