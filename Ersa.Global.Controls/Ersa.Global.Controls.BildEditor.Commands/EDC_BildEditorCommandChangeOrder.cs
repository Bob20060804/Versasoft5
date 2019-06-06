#define TRACE
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Commands
{
	public class EDC_BildEditorCommandChangeOrder : EDC_BildEditorCommandBase
	{
		private readonly List<int> m_lstVorher;

		private List<int> m_lstNachher;

		public EDC_BildEditorCommandChangeOrder(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_FuelleListe(i_edcBildEditorCanvas.PRO_lstGrafikliste, ref m_lstVorher);
		}

		public void SUB_NeuerZustand(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_FuelleListe(i_edcBildEditorCanvas.PRO_lstGrafikliste, ref m_lstNachher);
		}

		public override void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_ReihenfolgeAendern(i_edcBildEditorCanvas.PRO_lstGrafikliste, m_lstVorher);
		}

		public override void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_ReihenfolgeAendern(i_edcBildEditorCanvas.PRO_lstGrafikliste, m_lstNachher);
		}

		private static void SUB_FuelleListe(VisualCollection i_lstGrafiken, ref List<int> i_lstZuFuellen)
		{
			i_lstZuFuellen = (from EDC_GrafikBasisObjekt g in i_lstGrafiken
			select g.PRO_i32ObjektId).ToList();
		}

		private static void SUB_ReihenfolgeAendern(VisualCollection i_lstGrafiken, IEnumerable<int> i_lstIndizes)
		{
			List<EDC_GrafikBasisObjekt> list = new List<EDC_GrafikBasisObjekt>();
			foreach (int i32Id in i_lstIndizes)
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = i_lstGrafiken.Cast<EDC_GrafikBasisObjekt>().FirstOrDefault((EDC_GrafikBasisObjekt i_edcGrafik) => i_edcGrafik.PRO_i32ObjektId == i32Id);
				if (eDC_GrafikBasisObjekt != null)
				{
					list.Add(eDC_GrafikBasisObjekt);
					i_lstGrafiken.Remove(eDC_GrafikBasisObjekt);
				}
			}
			foreach (EDC_GrafikBasisObjekt item in list)
			{
				i_lstGrafiken.Add(item);
			}
		}

		[Conditional("DEBUG")]
		private static void SUB_Dump(IEnumerable<int> i_lstListe, string i_strText)
		{
			Trace.WriteLine(string.Empty);
			Trace.WriteLine(i_strText);
			Trace.WriteLine(string.Empty);
			string text = string.Empty;
			foreach (int item in i_lstListe)
			{
				text += item.ToString(CultureInfo.InvariantCulture);
				text += " ";
			}
			Trace.WriteLine(text);
		}
	}
}
