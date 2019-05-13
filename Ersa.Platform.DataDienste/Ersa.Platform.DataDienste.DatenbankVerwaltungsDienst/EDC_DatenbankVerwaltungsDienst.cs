using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts.Datenbankdaten;
using Ersa.Platform.DataContracts.DatenbankVerwaltung;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using Ersa.Platform.DataDienste.DatenbankVerwaltungsDienst.Interfaces;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.DatenbankVerwaltungsDienst
{
	[Export(typeof(INF_DatenbankVerwaltungsDienst))]
	public class EDC_DatenbankVerwaltungsDienst : INF_DatenbankVerwaltungsDienst
	{
		private readonly INF_DatenbankVerwaltungDataAccess m_edcDatenbankVerwaltungDataAccess;

		private readonly INF_DatenbankdatenDataAccess m_edcDatenbankdatenDataAccess;

		private readonly INF_LoetprogrammProgrammDataAccess m_edcProgrammDataAccess;

		private readonly INF_LoetprogrammVersionDataAccess m_edcProgrammVersionDataAccess;

		private readonly INF_LoetprogrammParameterDataAccess m_edcParameterDataAccess;

		private readonly INF_MaschinenVerwaltungsDienst m_edcMaschinenVerwaltungsDienst;

		[ImportingConstructor]
		public EDC_DatenbankVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_MaschinenVerwaltungsDienst i_edcMaschinenVerwaltungsDienst)
		{
			m_edcDatenbankVerwaltungDataAccess = i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DatenbankVerwaltungDataAccess>();
			m_edcDatenbankdatenDataAccess = i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DatenbankdatenDataAccess>();
			m_edcProgrammDataAccess = i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammProgrammDataAccess>();
			m_edcProgrammVersionDataAccess = i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammVersionDataAccess>();
			m_edcParameterDataAccess = i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammParameterDataAccess>();
			m_edcMaschinenVerwaltungsDienst = i_edcMaschinenVerwaltungsDienst;
		}

		public Task<long> FUN_fdcLeseLoetprogrammVariablenVersionAusDatenbankAsync()
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcLeseLoetprogrammVariablenVersionAusDatenbankAsync();
		}

		public Task FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(long i_i64Version)
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(i_i64Version);
		}

		public Task<long> FUN_fdcLeseProzessschreiberVariablenVersionAusDatenbankAsync()
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcLeseProzessschreiberVariablenVersionAusDatenbankAsync();
		}

		public Task FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(long i_i64Version)
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(i_i64Version);
		}

		public Task<long> FUN_fdcLeseLoetprotokollVariablenVersionAusDatenbankAsync()
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcLeseLoetprotokollVariablenVersionAusDatenbankAsync();
		}

		public Task FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(long i_i64Version)
		{
			return m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(i_i64Version);
		}

		public async Task FUN_fdcUpdateLoetprogrammVariablenAsync(IDictionary<string, string> i_dicNamenMapping, IEnumerable<EDC_LoetprogrammParameterData> i_enuNeueVariablen, long i_i64NeueVersion)
		{
			IDbTransaction fdcTransaktion = await m_edcDatenbankVerwaltungDataAccess.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long[] i_enuMaschinenGruppenIds = await m_edcMaschinenVerwaltungsDienst.FUNa_i64AktiveGruppenIdsErmittelnAsync();
				List<EDC_LoetprogrammData> lstBetroffeneLoetprogramme = (await m_edcProgrammDataAccess.FUN_fdcHoleProgrammeZuMaschinenGruppenAsync(i_enuMaschinenGruppenIds, fdcTransaktion)).ToList();
				if (lstBetroffeneLoetprogramme.Any())
				{
					await m_edcDatenbankVerwaltungDataAccess.FUN_fdcAktualisiereWerteInTabelleAsync("ProgramParameter", "Variable", i_dicNamenMapping, fdcTransaktion);
					await FUN_fdcFuegeNeueParameterHinzuAsync(lstBetroffeneLoetprogramme, i_enuNeueVariablen, fdcTransaktion);
					await FUN_fdcSetzeNeueProgrammVersionen(lstBetroffeneLoetprogramme, i_i64NeueVersion, fdcTransaktion);
				}
				await m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(i_i64NeueVersion, fdcTransaktion);
				m_edcDatenbankVerwaltungDataAccess.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcDatenbankVerwaltungDataAccess.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcUpdateProzessschreiberVariablenAsync(IDictionary<string, string> i_dicMapping, long i_i64NeueVersion)
		{
			IDbTransaction fdcTransaktion = await m_edcDatenbankVerwaltungDataAccess.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankVerwaltungDataAccess.FUN_fdcAktualisiereWerteInTabelleAsync("RecorderVariables", "Variable", i_dicMapping, fdcTransaktion);
				await m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(i_i64NeueVersion, fdcTransaktion);
				m_edcDatenbankVerwaltungDataAccess.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcDatenbankVerwaltungDataAccess.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcUpdateLoetprotokollVariablenAsync(IDictionary<string, string> i_dicMapping, long i_i64NeueVersion)
		{
			IDbTransaction fdcTransaktion = await m_edcDatenbankVerwaltungDataAccess.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankVerwaltungDataAccess.FUN_fdcAktualisiereWerteInTabelleAsync("ProtocolVariables", "Variable", i_dicMapping, fdcTransaktion);
				await m_edcDatenbankdatenDataAccess.FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(i_i64NeueVersion, fdcTransaktion);
				m_edcDatenbankVerwaltungDataAccess.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcDatenbankVerwaltungDataAccess.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcFuegeNeueParameterHinzuAsync(IEnumerable<EDC_LoetprogrammData> i_enuBetroffeneProgramme, IEnumerable<EDC_LoetprogrammParameterData> i_enuNeueVariablen, IDbTransaction i_fdcTransaktion)
		{
			if (i_enuNeueVariablen != null)
			{
				List<EDC_LoetprogrammParameterData> lstLoetprogrammParameterData = i_enuNeueVariablen.ToList();
				foreach (EDC_LoetprogrammData item in i_enuBetroffeneProgramme)
				{
					foreach (EDC_LoetprogrammVersionAbfrageData item2 in await m_edcProgrammVersionDataAccess.FUN_fdcHoleVersionenStapelAsync(item.PRO_i64ProgrammId, i_fdcTransaktion))
					{
						await m_edcParameterDataAccess.FUN_fdcParameterListeHinzufuegenAsync(lstLoetprogrammParameterData, item2.PRO_i64VersionsId, i_fdcTransaktion);
					}
				}
			}
		}

		private async Task FUN_fdcSetzeNeueProgrammVersionen(IEnumerable<EDC_LoetprogrammData> i_lstBetroffeneLoetprogramme, long i_i64NeueVersion, IDbTransaction i_fdcTransaktion)
		{
			IEnumerable<long> i_enuProgrammId = from i_edcProgramm in i_lstBetroffeneLoetprogramme
			select i_edcProgramm.PRO_i64ProgrammId;
			await m_edcProgrammDataAccess.FUN_fdcSetzeNeueProgrammVersion(i_enuProgrammId, i_i64NeueVersion, i_fdcTransaktion);
		}
	}
}
