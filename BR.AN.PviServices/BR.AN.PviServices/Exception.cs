using System;

namespace BR.AN.PviServices
{
	public class Exception
	{
		internal uint propBacktraceCount;

		internal uint propDataLength;

		internal string propARVersion;

		internal ProcessorData propProcessorData;

		internal TaskData propTaskData;

		internal MemoryData propMemoryData;

		internal Backtrace propBacktrace;

		internal ExceptionType propType;

		[CLSCompliant(false)]
		public uint BacktraceCount
		{
			get
			{
				return propBacktraceCount;
			}
		}

		[CLSCompliant(false)]
		public uint DataLength
		{
			get
			{
				return propDataLength;
			}
		}

		public string ArVersion => propARVersion;

		public ProcessorData ProcessorData => propProcessorData;

		public TaskData TaskData => propTaskData;

		public MemoryData MemoryData => propMemoryData;

		public Backtrace Backtrace => propBacktrace;

		public ExceptionType Type => propType;

		public Exception()
		{
		}

		internal Exception(ExceptionHeader header)
		{
			propBacktraceCount = header.traceRec;
			propDataLength = header.excInfoSize;
			propARVersion = header.arVersion;
		}

		public override string ToString()
		{
			return "ArVersion=\"" + propARVersion + "\" Type=\"" + propType.ToString() + "\" DataLength=\"" + propDataLength.ToString() + "\" BacktraceCount=\"" + propBacktraceCount.ToString() + "\"" + ((propBacktrace != null) ? (" " + propBacktrace.ToString()) : "") + ((propMemoryData != null) ? (" " + propMemoryData.ToString()) : "") + ((propProcessorData != null) ? (" " + propProcessorData.ToString()) : "") + ((propTaskData != null) ? (" " + propTaskData.ToString()) : "");
		}

		internal virtual string ToStringHTM()
		{
			string text = $"<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Exception</td>\r\n<td bordercolor=\"#C0C0C0\">{Type}</td></tr>\r\n";
			string text2 = $"<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">BacktraceCount</td>\r\n<td bordercolor=\"#C0C0C0\">{BacktraceCount}</td></tr>\r\n";
			string text3 = $"<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">DataLength</td>\r\n<td bordercolor=\"#C0C0C0\">{DataLength}</td></tr>\r\n";
			string text4 = $"<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">ArVersion</td>\r\n<td bordercolor=\"#C0C0C0\">{ArVersion}</td></tr>\r\n";
			return $"{text}{text2}{text3}{text4}";
		}

		internal virtual string ToStringCSV()
		{
			return $"\"{Type}\";\"{BacktraceCount}\";\"{DataLength}\";\"{ArVersion}\";";
		}
	}
}
