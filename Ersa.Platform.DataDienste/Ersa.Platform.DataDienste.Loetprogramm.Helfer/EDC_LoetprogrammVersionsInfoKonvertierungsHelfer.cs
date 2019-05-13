using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;

namespace Ersa.Platform.DataDienste.Loetprogramm.Helfer
{
	public static class EDC_LoetprogrammVersionsInfoKonvertierungsHelfer
	{
		public static EDC_VersionsInfo FUN_edcKonvertieren(EDC_LoetprogrammVersionAbfrageData i_edcVersion, INF_SerialisierungsDienst i_edcSerialisierungsDienst)
		{
			EDC_VersionsInfo eDC_VersionsInfo = new EDC_VersionsInfo
			{
				PRO_i64VersionsId = i_edcVersion.PRO_i64VersionsId,
				PRO_i64ProgrammId = i_edcVersion.PRO_i64ProgrammId,
				PRO_enmVersionstatus = i_edcVersion.PRO_enmProgrammStatus,
				PRO_i32SetNummer = i_edcVersion.PRO_i32SetNummer,
				PRO_dtmVersionsDatum = i_edcVersion.PRO_dtmAngelegtAm,
				PRO_strKommentar = i_edcVersion.PRO_strKommentar,
				PRO_dtmBearbeitungsDatum = i_edcVersion.PRO_dtmBearbeitetAm,
				PRO_blnIstFehlerhaft = !i_edcVersion.PRO_blnValide,
				PRO_strBenutzer = i_edcVersion.PRO_strBenutzername,
				PRO_enmFreigabestatus = i_edcVersion.PRO_enmFreigabeStatus
			};
			if (!string.IsNullOrEmpty(i_edcVersion.PRO_strFreigabeNotizen))
			{
				eDC_VersionsInfo.PRO_edcFreigabeNotizen = i_edcSerialisierungsDienst.FUN_objDeserialisieren<EDC_FreigabeNotizen>(i_edcVersion.PRO_strFreigabeNotizen);
			}
			return eDC_VersionsInfo;
		}
	}
}
