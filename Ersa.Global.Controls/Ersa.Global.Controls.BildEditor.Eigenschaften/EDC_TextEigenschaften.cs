using Ersa.Global.Controls.BildEditor.Converter;
using Ersa.Global.Controls.BildEditor.Grafik;
using System;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public class EDC_TextEigenschaften : EDC_GrafikEigenschaften
	{
		public string PRO_strText
		{
			get;
			set;
		}

		public string PRO_strFontFamilyName
		{
			get;
			set;
		}

		public string PRO_strFontStyle
		{
			get;
			set;
		}

		public string PRO_strFontWeight
		{
			get;
			set;
		}

		public string PRO_strFontStretch
		{
			get;
			set;
		}

		public double PRO_dblFontSize
		{
			get;
			set;
		}

		public EDC_TextEigenschaften()
		{
		}

		public EDC_TextEigenschaften(EDC_TextGrafik i_edcTextGrafik)
		{
			if (i_edcTextGrafik == null)
			{
				throw new ArgumentNullException("i_edcTextGrafik");
			}
			base.PRO_dblLinks = i_edcTextGrafik.PRO_fdcStartPunkt.X;
			base.PRO_dblOben = i_edcTextGrafik.PRO_fdcEndPunkt.Y;
			base.PRO_dblRechts = i_edcTextGrafik.PRO_fdcEndPunkt.X;
			base.PRO_dblUnten = i_edcTextGrafik.PRO_fdcStartPunkt.Y;
			base.PRO_dblStrichStaerke = i_edcTextGrafik.PRO_dblStrichStaerke;
			base.PRO_fdcStrichFarbe = i_edcTextGrafik.PRO_fdcGrafikFarbe;
			base.PRO_dblSkalierung = i_edcTextGrafik.PRO_dblSkalierung;
			base.PRO_i32GrafikObjektId = i_edcTextGrafik.PRO_i32ObjektId;
			base.PRO_blnIstSelektiert = i_edcTextGrafik.PRO_blnIstSelektiert;
			PRO_strText = i_edcTextGrafik.PRO_strText;
			PRO_strFontFamilyName = i_edcTextGrafik.PRO_strFontFamilyName;
			PRO_dblFontSize = i_edcTextGrafik.PRO_dblFontSize;
			PRO_strFontStyle = EDC_FontConverter.FUN_strFontStyleToString(i_edcTextGrafik.PRO_fdcFontStyle);
			PRO_strFontWeight = EDC_FontConverter.FUN_strFontWeightToString(i_edcTextGrafik.PRO_fdcFontWeight);
			PRO_strFontStretch = EDC_FontConverter.FUN_strFontStretchToString(i_edcTextGrafik.PRO_fdcFontStretch);
		}

		public override EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt()
		{
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = new EDC_TextGrafik(PRO_strText, base.PRO_dblLinks, base.PRO_dblOben, base.PRO_dblRechts, base.PRO_dblUnten, base.PRO_fdcStrichFarbe, PRO_dblFontSize, PRO_strFontFamilyName, EDC_FontConverter.FUN_fdcFontStyleFromString(PRO_strFontStyle), EDC_FontConverter.FUN_fdcFontWeightFromString(PRO_strFontWeight), EDC_FontConverter.FUN_fdcFontStretchFromString(PRO_strFontStretch), base.PRO_dblSkalierung);
			if (base.PRO_i32GrafikObjektId != 0)
			{
				eDC_GrafikBasisObjekt.PRO_i32ObjektId = base.PRO_i32GrafikObjektId;
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = base.PRO_blnIstSelektiert;
			}
			return eDC_GrafikBasisObjekt;
		}
	}
}
