using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Selektiv;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Duesentabelle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Duesentabelle
{
	public class EDC_DuesenbetriebDataAccess : EDC_DataAccess, INF_DuesenbetriebDataAccess, INF_DataAccess
	{
		public EDC_DuesenbetriebDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_DuesenbetriebWerteData>> FUN_fdcHoleAlleAktuellenDuesenBetriebswerteFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_DuesenbetriebWerteData.FUN_strHoleAktiveDusenFuerMaschineWhereStatement(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenbetriebWerteData(i_strWhereStatement));
		}

		public Task<EDC_DuesenbetriebWerteData> FUN_fdcHoleAktuelleDuesenBetriebswerteFuerDueseAsync(long i_i64MaschinenId, long i_i64GeomertieId, ENUM_SelektivTiegel i_enmTiegel)
		{
			EDC_DuesenbetriebWerteData i_edcSelectObjekt = new EDC_DuesenbetriebWerteData(EDC_DuesenbetriebWerteData.FUN_strHoleAktiveDusenWhereStatement(i_i64MaschinenId, i_i64GeomertieId, i_enmTiegel));
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(i_edcSelectObjekt);
		}

		public async Task<EDC_DuesenbetriebWerteData> FUN_fdcErstelleNeuenDuesenBetriebswertAsync(long i_i64MaschinenId, long i_i64GeomertieId, ENUM_SelektivTiegel i_enmTiegel)
		{
			EDC_DuesenbetriebWerteData edcDuesenwert = new EDC_DuesenbetriebWerteData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_i64GeometrieId = i_i64GeomertieId,
				PRO_enmTiegel = i_enmTiegel,
				PRO_i64AnzahlAktivierungen = 0L,
				PRO_i64GesamtZeit = 0L,
				PRO_i64WelleAusZeit = 0L,
				PRO_i64WelleEinZeit = 0L,
				PRO_dtmDuesenStartDatum = DateTime.Now,
				PRO_strDuesenGuid = string.Empty
			};
			await FUN_fdcDuesenBetriebswertSpeichernAsync(edcDuesenwert).ConfigureAwait(continueOnCapturedContext: false);
			return edcDuesenwert;
		}

		public async Task FUN_fdcDuesenBetriebswertSpeichernAsync(EDC_DuesenbetriebWerteData i_edcDuesenWerte)
		{
			if (string.IsNullOrEmpty(i_edcDuesenWerte.PRO_strDuesenGuid))
			{
				i_edcDuesenWerte.PRO_strDuesenGuid = Guid.NewGuid().ToString();
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcDuesenWerte).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcDuesenWerte).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task FUN_fdcTrackeDuesenWechselAsync(EDC_DuesenbetriebWerteData i_edcDuesenDataWert, long i_i64BenutzerId)
		{
			EDC_DuesenbetriebWechselData i_edcObjekt = new EDC_DuesenbetriebWechselData
			{
				PRO_i64MaschinenId = i_edcDuesenDataWert.PRO_i64MaschinenId,
				PRO_enmTiegel = i_edcDuesenDataWert.PRO_enmTiegel,
				PRO_strDuesenGuid = i_edcDuesenDataWert.PRO_strDuesenGuid,
				PRO_i64BenutzerId = i_i64BenutzerId,
				PRO_dtmWechselDatum = DateTime.Now
			};
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt);
		}

		public Task<IEnumerable<EDC_DuesenbetriebWechselData>> FUN_fdcHoleDuesenWechselTrackDatenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_DuesenbetriebWechselData.FUN_strHoleWechselFuerMaschineWhereStatement(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenbetriebWechselData(i_strWhereStatement));
		}
	}
}
