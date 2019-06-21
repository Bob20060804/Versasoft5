using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Cad;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Cad
{
	public class EDC_CadBildDataAccess : EDC_DataAccess, INF_CadBildDataAccess, INF_DataAccess
	{
		public EDC_CadBildDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcBildDatensatzSpeichernAsync(long i_i64ProgrammId, EDC_CadBildData i_edcBilddata, IDbTransaction i_fdcTransaktion = null)
		{
			await FUN_fdcBildVerwendungLoeschenAsync(i_i64ProgrammId, i_edcBilddata.PRO_enmCadBildVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			i_edcBilddata.PRO_i64ProgrammId = i_i64ProgrammId;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcBilderSpeichernAsync(long i_i64ProgrammId, IEnumerable<EDC_CadBildData> i_enuBilddata, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_enuBilddata != null)
			{
				foreach (EDC_CadBildData i_enuBilddatum in i_enuBilddata)
				{
					await FUN_fdcBildDatensatzSpeichernAsync(i_i64ProgrammId, i_enuBilddatum, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		public Task<EDC_CadBildData> FUN_fdcBildDatensatzLadenAsync(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadBildData.FUN_strProgrammIdUndVerwendungWhereStatementErstellen(i_i64ProgrammId, i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_CadBildData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadBildData>> FUN_fdcAlleBilddatenLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string strWhereStatement = EDC_CadBildData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return Task.Run(() => m_edcDatenbankAdapter.FUN_lstErstelleObjektliste(new EDC_CadBildData(strWhereStatement), i_fdcTransaktion));
		}

		public Task FUN_fdcBildVerwendungLoeschenAsync(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadBildData.FUN_strLoescheBildStatementErstellen(i_i64ProgrammId, i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task FUN_fdcBilderLoeschenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadBildData.FUN_strLoescheBilderStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleBilddatenDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadBildData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadBildData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcImportiereCadBildDatenAusLoetprogrammAsync(DataSet i_fdcDataSet, long i_i64NeueProgramId, IDbTransaction i_fdcTransaktion)
		{
			DataTable dataTable = i_fdcDataSet.Tables["CadImages"];
			if (dataTable != null)
			{
				List<EDC_CadBildData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadBildData>().ToList();
				foreach (EDC_CadBildData item in list)
				{
					item.PRO_i64ProgrammId = i_i64NeueProgramId;
				}
				return m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion);
			}
			return Task.FromResult(0);
		}
	}
}
