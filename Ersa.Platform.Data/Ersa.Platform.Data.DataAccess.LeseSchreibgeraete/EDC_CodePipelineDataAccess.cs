using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.LeseSchreibgeraete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.LeseSchreibgeraete
{
	public class EDC_CodePipelineDataAccess : EDC_DataAccess, INF_CodePipelineDataAccess, INF_DataAccess
	{
		public EDC_CodePipelineDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcCodeEinstellungenSetzenAsync(long i_i64MaschinenId, long i_i64ArrayIndex, bool i_blnAktiv, bool i_blnAlbAusElb, bool i_blnAlb, bool i_blnElb, long i_i64Timeout, long i_i64BenutzerId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_CodeKonfigData edcKonfiguration = await FUN_fdcHoleCodeKonfigurationAsync(i_i64MaschinenId, i_i64ArrayIndex, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (edcKonfiguration != null)
				{
					edcKonfiguration.PRO_blnWirdVerwendet = i_blnAktiv;
					edcKonfiguration.PRO_blnAlbAusElb = i_blnAlbAusElb;
					edcKonfiguration.PRO_blnAlbVerwenden = i_blnAlb;
					edcKonfiguration.PRO_blnElbVerwenden = i_blnElb;
					edcKonfiguration.PRO_i64Timeout = i_i64Timeout;
					await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcKonfiguration, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await FUN_fdcCodeKonfigurationTrackenAsync(edcKonfiguration, i_i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<bool> FUN_fdcIstCodeKonfigurationAktivAsync(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return (await FUN_fdcHoleCodeKonfigurationAsync(i_i64MaschinenId, i_i64ArrayIndex).ConfigureAwait(continueOnCapturedContext: false))?.PRO_blnWirdVerwendet ?? false;
		}

		public Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_CodeKonfigData.FUN_strMaschinenIdUndAktivWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeKonfigData(i_strWhereStatement));
		}

		public Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung)
		{
			string i_strWhereStatement = EDC_CodeKonfigData.FUN_strNurAktiveMaschinenIdUndVerwendungWhereStatementErstellen(i_i64MaschinenId, i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeKonfigData(i_strWhereStatement));
		}

		public Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenMitVerwendungAsync(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung)
		{
			string i_strWhereStatement = EDC_CodeKonfigData.FUN_strMaschinenIdUndVerwendungWhereStatementErstellen(i_i64MaschinenId, i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeKonfigData(i_strWhereStatement));
		}

		public Task<EDC_CodeKonfigData> FUN_fdcHoleCodeKonfigurationAsync(long i_i64MaschinenId, long i_i64ArrayIndex, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CodeKonfigData.FUN_strMaschinenIdUndArrayIndexWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_CodeKonfigData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_CodeKonfigData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeKonfigData(i_strWhereStatement));
		}

		public Task FUN_fdcCodeKonfigurationSpeichernAsync(EDC_CodeKonfigData i_edcCodeKonfiguration, long i_i64BenutzerId)
		{
			List<EDC_CodeKonfigData> i_lstCodeKonfigurationen = new List<EDC_CodeKonfigData>
			{
				i_edcCodeKonfiguration
			};
			return FUN_fdcCodeKonfigurationenSpeichernAsync(i_lstCodeKonfigurationen, i_i64BenutzerId);
		}

		public async Task FUN_fdcCodeKonfigurationenSpeichernAsync(IEnumerable<EDC_CodeKonfigData> i_lstCodeKonfigurationen, long i_i64BenutzerId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (EDC_CodeKonfigData edcKonfiguration in i_lstCodeKonfigurationen)
				{
					await FUN_fdcCodeKonfigurationTrackenAsync(edcKonfiguration, i_i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					EDC_CodeKonfigData eDC_CodeKonfigData = await FUN_fdcHoleCodeKonfigurationAsync(edcKonfiguration.PRO_i64MaschinenId, edcKonfiguration.PRO_i64ArrayIndex, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (eDC_CodeKonfigData != null)
					{
						if (!eDC_CodeKonfigData.PRO_enmCodeVerwendung.Equals(edcKonfiguration.PRO_enmCodeVerwendung))
						{
							await FUN_fdcLoeschePipelineArrayAsync(edcKonfiguration.PRO_i64MaschinenId, edcKonfiguration.PRO_i64ArrayIndex, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
						}
						await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcKonfiguration, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcKonfiguration, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcLoeschePipelineZweigAsync(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig, IDbTransaction i_fdcTransaktion = null)
		{
			string strWhereStatement = EDC_CodePipelineData.FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex, i_i64Zweig);
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodePipelineData(strWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcLoeschePipelineArrayAsync(long i_i64MaschinenId, long i_i64ArrayIndex, IDbTransaction i_fdcTransaktion = null)
		{
			string strWhereStatement = EDC_CodePipelineData.FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex);
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodePipelineData(strWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<EDC_CodePipelineData> FUN_fdcHoleCodePipelineElementAsync(long i_i64MaschinenId, long i_i64ArrayIndex, ENUM_PipelineElement i_enuElement, long i_i64Zweig)
		{
			string i_strWhereStatement = EDC_CodePipelineData.FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex, (long)i_enuElement, i_i64Zweig);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_CodePipelineData(i_strWhereStatement));
		}

		public Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleCodePipelineElementeAsync(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			string i_strWhereStatement = EDC_CodePipelineData.FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodePipelineData(i_strWhereStatement));
		}

		public async Task FUN_fdcCodePipelineElementeSpeichernAsync(IEnumerable<EDC_CodePipelineData> i_lstPipelineElemente, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction fdcTransaktion = null;
			try
			{
				List<EDC_CodePipelineData> lstElemente = i_lstPipelineElemente.ToList();
				IEnumerable<EDC_CodePipelineData> enmDistinct = from edcElement in lstElemente
				group edcElement by new
				{
					edcElement.PRO_i64ArrayIndex,
					edcElement.PRO_i64Zweig
				} into edcDistinctDesignationGroup
				select edcDistinctDesignationGroup.First();
				IDbTransaction dbTransaction = i_fdcTransaktion;
				if (dbTransaction == null)
				{
					dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				fdcTransaktion = dbTransaction;
				foreach (EDC_CodePipelineData item in enmDistinct)
				{
					string i_strWhereStatement = EDC_CodePipelineData.FUN_strWhereStatementErstellen(item.PRO_i64MaschinenId, item.PRO_i64ArrayIndex, item.PRO_i64Zweig);
					await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodePipelineData(i_strWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				foreach (EDC_CodePipelineData edcElement2 in lstElemente)
				{
					await FUN_fdcPipelineElementeTrackenAsync(edcElement2, i_i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcElement2, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		private async Task FUN_fdcPipelineElementeTrackenAsync(EDC_CodePipelineData i_edcElementgData, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion)
		{
			EDC_CodePipelineTrackData eDC_CodePipelineTrackData = new EDC_CodePipelineTrackData();
			EDC_CodePipelineTrackData eDC_CodePipelineTrackData2 = eDC_CodePipelineTrackData;
			long num2 = eDC_CodePipelineTrackData2.PRO_i64TrackId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_seprotokoll").ConfigureAwait(continueOnCapturedContext: false);
			eDC_CodePipelineTrackData.PRO_i64MaschinenId = i_edcElementgData.PRO_i64MaschinenId;
			eDC_CodePipelineTrackData.PRO_i64ArrayIndex = i_edcElementgData.PRO_i64ArrayIndex;
			eDC_CodePipelineTrackData.PRO_i64PipelineElement = i_edcElementgData.PRO_i64PipelineElement;
			eDC_CodePipelineTrackData.PRO_i64Zweig = i_edcElementgData.PRO_i64Zweig;
			eDC_CodePipelineTrackData.PRO_strInhalt = i_edcElementgData.PRO_strInhalt;
			eDC_CodePipelineTrackData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_CodePipelineTrackData.PRO_dtmAngelegtAm = DateTime.Now;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_CodePipelineTrackData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcCodeKonfigurationTrackenAsync(EDC_CodeKonfigData i_edcKonfigData, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion)
		{
			EDC_CodeKonfigTrackData eDC_CodeKonfigTrackData = new EDC_CodeKonfigTrackData();
			EDC_CodeKonfigTrackData eDC_CodeKonfigTrackData2 = eDC_CodeKonfigTrackData;
			long num2 = eDC_CodeKonfigTrackData2.PRO_i64TrackId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_seprotokoll").ConfigureAwait(continueOnCapturedContext: false);
			eDC_CodeKonfigTrackData.PRO_i64MaschinenId = i_edcKonfigData.PRO_i64MaschinenId;
			eDC_CodeKonfigTrackData.PRO_blnIstKonfiguriert = i_edcKonfigData.PRO_blnIstKonfiguriert;
			eDC_CodeKonfigTrackData.PRO_blnWirdVerwendet = i_edcKonfigData.PRO_blnWirdVerwendet;
			eDC_CodeKonfigTrackData.PRO_i64ArrayIndex = i_edcKonfigData.PRO_i64ArrayIndex;
			eDC_CodeKonfigTrackData.PRO_i64Verwendung = i_edcKonfigData.PRO_i64Verwendung;
			eDC_CodeKonfigTrackData.PRO_i64Ort = i_edcKonfigData.PRO_i64Ort;
			eDC_CodeKonfigTrackData.PRO_i64Spur = i_edcKonfigData.PRO_i64Spur;
			eDC_CodeKonfigTrackData.PRO_blnAlbVerwenden = i_edcKonfigData.PRO_blnAlbVerwenden;
			eDC_CodeKonfigTrackData.PRO_blnElbVerwenden = i_edcKonfigData.PRO_blnElbVerwenden;
			eDC_CodeKonfigTrackData.PRO_i64Timeout = i_edcKonfigData.PRO_i64Timeout;
			eDC_CodeKonfigTrackData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_CodeKonfigTrackData.PRO_dtmAngelegtAm = DateTime.Now;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_CodeKonfigTrackData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
