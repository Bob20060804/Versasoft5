using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Benutzer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Benutzer
{
	public class EDC_AktiveBenutzerDataAccess : EDC_DataAccess, INF_AktiveBenutzerDataAccess, INF_DataAccess
	{
		public EDC_AktiveBenutzerDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_AktiverBenutzerData>> FUN_fdcAktiveBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AktiverBenutzerData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_AktiverBenutzerData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcAktiverBenutzerHinzufuegenAsync(EDC_AktiverBenutzerData i_edcAktiverBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcAktiverBenutzer.PRO_i64MaschinenId = i_i64MaschinenId;
			if (!(await FUN_fdcIstAktiverBenutzerSchonVorhandenAsync(i_edcAktiverBenutzer, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)))
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcAktiverBenutzer, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task FUN_fdcAktiverBenutzerEntfernenAsync(long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AktiverBenutzerData.FUN_strBenutzerIdWhereStatementErstellen(i_i64BenutzerId, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_AktiverBenutzerData(i_strWhereStatement), i_fdcTransaktion);
		}

		private async Task<bool> FUN_fdcIstAktiverBenutzerSchonVorhandenAsync(EDC_AktiverBenutzerData i_edcAktiverBenutzer, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_AktiverBenutzerData.FUN_strSelectCountFuerAnwenderUndMaschineStatementErstellen(i_edcAktiverBenutzer.PRO_i64BenutzerId, i_edcAktiverBenutzer.PRO_i64MaschinenId);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return false;
			}
			return Convert.ToInt64(obj) > 0;
		}
	}
}
