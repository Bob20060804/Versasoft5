using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammValideDataAccess : EDC_DataAccess, INF_LoetprogrammValideDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammValideDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_LoetprogrammVersionValideData> FUN_fdcHoleLoetprogrammVersionValideDataObjektAsync(long i_i64VersionsId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionValideData.FUN_strVersionsIdUndMaschinenIdWhereStatementErstellen(i_i64VersionsId, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammVersionValideData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(long i_i64VersionsId, long i_i64MaschinenId, bool i_blnValide, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionValideData eDC_LoetprogrammVersionValideData = await FUN_fdcHoleLoetprogrammVersionValideDataObjektAsync(i_i64VersionsId, i_i64MaschinenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionValideData != null)
			{
				eDC_LoetprogrammVersionValideData.PRO_blnValide = i_blnValide;
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(eDC_LoetprogrammVersionValideData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				eDC_LoetprogrammVersionValideData = new EDC_LoetprogrammVersionValideData
				{
					PRO_i64VersionsId = i_i64VersionsId,
					PRO_i64MaschinenId = i_i64MaschinenId,
					PRO_blnValide = i_blnValide
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_LoetprogrammVersionValideData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
