using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammBibliothekDataAccess : EDC_DataAccess, INF_LoetprogrammBibliothekDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammBibliothekDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleDefaultBibliothekFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBibliothekData.FUN_strDefaultBibWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammBibliothekData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleBibliothekMitIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBibliothekData.FUN_strBibliotheksIdWhereStatementErstellen(i_i64BibliotheksId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammBibliothekData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleBibliothekMitNamenAsync(string i_strBibliotheksName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBibliothekData.FUN_strBibliotheksNameWhereStatementErstellen(i_strBibliotheksName, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammBibliothekData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_LoetprogrammBibliothekData>> FUN_fdcHoleAlleNichtGeloeschtenBibliothekenFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBibliothekData.FUN_strAlleNichtGeloeschtenBibliothkenWhereErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammBibliothekData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleBibliothekInDataTableAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBibliothekData.FUN_strBibliotheksIdWhereStatementErstellen(i_i64BibliotheksId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammBibliothekData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<EDC_LoetprogrammBibliothekData> FUN_fdcBibliothekErstellenAsync(string i_strBibliotheksName, long i_i64BenutzerId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = new EDC_LoetprogrammBibliothekData();
			EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData2 = eDC_LoetprogrammBibliothekData;
			long num2 = eDC_LoetprogrammBibliothekData2.PRO_i64BibliothekId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			eDC_LoetprogrammBibliothekData.PRO_i64GruppenId = i_i64GruppenId;
			eDC_LoetprogrammBibliothekData.PRO_strName = i_strBibliotheksName;
			eDC_LoetprogrammBibliothekData.PRO_blnGeloescht = false;
			eDC_LoetprogrammBibliothekData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_LoetprogrammBibliothekData.PRO_dtmAngelegtAm = DateTime.Now;
			eDC_LoetprogrammBibliothekData.PRO_i64BearbeitetVon = i_i64BenutzerId;
			eDC_LoetprogrammBibliothekData.PRO_dtmBearbeitetAm = DateTime.Now;
			EDC_LoetprogrammBibliothekData edcNeueBibliothek = eDC_LoetprogrammBibliothekData;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcNeueBibliothek, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return edcNeueBibliothek;
		}

		public Task FUN_fdcBibliothekGeloeschtSetzenAsync(long i_i64BibliotheksId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammBibliothekData.FUN_strLoeschenUpdateStatementErstellen(i_i64BibliotheksId, i_i64BenutzerId);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public Task FUN_fdcBibliothekUmbenennenAsync(long i_i64BibliotheksId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammBibliothekData.FUN_strUmbenennenUpdateAufIdStatementErstellen(i_i64BibliotheksId, i_strNeuerName);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public async Task<EDC_LoetprogrammBibliothekData> FUN_fdcBibliothekDuplizierenAsync(long i_i64BibliotheksId, string i_strNeuerName, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammBibliothekData edcBibliothek = await FUN_edcHoleBibliothekMitIdAsync(i_i64BibliotheksId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (edcBibliothek == null)
			{
				return null;
			}
			uint num = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammBibliothekData edcBibliothekClone = new EDC_LoetprogrammBibliothekData
			{
				PRO_i64BibliothekId = num,
				PRO_i64GruppenId = edcBibliothek.PRO_i64GruppenId,
				PRO_strName = i_strNeuerName,
				PRO_strBeschreibung = edcBibliothek.PRO_strBeschreibung,
				PRO_blnGeloescht = edcBibliothek.PRO_blnGeloescht,
				PRO_dtmAngelegtAm = DateTime.Now,
				PRO_i64AngelegtVon = i_i64BenutzerId,
				PRO_dtmBearbeitetAm = DateTime.Now,
				PRO_i64BearbeitetVon = i_i64BenutzerId
			};
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcBibliothekClone, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return edcBibliothekClone;
		}

		public Task<EDC_LoetprogrammBibliothekData> FUN_fdcImportiereBibliothekAsync(DataTable i_fdcTable, string i_strNeuerName, long i_i64BenutzerId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_fdcTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammBibliothekData>().ToList().Count != 1)
			{
				throw new InvalidDataException("the target library is not defined unambiguously");
			}
			return FUN_fdcBibliothekErstellenAsync(i_strNeuerName, i_i64BenutzerId, i_i64GruppenId, i_fdcTransaktion);
		}
	}
}
