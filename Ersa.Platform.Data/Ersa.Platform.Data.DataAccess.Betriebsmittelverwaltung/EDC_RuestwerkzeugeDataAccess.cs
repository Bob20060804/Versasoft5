using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Betriebsmittelverwaltung
{
	public class EDC_RuestwerkzeugeDataAccess : EDC_DataAccess, INF_RuestwerkzeugeDataAccess, INF_DataAccess
	{
		public EDC_RuestwerkzeugeDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcLeseRuestwerkzeugeFuerKomponentenIdAusDatenbankAsync(long i_i32RuestkomponentenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestwerkzeugeData.FUN_strRuestkomponentenIdWhereStatementErstellen(i_i32RuestkomponentenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_RuestwerkzeugeData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_RuestwerkzeugeData> FUN_fdcLeseRuestwerkzeugFuerIdAusDatenbankAsync(long i_i64RuestwerkzeugId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestwerkzeugeData.FUN_strRuestwerkzeugIdWhereStatementErstellen(i_i64RuestwerkzeugId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_RuestwerkzeugeData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task<EDC_RuestwerkzeugeData> FUN_fdcRuestwerkzeugDatenSatzHinzufuegenAsync(EDC_RuestwerkzeugeData i_edcRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcRuestwerkzeugeData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i_edcRuestwerkzeugeData;
		}

		public Task FUN_fdcRuestwerkzeugDatenSatzAendernAsync(EDC_RuestwerkzeugeData i_edcRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcRuestwerkzeugeData.PRO_strWhereStatement = EDC_RuestwerkzeugeData.FUN_strRuestwerkzeugIdWhereStatementErstellen(i_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcRuestwerkzeugeData, i_fdcTransaktion);
		}

		public Task FUN_fdcRuestwerkzeugDatenSatzLoeschenAsync(long i_i64RuestwerkzeugId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_RuestwerkzeugeData.FUN_strLoeschenUpdateStatementErstellen(i_i64RuestwerkzeugId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task FUN_fdcRuestwerkzeugDatenLoeschenFuerKomponentenIdAsync(long i_i64RuestkomponentenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_RuestwerkzeugeData.FUN_strLoeschenUpdateStatementFuerKomponentenIdErstellen(i_i64RuestkomponentenId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}
	}
}
