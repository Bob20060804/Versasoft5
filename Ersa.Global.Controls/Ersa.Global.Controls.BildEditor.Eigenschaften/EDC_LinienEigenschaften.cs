using Ersa.Global.Controls.BildEditor.Grafik;
using System;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_LinienEigenschaften : EDC_GrafikEigenschaften
	{
		public EDC_LinienEigenschaften()
		{
		}

		public EDC_LinienEigenschaften(EDC_LinienGrafik line)
		{
			if (line == null)
			{
				throw new ArgumentNullException("line");
			}
			base.PRO_fdcStartPunkt = line.PRO_fdcStartPunkt;
			base.PRO_fdcEndPunkt = line.PRO_fdcEndPunkt;
			base.PRO_dblStrichStaerke = line.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = line.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = line.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = line.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = line.PRO_blnIstSelektiert;
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_LinienGrafik(base.PRO_fdcStartPunkt, base.PRO_fdcEndPunkt, base.PRO_dblStrichStaerke, base.PRO_fdcStrichFarbe, base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
