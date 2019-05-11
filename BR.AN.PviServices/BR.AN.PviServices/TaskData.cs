using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TaskData
	{
		internal ARMRegisters _ARMRegisters;

		internal uint propId;

		internal uint propPriority;

		internal string propName;

		internal uint propStackBegin;

		internal uint propStackEnd;

		internal uint propStackSize;

		internal uint propRegisterEax;

		internal uint propRegisterEbx;

		internal uint propRegisterEcx;

		internal uint propRegisterEdx;

		internal uint propRegisterEsi;

		internal uint propRegisterEdi;

		internal uint propRegisterEip;

		internal uint propRegisterEsp;

		internal uint propRegisterEbp;

		internal uint propRegisterEflags;

		public ARMRegisters ARMRegisters => _ARMRegisters;

		[CLSCompliant(false)]
		public uint Id
		{
			get
			{
				return propId;
			}
		}

		[CLSCompliant(false)]
		public uint Priority
		{
			get
			{
				return propPriority;
			}
		}

		[CLSCompliant(false)]
		public string Name
		{
			get
			{
				return propName;
			}
		}

		[CLSCompliant(false)]
		public uint StackBegin
		{
			get
			{
				return propStackBegin;
			}
		}

		[CLSCompliant(false)]
		public uint StackEnd
		{
			get
			{
				return propStackEnd;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEAX
		{
			get
			{
				return propRegisterEax;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEBX
		{
			get
			{
				return propRegisterEbx;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterECX
		{
			get
			{
				return propRegisterEcx;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEDX
		{
			get
			{
				return propRegisterEdx;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterESI
		{
			get
			{
				return propRegisterEsi;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEDI
		{
			get
			{
				return propRegisterEdi;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEIP
		{
			get
			{
				return propRegisterEip;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEBP
		{
			get
			{
				return propRegisterEbp;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterESP
		{
			get
			{
				return propRegisterEsp;
			}
		}

		[CLSCompliant(false)]
		public uint RegisterEFLAGS
		{
			get
			{
				return propRegisterEflags;
			}
		}

		[CLSCompliant(false)]
		public uint StackSize
		{
			get
			{
				return propStackSize;
			}
		}

		internal TaskData()
		{
		}

		internal TaskData(ExceptionTaskInfo taskInfo)
		{
			propId = taskInfo.taskId;
			propPriority = taskInfo.taskPrio;
			propName = taskInfo.taskName;
			propStackBegin = taskInfo.stackBottom;
			propStackEnd = taskInfo.stackEnd;
			propStackSize = taskInfo.stackSize;
			propRegisterEax = taskInfo.eax;
			propRegisterEbx = taskInfo.ebx;
			propRegisterEcx = taskInfo.ecx;
			propRegisterEdx = taskInfo.edx;
			propRegisterEsi = taskInfo.esi;
			propRegisterEdi = taskInfo.edi;
			propRegisterEip = taskInfo.eip;
			propRegisterEsp = taskInfo.esp;
			propRegisterEbp = taskInfo.ebp;
			propRegisterEflags = taskInfo.eflags;
			_ARMRegisters = null;
		}

		internal TaskData(ARMExceptionTaskInfo taskInfo)
		{
			propId = taskInfo.taskId;
			propPriority = taskInfo.taskPrio;
			propName = taskInfo.taskName;
			propStackBegin = taskInfo.stackBottom;
			propStackEnd = taskInfo.stackEnd;
			propStackSize = taskInfo.stackSize;
			propRegisterEax = 0u;
			propRegisterEbx = 0u;
			propRegisterEcx = 0u;
			propRegisterEdx = 0u;
			propRegisterEsi = 0u;
			propRegisterEdi = 0u;
			propRegisterEip = 0u;
			propRegisterEsp = 0u;
			propRegisterEbp = 0u;
			propRegisterEflags = 0u;
			_ARMRegisters = new ARMRegisters(taskInfo);
		}

		public override string ToString()
		{
			string text = "ID=\"" + propId.ToString() + "\" Name=\"" + propName.ToString() + "\" Priority=\"" + propPriority.ToString();
			if (_ARMRegisters != null)
			{
				return text + "\" GPRegister00=\"" + _ARMRegisters.GeneralPurposeRegister00.ToString() + "\" GPRegister01=\"" + _ARMRegisters.GeneralPurposeRegister01.ToString() + "\" GPRegister02=\"" + _ARMRegisters.GeneralPurposeRegister02.ToString() + "\" GPRegister03=\"" + _ARMRegisters.GeneralPurposeRegister03.ToString() + "\" GPRegister04=\"" + _ARMRegisters.GeneralPurposeRegister04.ToString() + "\" GPRegister05=\"" + _ARMRegisters.GeneralPurposeRegister05.ToString() + "\" GPRegister06=\"" + _ARMRegisters.GeneralPurposeRegister06.ToString() + "\" GPRegister07=\"" + _ARMRegisters.GeneralPurposeRegister07.ToString() + "\" GPRegister08=\"" + _ARMRegisters.GeneralPurposeRegister08.ToString() + "\" GPRegister09=\"" + _ARMRegisters.GeneralPurposeRegister09.ToString() + "\" GPRegister10=\"" + _ARMRegisters.GeneralPurposeRegister10.ToString() + "\" FramePointer=\"" + _ARMRegisters.FramePointer.ToString() + "\" GPRegister12=\"" + _ARMRegisters.GeneralPurposeRegister12.ToString() + "\" StackPointer=\"" + _ARMRegisters.StackPointer.ToString() + "\" LinkRegister=\"" + _ARMRegisters.LinkRegister.ToString() + "\" ProgramCounter=\"" + _ARMRegisters.ProgramCounter.ToString() + "\" CurrentProgramStatusRegister=\"" + _ARMRegisters.CurrentProgramStatusRegister.ToString() + "\" TranslationTableBaseControlRegister=\"" + _ARMRegisters.TranslationTableBaseControlRegister.ToString() + "\"";
			}
			return text + "\" Eax=\"" + propRegisterEax.ToString() + "\" Ebp=\"" + propRegisterEbp.ToString() + "\" Ebx=\"" + propRegisterEbx.ToString() + "\" Ecx=\"" + propRegisterEcx.ToString() + "\" Edi=\"" + propRegisterEdi.ToString() + "\" Edx=\"" + propRegisterEdx.ToString() + "\" Eflags=\"" + propRegisterEflags.ToString() + "\" Eip=\"" + propRegisterEip.ToString() + "\" Esi=\"" + propRegisterEsi.ToString() + "\" Esp=\"" + propRegisterEsp.ToString() + "\" StackBegin=\"" + propStackBegin.ToString() + "\" StackEnd=\"" + propStackEnd.ToString() + "\" StackSize=\"" + propStackSize.ToString() + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string text = "";
			string text2 = "";
			ArrayList arrayList = new ArrayList();
			text2 = "<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">TaskData</td>";
			text2 += "<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" ";
			text2 += "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber3\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">";
			arrayList.Add(text2);
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">Id</td><td>{Id}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">Priority</td><td>{Priority}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">Name</td><td>{Name}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">StackBegin</td><td>{StackBegin}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">StackEnd</td><td>{StackEnd}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">StackSize</td><td>{StackSize}</td></tr>");
			if (_ARMRegisters != null)
			{
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister00</td><td>{_ARMRegisters.GeneralPurposeRegister00}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister01</td><td>{_ARMRegisters.GeneralPurposeRegister01}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister02</td><td>{_ARMRegisters.GeneralPurposeRegister02}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister03</td><td>{_ARMRegisters.GeneralPurposeRegister03}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister04</td><td>{_ARMRegisters.GeneralPurposeRegister04}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister05</td><td>{_ARMRegisters.GeneralPurposeRegister05}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister06</td><td>{_ARMRegisters.GeneralPurposeRegister06}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister07</td><td>{_ARMRegisters.GeneralPurposeRegister07}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister08</td><td>{_ARMRegisters.GeneralPurposeRegister08}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister09</td><td>{_ARMRegisters.GeneralPurposeRegister09}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister10</td><td>{_ARMRegisters.GeneralPurposeRegister10}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">FramePointer</td><td>{_ARMRegisters.FramePointer}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">GPRegister12</td><td>{_ARMRegisters.GeneralPurposeRegister12}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">StackPointer</td><td>{_ARMRegisters.StackPointer}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">LinkRegister</td><td>{_ARMRegisters.LinkRegister}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">ProgramCounter</td><td>{_ARMRegisters.ProgramCounter}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">CurrentProgramStatusRegister</td><td>{_ARMRegisters.CurrentProgramStatusRegister}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">TranslationTableBaseControlRegister</td><td>{_ARMRegisters.TranslationTableBaseControlRegister}</td></tr></table>\r\n</td>\r\n</tr>\r\n");
			}
			else
			{
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEAX</td><td>{RegisterEAX}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEBX</td><td>{RegisterEBX}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterECX</td><td>{RegisterECX}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEDX</td><td>{RegisterEDX}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterESI</td><td>{RegisterESI}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEDI</td><td>{RegisterEDI}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEIP</td><td>{RegisterEIP}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterESP</td><td>{RegisterESP}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEBP</td><td>{RegisterEBP}</td></tr>");
				arrayList.Add($"<tr><td align=\"left\" valign=\"top\">RegisterEFlags</td><td>{RegisterEFLAGS}</td></tr></table>\r\n</td>\r\n</tr>\r\n");
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				text += arrayList[i].ToString();
			}
			return text;
		}

		internal virtual string ToStringCSV()
		{
			string str = $"\"{Id}\";\"{Priority}\";\"{Name}\";\"{StackBegin}\";\"{StackEnd}\";\"{StackSize}\";";
			if (_ARMRegisters != null)
			{
				return str + $"\"{_ARMRegisters.GeneralPurposeRegister00}\";\"{_ARMRegisters.GeneralPurposeRegister01}\";\"{_ARMRegisters.GeneralPurposeRegister02}\";\"{_ARMRegisters.GeneralPurposeRegister03}\";\"{_ARMRegisters.GeneralPurposeRegister04}\";\"{_ARMRegisters.GeneralPurposeRegister05}\";\"{_ARMRegisters.GeneralPurposeRegister06}\";\"{_ARMRegisters.GeneralPurposeRegister07}\";\"{_ARMRegisters.GeneralPurposeRegister08}\";\"{_ARMRegisters.GeneralPurposeRegister09}\";\"{_ARMRegisters.GeneralPurposeRegister10}\";\"{_ARMRegisters.FramePointer}\";\"{_ARMRegisters.GeneralPurposeRegister12}\";\"{_ARMRegisters.StackPointer}\";\"{_ARMRegisters.LinkRegister}\";\"{_ARMRegisters.ProgramCounter}\";\"{_ARMRegisters.CurrentProgramStatusRegister}\";\"{_ARMRegisters.TranslationTableBaseControlRegister}\";";
			}
			return str + $"\"{RegisterEAX}\";\"{RegisterEBX}\";\"{RegisterECX}\";\"{RegisterEDX}\";\"{RegisterESI}\";\"{RegisterEDI}\";\"{RegisterEIP}\";\"{RegisterESP}\";\"{RegisterEBP}\";\"{RegisterEFLAGS}\";";
		}
	}
}
