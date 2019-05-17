using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.UI.Programm.ViewModels;
using System;
using System.Linq;

namespace Ersa.Platform.UI.Programm.Helfer
{
	public static class EDC_ElementViewModelHelfer
	{
		private const int mC_i32AnzahlAngezeigterVersionen = 3;

		public static EDC_BibliothekViewModel FUN_edcBibliothekKonvertieren(EDC_BibliothekInfo i_edcBib, INF_LoetprogrammVerwaltungsDienst i_edcLoetprogrammVerwaltungsDienst, ENUM_LoetprogrammFreigabeArt i_enmLoetprogrammFreigabeArt, Func<string> i_delSuchbegriff, bool i_blnFehlerhafteProgrammFreigabenProgrammeIgnorieren = false, bool i_blnFehlerhafteProgrammArbeitsversionIgnorieren = false)
		{
			return new EDC_BibliothekViewModel(i_edcBib.PRO_i64BibliotheksId, i_edcBib.PRO_strBibliotheksName, i_enmLoetprogrammFreigabeArt, async delegate
			{
				if (i_edcLoetprogrammVerwaltungsDienst == null)
				{
					return Enumerable.Empty<EDC_ProgrammViewModel>();
				}
				return (from i_edcPrg in (await i_edcLoetprogrammVerwaltungsDienst.FUN_fdcBibliothekAuslesenAsync(i_edcBib.PRO_i64BibliotheksId, i_delSuchbegriff?.Invoke())).PRO_lstProgramme.Where(delegate(EDC_ProgrammInfo i_edcPrg)
				{
					if (i_blnFehlerhafteProgrammFreigabenProgrammeIgnorieren && !FUN_blnHatProgrammFehlerfreieFreigabe(i_edcPrg))
					{
						return false;
					}
					if (i_blnFehlerhafteProgrammArbeitsversionIgnorieren && !FUN_blnHatProgrammFehlerfreieArbeitsversion(i_edcPrg))
					{
						return false;
					}
					return true;
				})
				select FUN_edcProgrammKonvertieren(i_edcPrg, i_edcLoetprogrammVerwaltungsDienst, i_enmLoetprogrammFreigabeArt)).ToArray();
			});
		}

		public static EDC_ProgrammViewModel FUN_edcProgrammKonvertieren(EDC_ProgrammInfo i_edcPrg, INF_LoetprogrammVerwaltungsDienst i_edcLoetprogrammVerwaltungsDienst, ENUM_LoetprogrammFreigabeArt i_enmLoetprogrammFreigabeArt)
		{
			return new EDC_ProgrammViewModel(i_edcPrg.PRO_i64Id, i_edcPrg.PRO_strProgrammName, i_edcPrg.PRO_i64BibId, i_edcPrg.PRO_strBibliotheksName, async delegate
			{
				if (i_edcLoetprogrammVerwaltungsDienst == null)
				{
					return Enumerable.Empty<EDC_VersionViewModel>();
				}
				return (from i_edcVersion in (from i_edcVersion in await i_edcLoetprogrammVerwaltungsDienst.FUN_fdcLoetprogrammVersionsStapelHolenAsync(i_edcPrg.PRO_i64Id)
				orderby i_edcVersion.PRO_dtmBearbeitungsDatum descending
				select i_edcVersion).Take(3)
				select FUN_edcVersionKonvertieren(i_edcVersion, i_enmLoetprogrammFreigabeArt)).ToArray();
			})
			{
				PRO_dtmDatum = i_edcPrg.PRO_dtmDatum,
				PRO_strBenutzername = i_edcPrg.PRO_strBenutzername,
				PRO_blnIstFehlerhaft = i_edcPrg.PRO_blnIstFehlerhaft,
				PRO_strKommentar = i_edcPrg.PRO_strKommentar,
				PROa_enmStatus = i_edcPrg.PROa_enmStatus,
				PROa_enmFreigabeStatus = i_edcPrg.PROa_enmFreigabeStatus,
				PRO_enmFreigabeArt = i_enmLoetprogrammFreigabeArt
			};
		}

		public static EDC_VersionViewModel FUN_edcVersionKonvertieren(EDC_VersionsInfo i_edcVersion, ENUM_LoetprogrammFreigabeArt i_enmLoetprogrammFreigabeArt)
		{
			return new EDC_VersionViewModel(i_edcVersion.PRO_i64VersionsId, $"Version {i_edcVersion.PRO_i32SetNummer}", i_edcVersion.PRO_i64ProgrammId)
			{
				PRO_dtmDatum = i_edcVersion.PRO_dtmBearbeitungsDatum,
				PRO_strBenutzername = i_edcVersion.PRO_strBearbeitungsBenutzer,
				PRO_blnIstFehlerhaft = i_edcVersion.PRO_blnIstFehlerhaft,
				PRO_strKommentar = i_edcVersion.PRO_strKommentar,
				PRO_enmStatus = i_edcVersion.PRO_enmVersionstatus,
				PRO_enmFreigabeStatus = i_edcVersion.PRO_enmFreigabestatus,
				PRO_enmFreigabeArt = i_enmLoetprogrammFreigabeArt
			};
		}

		private static bool FUN_blnHatProgrammFehlerfreieFreigabe(EDC_ProgrammInfo i_edcProgInfo)
		{
			if (!i_edcProgInfo.PRO_blnIstFehlerhaft)
			{
				return i_edcProgInfo.PROa_enmStatus.Contains(ENUM_LoetprogrammStatus.Freigegeben);
			}
			return false;
		}

		private static bool FUN_blnHatProgrammFehlerfreieArbeitsversion(EDC_ProgrammInfo i_edcProgInfo)
		{
			if (!i_edcProgInfo.PROa_enmStatus.Contains(ENUM_LoetprogrammStatus.Arbeitsversion))
			{
				return false;
			}
			if (i_edcProgInfo.PROa_enmStatus.Length != i_edcProgInfo.PROa_enmFehlerhaft.Length)
			{
				return false;
			}
			int num = i_edcProgInfo.PROa_enmStatus.ToList().IndexOf(ENUM_LoetprogrammStatus.Arbeitsversion);
			return !i_edcProgInfo.PROa_enmFehlerhaft[num];
		}
	}
}
