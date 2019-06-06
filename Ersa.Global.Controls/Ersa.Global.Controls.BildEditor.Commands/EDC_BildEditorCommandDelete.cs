#define TRACE
using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ersa.Global.Controls.BildEditor.Commands
{
	public class EDC_BildEditorCommandDelete : EDC_BildEditorCommandBase
	{
		private readonly List<EDC_GrafikEigenschaften> m_lstClone;

		private readonly List<int> m_lstIndizes;

		public EDC_BildEditorCommandDelete(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			m_lstClone = new List<EDC_GrafikEigenschaften>();
			m_lstIndizes = new List<int>();
			int num = 0;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				m_lstClone.Add(item.FUN_edcSerialisiereObjekt());
				m_lstIndizes.Add(num);
				num++;
			}
		}

		public override void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			int num = 0;
			foreach (EDC_GrafikEigenschaften item in m_lstClone)
			{
				int num2 = m_lstIndizes[num];
				if (num2 >= 0 && num2 <= i_edcBildEditorCanvas.PRO_lstGrafikliste.Count)
				{
					i_edcBildEditorCanvas.PRO_lstGrafikliste.Insert(num2, item.FUN_edcErstelleGrafikObjekt());
				}
				else
				{
					i_edcBildEditorCanvas.PRO_lstGrafikliste.Add(item.FUN_edcErstelleGrafikObjekt());
					Trace.WriteLine("EDC_BildEditorCommandDelete.SUB_Undo - incorrect index");
				}
				num++;
			}
			i_edcBildEditorCanvas.SUB_AuschnittAktualisieren();
		}

		public override void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			for (int num = i_edcBildEditorCanvas.PRO_lstGrafikliste.Count - 1; num >= 0; num--)
			{
				EDC_GrafikBasisObjekt edcGrafik = (EDC_GrafikBasisObjekt)i_edcBildEditorCanvas.PRO_lstGrafikliste[num];
				if (m_lstClone.Any((EDC_GrafikEigenschaften i_edcGrafik) => i_edcGrafik.PRO_i32GrafikObjektId == edcGrafik.PRO_i32ObjektId))
				{
					i_edcBildEditorCanvas.PRO_lstGrafikliste.RemoveAt(num);
				}
			}
		}
	}
}
