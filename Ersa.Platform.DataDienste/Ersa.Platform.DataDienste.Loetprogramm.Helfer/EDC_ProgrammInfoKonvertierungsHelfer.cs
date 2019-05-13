using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.DataDienste.Loetprogramm.Helfer
{
	public static class EDC_ProgrammInfoKonvertierungsHelfer
	{
		public static EDC_ProgrammInfo FUN_edcProgrammInfoKonvertieren(EDC_LoetprogrammInfoDataAbfrage i_edcLpData, IEnumerable<ENUM_LoetprogrammStatus> i_enuStatus, IEnumerable<ENUM_LoetprogrammFreigabeStatus> i_enuFreigabestatus, IEnumerable<bool> i_enuFehlerhaft)
		{
			return new EDC_ProgrammInfo
			{
				PRO_i64Id = i_edcLpData.PRO_i64ProgrammId,
				PRO_i64BibId = i_edcLpData.PRO_i64BibliotheksId,
				PRO_i64VersionsId = i_edcLpData.PRO_i64VersionsId,
				PRO_i32VersionsNummer = i_edcLpData.PRO_i64Version,
				PRO_blnIstVorlageProgramm = i_edcLpData.PRO_blnIstDefault,
				PRO_dtmDatum = i_edcLpData.PRO_dtmBearbeitetAm,
				PRO_strBenutzername = i_edcLpData.PRO_strBenutzername,
				PROa_enmStatus = i_enuStatus.ToArray(),
				PROa_enmFreigabeStatus = i_enuFreigabestatus.ToArray(),
				PROa_enmFehlerhaft = i_enuFehlerhaft.ToArray(),
				PRO_i32SetNummer = i_edcLpData.PRO_i32SetNummer,
				PRO_strBibliotheksName = i_edcLpData.PRO_strBibliotheksName,
				PRO_strFirmenname = i_edcLpData.PRO_strBeschreibung,
				PRO_strKommentar = i_edcLpData.PRO_strNotizen,
				PRO_strEingangskontrolle = i_edcLpData.PRO_strEingangskontrolle,
				PRO_strAusgangskontrolle = i_edcLpData.PRO_strAusgangskontrolle,
				PRO_strProgrammName = i_edcLpData.PRO_strProgrammName
			};
		}

		public static EDC_LoetprogrammData FUN_edcProgrammInfoKonvertieren(EDC_ProgrammInfo i_edcPrgInfo)
		{
			return new EDC_LoetprogrammData
			{
				PRO_blnIstDefault = i_edcPrgInfo.PRO_blnIstVorlageProgramm,
				PRO_dtmAngelegtAm = i_edcPrgInfo.PRO_dtmDatum,
				PRO_dtmBearbeitetAm = i_edcPrgInfo.PRO_dtmDatum,
				PRO_i64BibliotheksId = i_edcPrgInfo.PRO_i64BibId,
				PRO_i64ProgrammId = i_edcPrgInfo.PRO_i64Id,
				PRO_i32Version = i_edcPrgInfo.PRO_i32VersionsNummer,
				PRO_strBeschreibung = i_edcPrgInfo.PRO_strFirmenname,
				PRO_strName = i_edcPrgInfo.PRO_strProgrammName,
				PRO_strNotizen = i_edcPrgInfo.PRO_strKommentar,
				PRO_strEingangskontrolle = i_edcPrgInfo.PRO_strEingangskontrolle,
				PRO_strAusgangskontrolle = i_edcPrgInfo.PRO_strAusgangskontrolle
			};
		}
	}
}
