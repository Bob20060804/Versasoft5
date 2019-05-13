using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Produktionssteuerung;
using Ersa.Platform.Common.Produktionssteuerung;

namespace Ersa.Platform.DataDienste.Produktionssteuerung.Helfer
{
	public static class EDC_ProduktionssteuerungsKonvertierungsHelfer
	{
		public static EDC_Produktionssteuerungsdaten FUN_edcKonvertieren(INF_SerialisierungsDienst i_edcJsonSerialisierungsDienst, EDC_ProduktionssteuerungData i_edcProduktionssteuerungData)
		{
			return new EDC_Produktionssteuerungsdaten
			{
				PRO_i64ProduktionssteuerungId = i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId,
				PRO_i64MaschinenId = i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId,
				PRO_blnIstAktiv = i_edcProduktionssteuerungData.PRO_blnIstAktiv,
				PRO_dtmAngelegtAm = i_edcProduktionssteuerungData.PRO_dtmAngelegtAm,
				PRO_i64AngelegtVon = i_edcProduktionssteuerungData.PRO_i64AngelegtVon,
				PRO_dtmBearbeitetAm = i_edcProduktionssteuerungData.PRO_dtmBearbeitetAm,
				PRO_i64BearbeitetVon = i_edcProduktionssteuerungData.PRO_i64BearbeitetVon,
				PRO_strBeschreibung = i_edcProduktionssteuerungData.PRO_strBeschreibung,
				PRO_edcProduktionsEinstellungen = i_edcJsonSerialisierungsDienst.FUN_objDeserialisieren<EDC_ProduktionsEinstellungen>(i_edcProduktionssteuerungData.PRO_strEinstellungen)
			};
		}

		public static EDC_ProduktionssteuerungData FUN_edcKonvertieren(INF_SerialisierungsDienst i_edcJsonSerialisierungsDienst, EDC_Produktionssteuerungsdaten i_edcProduktionssteuerungDaten)
		{
			return new EDC_ProduktionssteuerungData
			{
				PRO_i64ProduktionssteuerungId = i_edcProduktionssteuerungDaten.PRO_i64ProduktionssteuerungId,
				PRO_i64MaschinenId = i_edcProduktionssteuerungDaten.PRO_i64ProduktionssteuerungId,
				PRO_blnIstAktiv = i_edcProduktionssteuerungDaten.PRO_blnIstAktiv,
				PRO_dtmAngelegtAm = i_edcProduktionssteuerungDaten.PRO_dtmAngelegtAm,
				PRO_i64AngelegtVon = i_edcProduktionssteuerungDaten.PRO_i64AngelegtVon,
				PRO_dtmBearbeitetAm = i_edcProduktionssteuerungDaten.PRO_dtmBearbeitetAm,
				PRO_i64BearbeitetVon = i_edcProduktionssteuerungDaten.PRO_i64BearbeitetVon,
				PRO_strBeschreibung = i_edcProduktionssteuerungDaten.PRO_strBeschreibung,
				PRO_strEinstellungen = i_edcJsonSerialisierungsDienst.FUN_strSerialisieren(i_edcProduktionssteuerungDaten.PRO_edcProduktionsEinstellungen)
			};
		}
	}
}
