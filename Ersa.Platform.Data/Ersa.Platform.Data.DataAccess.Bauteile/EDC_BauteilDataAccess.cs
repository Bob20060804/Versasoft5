using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Bauteile;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Bauteile;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Bauteile
{
	public class EDC_BauteilDataAccess : EDC_DataAccess, INF_BauteilDataAccess, INF_DataAccess
	{
		public EDC_BauteilDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcBauteilSpeichernAsync(EDC_BauteilData i_edcBauteilData, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_BauteilData edcBauteilDataAusDb = null;
			if (!string.IsNullOrEmpty(i_edcBauteilData.PRO_strBauteilId) && i_edcBauteilData.PRO_strBauteilId.Equals(i_edcBauteilData.FUN_strBauteilIdErstellen()))
			{
				edcBauteilDataAusDb = await FUN_fdcBauteilDatensatzLadenAsync(i_edcBauteilData.PRO_strBauteilId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (edcBauteilDataAusDb != null)
				{
					await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcBauteilData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			if (edcBauteilDataAusDb == null)
			{
				i_edcBauteilData.PRO_strBauteilId = i_edcBauteilData.FUN_strBauteilIdErstellen();
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcBauteilData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcBauteileSpeichernAsync(IEnumerable<EDC_BauteilData> i_enuBauteildata, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_enuBauteildata != null)
			{
				IDbTransaction dbTransaction = i_fdcTransaktion;
				if (dbTransaction == null)
				{
					dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				IDbTransaction fdcTransaktion = dbTransaction;
				try
				{
					foreach (EDC_BauteilData i_enuBauteildatum in i_enuBauteildata)
					{
						await FUN_fdcBauteilSpeichernAsync(i_enuBauteildatum, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (i_fdcTransaktion == null)
					{
						SUB_CommitTransaktion(fdcTransaktion);
					}
				}
				catch
				{
					if (i_fdcTransaktion == null)
					{
						SUB_RollbackTransaktion(fdcTransaktion);
					}
					throw;
				}
			}
		}

		public Task<EDC_BauteilData> FUN_fdcBauteilDatensatzLadenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilData.FUN_strBauteilIdWhereStatementErstellen(i_strBauteilId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BauteilData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerPackageNameLadenAsync(string i_strPackageName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilData.FUN_strPackageNameWhereStatementErstellen(i_strPackageName);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerPackageNamenLadenAsync(IEnumerable<string> i_strPackageNamen, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilData.FUN_strPackageNameWhereStatementErstellen(i_strPackageNamen);
			return (from i_edcItem in await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)
			group i_edcItem by i_edcItem.PRO_strBauteilId into g
			select g.First()).ToList();
		}

		public Task FUN_fdcBauteilLoeschenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilData.FUN_strBauteilIdWhereStatementErstellen(i_strBauteilId);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_BauteilData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerLikePackageNameLadenAsync(string i_strLikePackageName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilData.FUN_strLikePackageNameWhereStatementErstellen(i_strLikePackageName);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcBauteilMakrosSpeichernAsync(IEnumerable<EDC_BauteilMakroData> i_lstBauteilMacroDataListe, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_BauteilMakroData item in i_lstBauteilMacroDataListe)
				{
					await FUN_fdcBauteilMakroSpeichernAsync(item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcBauteilMakroSpeichernAsync(EDC_BauteilMakroData i_edcBauteilMacroData, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilMakroData.FUN_strBauteilWhereStatementErstellen(i_edcBauteilMacroData);
			if (await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BauteilMakroData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) == null)
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcBauteilMacroData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcBauteilMacroData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task FUN_fdcBauteilMakroLoeschenAsync(EDC_BauteilMakroData i_edcBauteilMacroData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcBauteilMacroData.PRO_strWhereStatement = EDC_BauteilMakroData.FUN_strBauteilWhereStatementErstellen(i_edcBauteilMacroData);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(i_edcBauteilMacroData, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdLadenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilMakroData.FUN_strBauteilIdWhereStatementErstellen(i_strBauteilId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilMakroData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdUndGeometrieIdLadenAsync(string i_strBauteilId, long i_i64GeometrieId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilMakroData.FUN_strBauteilIdUndGeometrieIdWhereStatementErstellen(i_strBauteilId, i_i64GeometrieId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilMakroData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdsLadenAsync(IEnumerable<string> i_strBauteilId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BauteilMakroData.FUN_strBauteilIdsWhereStatementErstellen(i_strBauteilId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BauteilMakroData(i_strWhereStatement), i_fdcTransaktion);
		}
	}
}
