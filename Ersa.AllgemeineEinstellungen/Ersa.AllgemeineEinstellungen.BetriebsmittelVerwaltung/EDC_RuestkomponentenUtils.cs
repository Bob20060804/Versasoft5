using Ersa.AllgemeineEinstellungen.ViewModels;
using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ersa.AllgemeineEinstellungen.BetriebsmittelVerwaltung
{
	public static class EDC_RuestkomponentenUtils
	{
		public static IEnumerable<EDC_NiederhaltergruppeViewModel> FUN_enuModellKonvertieren(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponenten)
		{
			List<EDC_NiederhaltergruppeViewModel> list = new List<EDC_NiederhaltergruppeViewModel>();
			foreach (EDC_RuestkomponentenData item2 in i_enuRuestkomponenten)
			{
				EDC_NiederhaltergruppeViewModel item = new EDC_NiederhaltergruppeViewModel(item2.PRO_strName)
				{
					PRO_i64RuestkomponentenId = item2.PRO_i64RuestkomponentenId,
					PRO_enmTyp = item2.PRO_enmTyp,
					PRO_i64MaschinenGruppenId = item2.PRO_i64MachinenGruppenId,
					PRO_strName = item2.PRO_strName
				};
				list.Add(item);
			}
			return list;
		}

		public static IEnumerable<EDC_NiederhaltergruppeViewModel> FUN_enuModellKonvertieren(IEnumerable<EDC_Ruestkomponente> i_enuRuestkomponenten)
		{
			List<EDC_NiederhaltergruppeViewModel> list = new List<EDC_NiederhaltergruppeViewModel>();
			foreach (EDC_Ruestkomponente item in i_enuRuestkomponenten)
			{
				EDC_NiederhaltergruppeViewModel eDC_NiederhaltergruppeViewModel = new EDC_NiederhaltergruppeViewModel(item.PRO_edcRuestkomponenteData.PRO_strName)
				{
					PRO_i64RuestkomponentenId = item.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId,
					PRO_enmTyp = item.PRO_edcRuestkomponenteData.PRO_enmTyp,
					PRO_i64MaschinenGruppenId = item.PRO_edcRuestkomponenteData.PRO_i64MachinenGruppenId,
					PRO_strName = item.PRO_edcRuestkomponenteData.PRO_strName
				};
				List<EDC_NiederhalterViewModel> list2 = new List<EDC_NiederhalterViewModel>();
				foreach (EDC_Ruestwerkzeug item2 in item.PRO_enuRuestwerkzeuge)
				{
					list2.Add(FUN_edcEintragKonvertieren(item2));
				}
				eDC_NiederhaltergruppeViewModel.PRO_lstEintraege.AddRange(list2);
				list.Add(eDC_NiederhaltergruppeViewModel);
			}
			return list;
		}

		public static EDC_Ruestwerkzeug FUN_edcModellKonvertieren(EDC_NiederhalterViewModel i_edcUiEintrag)
		{
			EDC_RuestwerkzeugeData pRO_edcRuestwerkzeugeData = new EDC_RuestwerkzeugeData
			{
				PRO_i64RuestwerkzeugId = i_edcUiEintrag.PRO_i64RuestwerkzeugId,
				PRO_i64RuestkomponentenId = i_edcUiEintrag.PRO_i64RuestkomponentenId,
				PRO_strIdentifikation = i_edcUiEintrag.PRO_strIdentifikation
			};
			return new EDC_Ruestwerkzeug
			{
				PRO_edcRuestwerkzeugeData = pRO_edcRuestwerkzeugeData,
				PRO_blnGeaendert = i_edcUiEintrag.PRO_blnHatAenderung,
				PRO_blnGeloescht = i_edcUiEintrag.PRO_blnGeloescht
			};
		}

		public static EDC_Ruestkomponente FUN_edcModellKonvertieren(EDC_NiederhaltergruppeViewModel i_edcUiRuestkomponenten)
		{
			EDC_RuestkomponentenData pRO_edcRuestkomponenteData = new EDC_RuestkomponentenData
			{
				PRO_i64RuestkomponentenId = i_edcUiRuestkomponenten.PRO_i64RuestkomponentenId,
				PRO_enmTyp = i_edcUiRuestkomponenten.PRO_enmTyp,
				PRO_i64MachinenGruppenId = i_edcUiRuestkomponenten.PRO_i64MaschinenGruppenId,
				PRO_strName = i_edcUiRuestkomponenten.PRO_strName
			};
			return new EDC_Ruestkomponente
			{
				PRO_edcRuestkomponenteData = pRO_edcRuestkomponenteData,
				PRO_blnGeloescht = i_edcUiRuestkomponenten.PRO_blnGeloescht,
				PRO_blnGeaendert = i_edcUiRuestkomponenten.PRO_blnHatAenderung
			};
		}

		public static EDC_NiederhalterViewModel FUN_edcEintragKonvertieren(EDC_RuestwerkzeugeData i_edcRuestwerkzeug)
		{
			return new EDC_NiederhalterViewModel
			{
				PRO_i64RuestwerkzeugId = i_edcRuestwerkzeug.PRO_i64RuestwerkzeugId,
				PRO_i64RuestkomponentenId = i_edcRuestwerkzeug.PRO_i64RuestkomponentenId,
				PRO_strIdentifikation = i_edcRuestwerkzeug.PRO_strIdentifikation
			};
		}

		public static EDC_NiederhalterViewModel FUN_edcEintragKonvertieren(EDC_Ruestwerkzeug i_edcRuestwerkzeug)
		{
			return new EDC_NiederhalterViewModel
			{
				PRO_i64RuestwerkzeugId = i_edcRuestwerkzeug.PRO_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId,
				PRO_i64RuestkomponentenId = i_edcRuestwerkzeug.PRO_edcRuestwerkzeugeData.PRO_i64RuestkomponentenId,
				PRO_strIdentifikation = i_edcRuestwerkzeug.PRO_edcRuestwerkzeugeData.PRO_strIdentifikation
			};
		}
	}
}
