using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Linq;

namespace Ersa.Global.Controls.BildEditor.Commands
{
	public class EDC_BildEditorCommandAdd : EDC_BildEditorCommandBase
	{
		private readonly EDC_GrafikEigenschaften m_objObjektClone;

		public EDC_BildEditorCommandAdd(EDC_GrafikBasisObjekt i_fdcObject)
		{
			m_objObjektClone = i_fdcObject.FUN_edcSerialisiereObjekt();
		}

		public override void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = i_edcBildEditorCanvas.PRO_lstGrafikliste.Cast<EDC_GrafikBasisObjekt>().FirstOrDefault((EDC_GrafikBasisObjekt i_edcGrafik) => i_edcGrafik.PRO_i32ObjektId == m_objObjektClone.PRO_i32GrafikObjektId);
			if (eDC_GrafikBasisObjekt != null)
			{
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Remove(eDC_GrafikBasisObjekt);
			}
		}

		public override void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_edcBildEditorCanvas.SUB_UnselektiereAlleGrafikObjekte();
			i_edcBildEditorCanvas.PRO_lstGrafikliste.Add(m_objObjektClone.FUN_edcErstelleGrafikObjekt());
			i_edcBildEditorCanvas.SUB_AuschnittAktualisieren();
		}
	}
}
