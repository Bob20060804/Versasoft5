using Ersa.Global.Dienste.Interfaces;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_ProzessStartDienst))]
	public class EDC_ProzessStartDienst : INF_ProzessStartDienst
	{
		public void SUB_ProzessStarten(string i_strDateiname)
		{
			Process.Start(i_strDateiname);
		}

		public void SUB_ProzessStarten(string i_strDateiname, string i_strArgumente)
		{
			Process.Start(i_strDateiname, i_strArgumente);
		}
	}
}
