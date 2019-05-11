using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Fluxer;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Loeten;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Uebergeordnet;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Selektiv.Cad4.Autorouting
{
	public static class EDC_CncSchrittConverter
	{
		public static EDC_AbstractCncSchritt FUN_edcConvert(EDC_CadCncSchrittData i_edcSchrittDaten)
		{
			switch (i_edcSchrittDaten.PRO_enmBahnTyp)
			{
			case ENUM_CncSchrittTyp.HorizontalBewegung:
				return new EDC_HorizontalBewegungSchritt
				{
					PRO_dblGeschwindigkeit = i_edcSchrittDaten.PRO_sngParameter1
				};
			case ENUM_CncSchrittTyp.VertikalBewegung:
				return new EDC_VertikalBewegungSchritt
				{
					PRO_dblZielwertZ = i_edcSchrittDaten.PRO_sngParameter1,
					PRO_dblGeschwindigkeit = i_edcSchrittDaten.PRO_sngParameter2
				};
			case ENUM_CncSchrittTyp.Warten:
				return new EDC_WartenSchritt
				{
					PRO_dblZeit = i_edcSchrittDaten.PRO_sngParameter1
				};
			case ENUM_CncSchrittTyp.LoetwellenHoehe:
				return new EDC_LoetwellenhoeheSchritt
				{
					PRO_dblNeueHoehe = i_edcSchrittDaten.PRO_sngParameter1,
					PRO_dblZeit = i_edcSchrittDaten.PRO_sngParameter2
				};
			case ENUM_CncSchrittTyp.FluxerDosis:
				return new EDC_FluxerDosisSchritt
				{
					PRO_dblNeueDosis = i_edcSchrittDaten.PRO_sngParameter1
				};
			case ENUM_CncSchrittTyp.Weiterfahren:
				return new EDC_WeiterfahrenSchritt
				{
					PRO_dblStrecke = i_edcSchrittDaten.PRO_sngParameter1
				};
			default:
				return null;
			}
		}

		public static Dictionary<long, List<EDC_AbstractCncSchritt>> FUN_dicConvert<T>(IEnumerable<T> i_enuSchrittDaten) where T : EDC_CadCncSchrittData, new()
		{
			Dictionary<long, List<EDC_AbstractCncSchritt>> dictionary = new Dictionary<long, List<EDC_AbstractCncSchritt>>();
			foreach (T item in i_enuSchrittDaten)
			{
				if (!dictionary.TryGetValue(item.PRO_i64SchrittId, out List<EDC_AbstractCncSchritt> value))
				{
					value = new List<EDC_AbstractCncSchritt>();
					dictionary.Add(item.PRO_i64SchrittId, value);
				}
				value.Add(FUN_edcConvert(item));
			}
			return dictionary;
		}

		public static IEnumerable<T> FUN_enuConvert<T>(IEnumerable<EDC_AbstractCncSchritt> i_enuSchritte, long i_i64VersionsId, long i_i64SchrittId) where T : EDC_CadCncSchrittData, new()
		{
			return new List<EDC_AbstractCncSchritt>(i_enuSchritte).Select((EDC_AbstractCncSchritt i_edcSchritt, int i_i32I) => FUN_edcConvert<T>(i_edcSchritt, i_i32I, i_i64VersionsId, i_i64SchrittId));
		}

		public static T FUN_edcConvert<T>(EDC_AbstractCncSchritt i_edcSchritt, int i_i32Rang, long i_i64VersionsId, long i_i64SchrittId) where T : EDC_CadCncSchrittData, new()
		{
			T val = new T();
			val.PRO_i32CncRang = i_i32Rang;
			val.PRO_i64VersionsId = i_i64VersionsId;
			val.PRO_i64SchrittId = i_i64SchrittId;
			T val2 = val;
			if (i_edcSchritt is EDC_HorizontalBewegungSchritt)
			{
				EDC_HorizontalBewegungSchritt eDC_HorizontalBewegungSchritt = (EDC_HorizontalBewegungSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.HorizontalBewegung;
				val2.PRO_sngParameter1 = (float)eDC_HorizontalBewegungSchritt.PRO_dblGeschwindigkeit;
				return val2;
			}
			if (i_edcSchritt is EDC_VertikalBewegungSchritt)
			{
				EDC_VertikalBewegungSchritt eDC_VertikalBewegungSchritt = (EDC_VertikalBewegungSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.VertikalBewegung;
				val2.PRO_sngParameter1 = (float)eDC_VertikalBewegungSchritt.PRO_dblZielwertZ;
				val2.PRO_sngParameter2 = (float)eDC_VertikalBewegungSchritt.PRO_dblGeschwindigkeit;
				return val2;
			}
			if (i_edcSchritt is EDC_LoetwellenhoeheSchritt)
			{
				EDC_LoetwellenhoeheSchritt eDC_LoetwellenhoeheSchritt = (EDC_LoetwellenhoeheSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.LoetwellenHoehe;
				val2.PRO_sngParameter1 = (float)eDC_LoetwellenhoeheSchritt.PRO_dblNeueHoehe;
				val2.PRO_sngParameter2 = (float)eDC_LoetwellenhoeheSchritt.PRO_dblZeit;
				return val2;
			}
			if (i_edcSchritt is EDC_WartenSchritt)
			{
				EDC_WartenSchritt eDC_WartenSchritt = (EDC_WartenSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.Warten;
				val2.PRO_sngParameter1 = (float)eDC_WartenSchritt.PRO_dblZeit;
				return val2;
			}
			if (i_edcSchritt is EDC_FluxerDosisSchritt)
			{
				EDC_FluxerDosisSchritt eDC_FluxerDosisSchritt = (EDC_FluxerDosisSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.FluxerDosis;
				val2.PRO_sngParameter1 = (float)eDC_FluxerDosisSchritt.PRO_dblNeueDosis;
				return val2;
			}
			if (i_edcSchritt is EDC_WeiterfahrenSchritt)
			{
				EDC_WeiterfahrenSchritt eDC_WeiterfahrenSchritt = (EDC_WeiterfahrenSchritt)i_edcSchritt;
				val2.PRO_enmBahnTyp = ENUM_CncSchrittTyp.Weiterfahren;
				val2.PRO_sngParameter1 = (float)eDC_WeiterfahrenSchritt.PRO_dblStrecke;
				return val2;
			}
			return null;
		}
	}
}
