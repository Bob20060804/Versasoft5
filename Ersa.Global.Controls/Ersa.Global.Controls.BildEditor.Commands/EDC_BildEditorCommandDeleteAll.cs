using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Collections.Generic;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Commands
{
	public class EDC_BildEditorCommandDeleteAll : EDC_BildEditorCommandBase
	{
		private readonly List<EDC_GrafikEigenschaften> m_lstClone;

		public EDC_BildEditorCommandDeleteAll(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			m_lstClone = new List<EDC_GrafikEigenschaften>();
			VisualCollection.Enumerator enumerator = i_edcBildEditorCanvas.PRO_lstGrafikliste.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				m_lstClone.Add(eDC_GrafikBasisObjekt.FUN_edcSerialisiereObjekt());
			}
		}

		public override void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			foreach (EDC_GrafikEigenschaften item in m_lstClone)
			{
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Add(item.FUN_edcErstelleGrafikObjekt());
			}
			i_edcBildEditorCanvas.SUB_AuschnittAktualisieren();
		}

		public override void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_edcBildEditorCanvas.PRO_lstGrafikliste.Clear();
		}
	}
}
