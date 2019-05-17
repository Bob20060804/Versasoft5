using Ersa.Global.Controls.BildEditor.Grafik;
using System;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_PunktEigenschaften : EDC_GrafikEigenschaften
	{
		public EDC_PunktEigenschaften()
		{
		}

		public EDC_PunktEigenschaften(EDC_PunktGrafik point)
		{
			if (point == null)
			{
				throw new ArgumentNullException("point");
			}
			base.PRO_fdcStartPunkt = point.PRO_fdcPosition;
			base.PRO_dblStrichStaerke = point.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = point.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = point.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = point.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = point.PRO_blnIstSelektiert;
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_PunktGrafik(base.PRO_fdcStartPunkt, base.PRO_dblStrichStaerke, base.PRO_fdcStrichFarbe, base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
