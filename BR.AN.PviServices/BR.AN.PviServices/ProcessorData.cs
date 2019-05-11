using System;

namespace BR.AN.PviServices
{
	public class ProcessorData
	{
		private bool isARM;

		internal uint propProgramCounter;

		internal uint _CurrentProgramStatusRegister;

		internal uint propEFlags;

		internal uint propErrorCode;

		[CLSCompliant(false)]
		public uint ProgramCounter
		{
			get
			{
				return propProgramCounter;
			}
		}

		[CLSCompliant(false)]
		public uint EFlags
		{
			get
			{
				return propEFlags;
			}
		}

		[CLSCompliant(false)]
		public uint CurrentProgramStatusRegister
		{
			get
			{
				return _CurrentProgramStatusRegister;
			}
		}

		[CLSCompliant(false)]
		public uint ErrorCode
		{
			get
			{
				return propErrorCode;
			}
		}

		internal ProcessorData()
		{
		}

		internal ProcessorData(ExceptionProcessorInfo processorInfo, bool isARMException)
		{
			isARM = isARMException;
			propProgramCounter = processorInfo.pc;
			if (isARMException)
			{
				propEFlags = (_CurrentProgramStatusRegister = processorInfo.eFlags);
				propErrorCode = 0u;
			}
			else
			{
				_CurrentProgramStatusRegister = 0u;
				propEFlags = processorInfo.eFlags;
				propErrorCode = processorInfo.excErrFrameCode;
			}
		}

		public override string ToString()
		{
			if (isARM)
			{
				return "CurrentProgramStatusRegister=\"0x" + $"{_CurrentProgramStatusRegister:X8}" + "\" ProgramCounter=\"0x" + $"{propProgramCounter:X8}" + "\"";
			}
			return "EFlags=\"" + propEFlags.ToString() + "\" ErrorCode=\"" + propErrorCode.ToString() + "\" ProgramCounter=\"0x" + $"{propProgramCounter:X8}" + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string str = "<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">ProcessorData</td>";
			str += "<td bordercolor=\"#C0C0C0\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" ";
			str += "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber7\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">";
			string text = $"<tr><td align=\"left\" valign=\"top\">ProgramCounter</td><td>{ProgramCounter}</td></tr>";
			string text2 = (!isARM) ? $"<tr><td align=\"left\" valign=\"top\">CurrentProgramStatusRegister</td><td>{_CurrentProgramStatusRegister}</td></tr>" : $"<tr><td align=\"left\" valign=\"top\">EFlags</td><td>{EFlags}</td></tr>";
			string text3 = $"<tr><td align=\"left\" valign=\"top\">ErrorCode</td><td>{ErrorCode}</td></tr></table>\r\n</td>\r\n</tr>\r\n";
			return $"{str}{text}{text2}{text3}";
		}

		internal virtual string ToStringCSV()
		{
			if (isARM)
			{
				return $"\"{ProgramCounter}\";\"{_CurrentProgramStatusRegister}\";";
			}
			return $"\"{ProgramCounter}\";\"{EFlags}\";\"{ErrorCode}\";";
		}
	}
}
