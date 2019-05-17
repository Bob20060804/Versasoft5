using Ersa.Global.Controls.BildEditor.Grafik;
using System;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_RechteckEigenschaften : EDC_GrafikEigenschaften
	{
		public EDC_RechteckEigenschaften()
		{
		}

		public EDC_RechteckEigenschaften(EDC_RechteckGrafik i_edcRechteck)
		{
			if (i_edcRechteck == null)
			{
				throw new ArgumentNullException("i_edcRechteck");
			}
			base.PRO_dblLinks = i_edcRechteck.PRO_fdcStartPunkt.X;
			base.PRO_dblOben = i_edcRechteck.PRO_fdcEndPunkt.Y;
			base.PRO_dblRechts = i_edcRechteck.PRO_fdcEndPunkt.X;
			base.PRO_dblUnten = i_edcRechteck.PRO_fdcStartPunkt.Y;
			base.PRO_dblStrichStaerke = i_edcRechteck.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = i_edcRechteck.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = i_edcRechteck.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = i_edcRechteck.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = i_edcRechteck.PRO_blnIstSelektiert;
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_RechteckGrafik(base.PRO_dblLinks, base.PRO_dblOben, base.PRO_dblRechts, base.PRO_dblUnten, base.PRO_dblStrichStaerke, base.PRO_fdcStrichFarbe, base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
