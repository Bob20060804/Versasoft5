using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Windows;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_MehrfachLinienEigensachften : EDC_GrafikEigenschaften
	{
		public Point[] PRO_fdcPunkte
		{
			get;
			set;
		}

		public EDC_MehrfachLinienEigensachften()
		{
		}

		public EDC_MehrfachLinienEigensachften(EDC_MehrfachLinienGrafik polyLine)
		{
			if (polyLine == null)
			{
				throw new ArgumentNullException("polyLine");
			}
			PRO_fdcPunkte = polyLine.FUN_fdcHolePunkte();
			base.PRO_dblStrichStaerke = polyLine.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = polyLine.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = polyLine.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = polyLine.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = polyLine.PRO_blnIstSelektiert;
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_MehrfachLinienGrafik(PRO_fdcPunkte, base.PRO_dblStrichStaerke, base.PRO_fdcStrichFarbe, base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
