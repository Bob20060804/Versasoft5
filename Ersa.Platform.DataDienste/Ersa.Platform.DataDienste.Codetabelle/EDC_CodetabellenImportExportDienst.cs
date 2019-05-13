using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Codetabellen;
using Ersa.Platform.DataDienste.Codetabelle.Interfaces;
using System.ComponentModel.Composition;

namespace Ersa.Platform.DataDienste.Codetabelle
{
	[Export(typeof(INF_CodetabellenImportExportDienst))]
	public class EDC_CodetabellenImportExportDienst : INF_CodetabellenImportExportDienst
	{
		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
		{
			get;
			set;
		}

		public EDC_Codetabelle FUN_edcImport(string i_strDateiPfad)
		{
			string i_strFormatierterString = PRO_edcIoDienst.FUN_strDateiInhaltLesen(i_strDateiPfad);
			EDC_Codetabelle eDC_Codetabelle = PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_Codetabelle>(i_strFormatierterString);
			foreach (EDC_Codeeintrag item in eDC_Codetabelle.PRO_lstEintraege)
			{
				SUB_CodeTrimmen(item);
			}
			return eDC_Codetabelle;
		}

		public void SUB_Export(EDC_Codetabelle i_edcCodetabelle, string i_strDateiPfad)
		{
			string i_strDateiInhalt = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcCodetabelle);
			PRO_edcIoDienst.SUB_DateiInhaltSchreiben(i_strDateiPfad, i_strDateiInhalt);
		}

		private void SUB_CodeTrimmen(EDC_Codeeintrag i_edcCodeEintrag)
		{
			string text = i_edcCodeEintrag.PRO_strCode.Trim();
			if (text.Length > 399)
			{
				text = text.Substring(0, 399);
			}
			i_edcCodeEintrag.PRO_strCode = text;
		}
	}
}
