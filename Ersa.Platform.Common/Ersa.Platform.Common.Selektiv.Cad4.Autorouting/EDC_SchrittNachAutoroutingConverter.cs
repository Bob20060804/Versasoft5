using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Selektiv.Cad4.Autorouting
{
	public static class EDC_SchrittNachAutoroutingConverter
	{
		public static IEnumerable<Tuple<EDC_CadRoutenData, IEnumerable<EDC_CadRoutenSchrittData>>> FUN_enuKonvertieren(IEnumerable<EDC_SchrittNachAutorouting> i_enuSchritte, long i_i64VersionsId)
		{
			return i_enuSchritte.Select((EDC_SchrittNachAutorouting i_edcAutorouting, int i_i32Id) => FUN_edcKonvertiere(i_edcAutorouting, i_i32Id, i_i64VersionsId));
		}

		public static IEnumerable<EDC_SchrittNachAutorouting> FUN_enuKonvertieren(IEnumerable<EDC_CadRoutenData> i_enuCadRoutenData, IEnumerable<EDC_CadRoutenSchrittData> i_enuRoutenSchrittDaten)
		{
			Dictionary<long, List<EDC_AbstractCncSchritt>> dicRoutenSchrittDaten = EDC_CncSchrittConverter.FUN_dicConvert(i_enuRoutenSchrittDaten);
			return from i_edcData in i_enuCadRoutenData
			orderby i_edcData.PRO_i64RoutingId
			select FUN_edcKonvertiere(i_edcData, dicRoutenSchrittDaten.ContainsKey(i_edcData.PRO_i64RoutingId) ? dicRoutenSchrittDaten[i_edcData.PRO_i64RoutingId] : new List<EDC_AbstractCncSchritt>());
		}

		private static Tuple<EDC_CadRoutenData, IEnumerable<EDC_CadRoutenSchrittData>> FUN_edcKonvertiere(EDC_SchrittNachAutorouting i_edcSchritt, long i_i64SchrittId, long i_i64VersionsId)
		{
			IEnumerable<EDC_CadRoutenSchrittData> item = EDC_CncSchrittConverter.FUN_enuConvert<EDC_CadRoutenSchrittData>(i_edcSchritt.PRO_lstAblaufSchritte, i_i64VersionsId, i_i64SchrittId);
			return new Tuple<EDC_CadRoutenData, IEnumerable<EDC_CadRoutenSchrittData>>(new EDC_CadRoutenData
			{
				PRO_i32SynchronisationsId = i_edcSchritt.PRO_i32SynchronisationsId,
				PRO_i32WerkzeugNummer = i_edcSchritt.PRO_i32WerkzeugNummer,
				PRO_i32ModulNummer = i_edcSchritt.PRO_i32ModulNummer,
				PRO_sngStartpunktX = i_edcSchritt.PRO_sngStartpunktX,
				PRO_sngStartpunktY = i_edcSchritt.PRO_sngStartpunktY,
				PRO_sngEndpunktX = i_edcSchritt.PRO_sngEndpunktX,
				PRO_sngEndpunktY = i_edcSchritt.PRO_sngEndpunktY,
				PRO_enmBahnTyp = i_edcSchritt.PRO_enmBahnTyp,
				PRO_enmSchrittModus = i_edcSchritt.PRO_enmSchrittModus,
				PRO_i64VersionsId = i_i64VersionsId,
				PRO_i32SynchronisationsModus = (int)i_edcSchritt.PRO_enmSynchronisationsModus,
				PRO_enmMaschinenModul = i_edcSchritt.PRO_enmModulTyp,
				PRO_i64RoutingId = i_i64SchrittId,
				PRO_enmSynchronisationsPosition = i_edcSchritt.PRO_enmSynchronisationsPosition
			}, item);
		}

		private static EDC_SchrittNachAutorouting FUN_edcKonvertiere(EDC_CadRoutenData i_edcRoutenData, IList<EDC_AbstractCncSchritt> i_lstSchritte)
		{
			return new EDC_SchrittNachAutorouting
			{
				PRO_i32ModulNummer = i_edcRoutenData.PRO_i32ModulNummer,
				PRO_enmBahnTyp = i_edcRoutenData.PRO_enmBahnTyp,
				PRO_sngStartpunktX = i_edcRoutenData.PRO_sngStartpunktX,
				PRO_sngStartpunktY = i_edcRoutenData.PRO_sngStartpunktY,
				PRO_sngEndpunktX = i_edcRoutenData.PRO_sngEndpunktX,
				PRO_sngEndpunktY = i_edcRoutenData.PRO_sngEndpunktY,
				PRO_enmModulTyp = i_edcRoutenData.PRO_enmMaschinenModul,
				PRO_enmSchrittModus = i_edcRoutenData.PRO_enmSchrittModus,
				PRO_enmSynchronisationsModus = (ENUM_SyncpunktAttribut)Enum.Parse(typeof(ENUM_SyncpunktAttribut), i_edcRoutenData.PRO_i32SynchronisationsModus.ToString()),
				PRO_i32SynchronisationsId = i_edcRoutenData.PRO_i32SynchronisationsId,
				PRO_i32WerkzeugNummer = i_edcRoutenData.PRO_i32WerkzeugNummer,
				PRO_lstAblaufSchritte = i_lstSchritte,
				PRO_enmSynchronisationsPosition = i_edcRoutenData.PRO_enmSynchronisationsPosition
			};
		}
	}
}
