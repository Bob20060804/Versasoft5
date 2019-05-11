using System.Collections;

namespace BR.AN.PviServices
{
	public class MemoryData
	{
		internal byte[] propMemPc;

		internal byte[] propMemESP;

		public byte[] PC => propMemPc;

		public byte[] ESP => propMemESP;

		internal MemoryData()
		{
		}

		internal MemoryData(ExceptionMemoryInfo memoryInfo)
		{
			propMemPc = memoryInfo.memPc;
			propMemESP = memoryInfo.memEsp;
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < propMemESP.GetLength(0); i++)
			{
				text += $"{propMemESP.GetValue(i):X2}";
			}
			string text2 = "";
			for (int i = 0; i < propMemPc.GetLength(0); i++)
			{
				text2 += $"{propMemPc.GetValue(i):X2}";
			}
			return "ESP=\"" + text + "\" PC=\"" + text2 + "\"";
		}

		private string PCToString()
		{
			string text = "";
			for (int i = 0; i < propMemPc.GetLength(0); i++)
			{
				text += $"{propMemPc.GetValue(i):X2}";
			}
			return text;
		}

		private string ESPToString()
		{
			string text = "";
			for (int i = 0; i < propMemESP.GetLength(0); i++)
			{
				text += $"{propMemESP.GetValue(i):X2}";
			}
			return text;
		}

		internal virtual string ToStringHTM()
		{
			string text = "";
			string text2 = "";
			ArrayList arrayList = new ArrayList();
			text2 = "<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">MemoryData</td>";
			text2 += "<td bordercolor=\"#C0C0C0\" style=\"border-collapse: collapse\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" ";
			text2 += "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber4\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">";
			arrayList.Add(text2);
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">PC</td><td>{PCToString()}</td></tr>");
			arrayList.Add($"<tr><td align=\"left\" valign=\"top\">ESP</td><td>{ESPToString()}</td></tr></table>\r\n</td>\r\n</tr>\r\n");
			for (int i = 0; i < arrayList.Count; i++)
			{
				text += arrayList[i].ToString();
			}
			return text;
		}

		internal virtual string ToStringCSV()
		{
			return "\"" + PCToString() + "\";\"" + ESPToString() + "\";";
		}
	}
}
