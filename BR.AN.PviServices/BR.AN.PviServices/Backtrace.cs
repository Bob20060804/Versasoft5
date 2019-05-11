using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class Backtrace
	{
		internal Backtrace propNextBacktrace;

		internal uint propTaskIdx;

		internal uint propInfo;

		internal uint propParamCount;

		internal uint propAddress;

		internal string propFunctionName;

		internal uint[] propParameter;

		internal Callstack propCallstack;

		internal FunctionInfo propFunctionInfo;

		internal PCInfo propPCInfo;

		public Backtrace NextBacktrace => propNextBacktrace;

		[CLSCompliant(false)]
		public uint Paramcount
		{
			get
			{
				return propParamCount;
			}
		}

		[CLSCompliant(false)]
		public uint Address
		{
			get
			{
				return propAddress;
			}
		}

		[CLSCompliant(false)]
		public string FunctionName
		{
			get
			{
				return propFunctionName;
			}
		}

		[CLSCompliant(false)]
		public uint[] Parameter
		{
			get
			{
				return propParameter;
			}
		}

		[CLSCompliant(false)]
		public uint Info
		{
			get
			{
				return propInfo;
			}
		}

		[CLSCompliant(false)]
		public uint TaskIndex
		{
			get
			{
				return propTaskIdx;
			}
		}

		public Callstack Callstack => propCallstack;

		public FunctionInfo FunctionInfo => propFunctionInfo;

		public PCInfo PCInfo => propPCInfo;

		internal Backtrace()
		{
			propTaskIdx = uint.MaxValue;
		}

		internal Backtrace(ExceptionTraceRecord traceRecord)
		{
			propTaskIdx = uint.MaxValue;
			propInfo = traceRecord.brmInfoFlag;
			propParamCount = traceRecord.paramCnt;
			propAddress = traceRecord.funcAddr;
			propFunctionName = traceRecord.funcName;
			propParameter = new uint[traceRecord.paramCnt];
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < propParamCount; i++)
			{
				text += $"{propParameter[i]:X8} ";
			}
			return "Address=\"" + propAddress + ((propCallstack != null) ? ("\" CallStack=\"" + propCallstack.ToString()) : "") + ((propFunctionInfo != null) ? ("\" FunctionInfo=\"" + propFunctionInfo.ToString()) : "") + ((propFunctionName != null) ? ("\" FunctionName=\"" + propFunctionName) : "") + "\" Info=\"0x" + $"{propInfo:X8}" + "\" Parameter=\"" + ((0 < propParamCount) ? text : propParamCount.ToString()) + "\"" + ((propPCInfo != null) ? (" " + propPCInfo.ToString()) : "") + " TaskIdx=\"0x" + $"{propTaskIdx:X8}" + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string text = "";
			string str = "";
			ArrayList arrayList = new ArrayList();
			arrayList.Add($"<tr>\r\n<td align=\"left\" valign=\"top\">Address</td>\r\n<td>{Address}</td>\r\n</tr>\r\n");
			arrayList.Add($"<tr>\r\n<td align=\"left\" valign=\"top\">FunctionName</td>\r\n<td>{FunctionName}</td>\r\n</tr>\r\n");
			arrayList.Add($"<tr>\r\n<td align=\"left\" valign=\"top\">Info</td>\r\n<td>{Info}</td>\r\n</tr>\r\n");
			arrayList.Add($"<tr>\r\n<td align=\"left\" valign=\"top\">TaskIndex</td>\r\n<td>{TaskIndex}</td>\r\n</tr>\r\n");
			arrayList.Add("<tr>\r\n<td align=\"left\" valign=\"top\">Parameters</td>\r\n<td>\r\n");
			for (int i = 0; i < propParamCount; i++)
			{
				str = ((i != 0) ? (str + $"<br>{propParameter[i]:X8}") : $"{propParameter[i]:X8}");
			}
			arrayList.Add(str + "</td>\r\n</tr>\r\n");
			for (int i = 0; i < arrayList.Count; i++)
			{
				text += arrayList[i].ToString();
			}
			return text;
		}

		internal virtual string ToStringCSV()
		{
			return $"\"{Address}\";\"{FunctionName}\";\"0x{Info:X8}\";\"0x{TaskIndex:X8}\";";
		}

		public bool ReplaceFunctionName(string pFunctionName)
		{
			if (propFunctionName != null && 0 < propFunctionName.Length && pFunctionName != null && 0 < pFunctionName.Length)
			{
				propFunctionName = pFunctionName;
				return true;
			}
			return false;
		}
	}
}
