using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Commands
{
	public class EDC_BildEditorCommandChangeState : EDC_BildEditorCommandBase
	{
		private readonly List<EDC_GrafikEigenschaften> m_lstVorher;

		private List<EDC_GrafikEigenschaften> m_lstNachher;

		public EDC_BildEditorCommandChangeState(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_FuelleListe(i_edcBildEditorCanvas.PRO_lstGrafikliste, ref m_lstVorher);
		}

		public void SUB_NeuerZustand(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_FuelleListe(i_edcBildEditorCanvas.PRO_lstGrafikliste, ref m_lstNachher);
		}

		public override void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_ErsetzeGrafiken(i_edcBildEditorCanvas.PRO_lstGrafikliste, m_lstVorher);
			i_edcBildEditorCanvas.SUB_AuschnittAktualisieren();
		}

		public override void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			SUB_ErsetzeGrafiken(i_edcBildEditorCanvas.PRO_lstGrafikliste, m_lstNachher);
			i_edcBildEditorCanvas.SUB_AuschnittAktualisieren();
		}

		private static void SUB_ErsetzeGrafiken(VisualCollection i_lstGrafiken, List<EDC_GrafikEigenschaften> i_edcEigenschaftenliste)
		{
			int i32Index;
			for (i32Index = 0; i32Index < i_lstGrafiken.Count; i32Index++)
			{
				EDC_GrafikEigenschaften eDC_GrafikEigenschaften = i_edcEigenschaftenliste.FirstOrDefault((EDC_GrafikEigenschaften i_edcGrafik) => i_edcGrafik.PRO_i32GrafikObjektId == ((EDC_GrafikBasisObjekt)i_lstGrafiken[i32Index]).PRO_i32ObjektId);
				if (eDC_GrafikEigenschaften != null)
				{
					i_lstGrafiken.RemoveAt(i32Index);
					i_lstGrafiken.Insert(i32Index, eDC_GrafikEigenschaften.FUN_edcErstelleGrafikObjekt());
				}
			}
		}

		private static void SUB_FuelleListe(VisualCollection i_lstGrafiken, ref List<EDC_GrafikEigenschaften> i_lstZuFuellen)
		{
			i_lstZuFuellen = (from EDC_GrafikBasisObjekt g in i_lstGrafiken
			where g.PRO_blnIstSelektiert
			select g.FUN_edcSerialisiereObjekt()).ToList();
		}
	}
}
