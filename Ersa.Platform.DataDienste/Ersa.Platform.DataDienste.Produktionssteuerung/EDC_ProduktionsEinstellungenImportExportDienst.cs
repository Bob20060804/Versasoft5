using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Produktionssteuerung;
using Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces;
using System.ComponentModel.Composition;

namespace Ersa.Platform.DataDienste.Produktionssteuerung
{
	[Export(typeof(INF_ProduktionsEinstellungenImportExportDienst))]
	public class EDC_ProduktionsEinstellungenImportExportDienst : INF_ProduktionsEinstellungenImportExportDienst
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

		public EDC_ProduktionsEinstellungen FUN_edcImport(string i_strDateiPfad)
		{
			string i_strFormatierterString = PRO_edcIoDienst.FUN_strDateiInhaltLesen(i_strDateiPfad);
			return PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_ProduktionsEinstellungen>(i_strFormatierterString);
		}

		public void SUB_Export(EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen, string i_strDateiPfad)
		{
			string i_strDateiInhalt = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcProduktionsEinstellungen);
			PRO_edcIoDienst.SUB_DateiInhaltSchreiben(i_strDateiPfad, i_strDateiInhalt);
		}
	}
}
