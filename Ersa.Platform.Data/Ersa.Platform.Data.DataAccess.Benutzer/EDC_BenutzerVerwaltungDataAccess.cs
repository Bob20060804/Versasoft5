using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Benutzer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Benutzer
{
	public class EDC_BenutzerVerwaltungDataAccess : EDC_DataAccess, INF_BenutzerVerwaltungDataAccess, INF_DataAccess
	{
		public EDC_BenutzerVerwaltungDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcGesamtBenutzerListeLadenAsync(IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerAbfrageData(), i_fdcTransaktion);
		}

		public Task<EDC_BenutzerAbfrageData> FUN_fdcMaschinenBenutzerLadenAsync(long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strMaschinenBenutzerMitIdWhereStatementErstellen(i_i64BenutzerId, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenDefaultBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strDefaultMaschinenBenutzerWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenBenutzerListeLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strNichtDefaultMaschinenBenutzerWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<long> FUN_fdcAnzahlAktiverMaschinenBenutzerErmittelnAsync(long i_i64MaschinenId)
		{
			return (await FUN_fdcAktiveMaschinenBenutzerLadenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToList().Count();
		}

		public Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcAktiveMaschinenBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strMaschinenBenutzerAktivWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenBenutzerMitNamenLadenAsync(string i_strName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strMaschinenBenutzerMitNamenWhereStatementErstellen(i_strName, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_BenutzerAbfrageData> FUN_fdcMaschinenBenutzerMitCodeLadenAsync(string i_strCode, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerAbfrageData.FUN_strMaschinenBenutzerMitCodeWhereStatementErstellen(i_strCode, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BenutzerAbfrageData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task FUN_fdcBenutzerHinzufuegenAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				i_edcBenutzer.PRO_dtmAngelegtAm = DateTime.Now;
				if (await FUN_fdcBenutzerMitNamenLadenAsync(i_edcBenutzer.PRO_strBenutzername, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != null)
				{
					throw new InvalidOperationException($"The user {i_edcBenutzer.PRO_strBenutzername} cannot be added because he already exists!");
				}
				await FUN_fdcBenutzerAnlegenAsync(i_edcBenutzer, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcBenutzerMappingAnlegenAsync(i_edcBenutzer, i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public async Task FUN_fdcBenutzerAendernAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				EDC_BenutzerData edcVorhandenerBenutzer = (i_edcBenutzer.PRO_i64BenutzerId <= 0) ? (await FUN_fdcBenutzerMitNamenLadenAsync(i_edcBenutzer.PRO_strBenutzername, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)) : (await FUN_fdcBenutzerDataLadenAsync(i_edcBenutzer.PRO_i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false));
				if (edcVorhandenerBenutzer == null)
				{
					throw new InvalidOperationException($"The user {i_edcBenutzer.PRO_strBenutzername} can not be modified because he does not exist!");
				}
				i_edcBenutzer.PRO_i64BenutzerId = edcVorhandenerBenutzer.PRO_i64BenutzerId;
				await FUN_fdcBenutzerMaschinenBerechtigungenAendernAsync(i_edcBenutzer, i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				edcVorhandenerBenutzer.PRO_strPasswortHash = i_edcBenutzer.PRO_strPasswortHash;
				edcVorhandenerBenutzer.PRO_strPasswortSalt = i_edcBenutzer.PRO_strPasswortSalt;
				edcVorhandenerBenutzer.PRO_blnIstExternerBenutzer = i_edcBenutzer.PRO_blnIstExternerBenutzer;
				edcVorhandenerBenutzer.PRO_strBarcode = i_edcBenutzer.PRO_strBarcode;
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcVorhandenerBenutzer, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public async Task FUN_fdcBenutzerSynchronisierenAsync(EDC_BenutzerAbfrageData i_edcSynchronBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				EDC_BenutzerData edcExistierenderBenutzer = await FUN_fdcBenutzerDataLadenAsync(i_edcSynchronBenutzer.PRO_i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (edcExistierenderBenutzer == null)
				{
					throw new InvalidOperationException("The user can not be modified because he does not exist!");
				}
				await FUN_fdcBenutzerMaschinenBerechtigungenAendernAsync(i_edcSynchronBenutzer, i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				edcExistierenderBenutzer.PRO_blnIstAktiv = i_edcSynchronBenutzer.PRO_blnIstAktiv;
				edcExistierenderBenutzer.PRO_blnIstExternerBenutzer = i_edcSynchronBenutzer.PRO_blnIstExternerBenutzer;
				edcExistierenderBenutzer.PRO_strBarcode = i_edcSynchronBenutzer.PRO_strBarcode;
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcExistierenderBenutzer, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public async Task FUN_fdcFuehreTabellenMappingMigrationDurchAsync()
		{
			IDbTransaction fdcTransaktion = null;
			try
			{
				fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<EDC_BenutzerData> list = (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerData(), fdcTransaktion)).ToList();
				foreach (EDC_BenutzerData item in list)
				{
					EDC_BenutzerMappingData i_edcObjekt = new EDC_BenutzerMappingData
					{
						PRO_i64BenutzerId = item.PRO_i64BenutzerId,
						PRO_i64MaschinenId = item.PRO_i64MaschinenId,
						PRO_i32Rechte = item.PRO_i32Rechte,
						PRO_blnIstAktivNachAutoAbmeldung = item.PRO_blnIstAktivNachAutoAbmeldung,
						PRO_blnIstAktiv = item.PRO_blnIstAktiv
					};
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcBenutzerMaschinenBerechtigungenAendernAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_BenutzerMappingData eDC_BenutzerMappingData = await FUN_fdcBenutzerMappingDataLadenAsync(i_edcBenutzer.PRO_i64BenutzerId, i_i64MaschinenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_BenutzerMappingData != null)
			{
				eDC_BenutzerMappingData.PRO_i32Rechte = i_edcBenutzer.PRO_i32Rechte;
				eDC_BenutzerMappingData.PRO_blnIstAktivNachAutoAbmeldung = i_edcBenutzer.PRO_blnIstAktivNachAutoAbmeldung;
				if (!i_edcBenutzer.PRO_blnIstDefaultBenutzer)
				{
					eDC_BenutzerMappingData.PRO_blnIstAktiv = i_edcBenutzer.PRO_blnIstAktiv;
				}
				if (!i_edcBenutzer.PRO_blnIstAktiv)
				{
					eDC_BenutzerMappingData.PRO_blnIstAktivNachAutoAbmeldung = false;
				}
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(eDC_BenutzerMappingData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				eDC_BenutzerMappingData = new EDC_BenutzerMappingData
				{
					PRO_i64BenutzerId = i_edcBenutzer.PRO_i64BenutzerId,
					PRO_i64MaschinenId = i_i64MaschinenId,
					PRO_i32Rechte = i_edcBenutzer.PRO_i32Rechte,
					PRO_blnIstAktivNachAutoAbmeldung = false,
					PRO_blnIstAktiv = true
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_BenutzerMappingData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task FUN_fdcBenutzerAnlegenAsync(EDC_BenutzerAbfrageData i_edcBenutzer, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcBenutzer.PRO_i64BenutzerId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			EDC_BenutzerData i_edcObjekt = i_edcBenutzer.FUN_edcConvertToBenutzerData();
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		private Task FUN_fdcBenutzerMappingAnlegenAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_BenutzerMappingData eDC_BenutzerMappingData = i_edcBenutzer.FUN_edcConvertToMappingData();
			eDC_BenutzerMappingData.PRO_i64MaschinenId = i_i64MaschinenId;
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_BenutzerMappingData, i_fdcTransaktion);
		}

		private Task<EDC_BenutzerData> FUN_fdcBenutzerMitNamenLadenAsync(string i_strName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerData.FUN_strBenutzerNamenWhereStatement(i_strName);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BenutzerData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		private Task<EDC_BenutzerData> FUN_fdcBenutzerDataLadenAsync(long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerData.FUN_strBenutzerIdWhereStatementErstellen(i_i64BenutzerId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BenutzerData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		private Task<EDC_BenutzerMappingData> FUN_fdcBenutzerMappingDataLadenAsync(long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BenutzerMappingData.FUN_strBenutzerUndMaschinenIdWhereStatementErstellen(i_i64BenutzerId, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BenutzerMappingData(i_strWhereStatement), null, i_fdcTransaktion);
		}
	}
}
