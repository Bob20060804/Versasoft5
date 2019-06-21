using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammBildDataAccess : EDC_DataAccess, INF_LoetprogrammBildDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammBildDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<Bitmap> FUN_fdcBildLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			byte[] array = await FUN_fdcBildArrayLadenAsync(i_i64LoetprogrammId, i_enmVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return (array != null) ? EDC_BildConverterHelfer.FUN_fdcByteArrayToImage(array) : null;
		}

		public async Task<byte[]> FUN_fdcBildArrayLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			return (await FUN_fdcBildDatensatzLadenAsync(i_i64LoetprogrammId, i_enmVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false))?.PRO_bytBild;
		}

		public Task<DataTable> FUN_fdcHoleBildDataInDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBildData.FUN_strProgrammIdMitAllenVerwendungenWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammBildData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcBildAktualisierenAsync(EDC_LoetprogrammBildData i_edcBilddata, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammBildData eDC_LoetprogrammBildData = await FUN_fdcBildDatensatzLadenAsync(i_edcBilddata.PRO_i64ProgrammId, i_edcBilddata.PRO_enmVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammBildData != null)
			{
				eDC_LoetprogrammBildData.PRO_strWhereStatement = EDC_LoetprogrammBildData.FUN_strProgrammIdUndVerwendungWhereStatementErstellen(i_edcBilddata.PRO_i64ProgrammId, i_edcBilddata.PRO_i32Verwendung);
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcBilddatenImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramImages"];
			if (dataTable != null)
			{
				List<EDC_LoetprogrammBildData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammBildData>().ToList();
				if (list.Count != 0)
				{
					foreach (EDC_LoetprogrammBildData item in list)
					{
						item.PRO_i64ProgrammId = i_i64ProgrammId;
						await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(item, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}
		}

		public Task<EDC_LoetprogrammBildData> FUN_fdcBildDatensatzLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBildData.FUN_strProgrammIdUndVerwendungWhereStatementErstellen(i_i64LoetprogrammId, (int)i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammBildData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task FUN_fdcBildVerwendungEntfernenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammBildData.FUN_strProgrammIdUndVerwendungWhereStatementErstellen(i_i64LoetprogrammId, (int)i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_LoetprogrammBildData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcLpBilderEntfernenAsync(long i_i64LoetprogrammId, IEnumerable<ENUM_BildVerwendung> i_enuVerwendungen, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(FUN_edcHoleAbfrageObjektAufAlleLpBilder(i_i64LoetprogrammId, i_enuVerwendungen.Cast<int>()), i_fdcTransaktion);
		}

		private EDC_LoetprogrammBildData FUN_edcHoleAbfrageObjektAufAlleLpBilder(long i_i64LoetprogrammId, IEnumerable<int> i_enuVerwendungen)
		{
			return new EDC_LoetprogrammBildData(EDC_LoetprogrammBildData.FUN_strProgrammIdMitVerwendungenWhereStatementErstellen(i_i64LoetprogrammId, i_enuVerwendungen));
		}
	}
}
