using Ersa.Global.Controls.BildEditor.Grafik;
using System;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_EllipsenEigenschaften : EDC_GrafikEigenschaften
	{
		public EDC_EllipsenEigenschaften()
		{
		}

		public EDC_EllipsenEigenschaften(EDC_EllipsenGrafik i_edcEllipseGrafik)
		{
			if (i_edcEllipseGrafik == null)
			{
				throw new ArgumentNullException("i_edcEllipseGrafik");
			}
			base.PRO_dblLinks = i_edcEllipseGrafik.PRO_fdcStartPunkt.X;
			base.PRO_dblOben = i_edcEllipseGrafik.PRO_fdcStartPunkt.Y;
			base.PRO_dblRechts = i_edcEllipseGrafik.PRO_fdcEndPunkt.X;
			base.PRO_dblUnten = i_edcEllipseGrafik.PRO_fdcEndPunkt.Y;
			base.PRO_dblStrichStaerke = i_edcEllipseGrafik.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = i_edcEllipseGrafik.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = i_edcEllipseGrafik.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = i_edcEllipseGrafik.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = i_edcEllipseGrafik.PRO_blnIstSelektiert;
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_EllipsenGrafik(base.PRO_dblLinks, base.PRO_dblOben, base.PRO_dblRechts, base.PRO_dblUnten, base.PRO_dblStrichStaerke, base.PRO_fdcStrichFarbe, base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
