using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.BetriebsmittelVerwaltung
{
	[Export(typeof(INF_RuestkomponentenVerwaltungsDienst))]
	public class EDC_RuestkomponentenVerwaltungsDienst : EDC_DataDienst, INF_RuestkomponentenVerwaltungsDienst
	{
		private readonly Lazy<INF_RuestkomponentenDataAccess> m_edcRuestkomponentenDataAccess;

		private readonly Lazy<INF_RuestwerkzeugeDataAccess> m_edcRuestwerkzeugeDataAccess;

		[ImportingConstructor]
		public EDC_RuestkomponentenVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcRuestkomponentenDataAccess = new Lazy<INF_RuestkomponentenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_RuestkomponentenDataAccess>);
			m_edcRuestwerkzeugeDataAccess = new Lazy<INF_RuestwerkzeugeDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_RuestwerkzeugeDataAccess>);
		}

		public async Task<EDC_RuestkomponentenData> FUN_fdcLeseRuestkomponentenDatenFuerFuerKomponentenIdAsync(long i_i64RuestkomponentenId)
		{
			return await m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponenteFuerIdAusDatenbankAsync(i_i64RuestkomponentenId);
		}

		public async Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcLeseRuestkomponentenDatenFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp)
		{
			long i_i64MachineGroupId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponentenFuerTypeAusDatenbankAsync(i_i64MachineGroupId, (int)i_enmRuestkomponentenTyp);
		}

		public async Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcRuestkomponentenDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null)
		{
			long i64MaschinenGruppenId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = i_fdcTransaktion ?? (await m_edcRuestkomponentenDataAccess.Value.FUN_fdcStarteTransaktionAsync());
			try
			{
				List<EDC_RuestkomponentenData> lstRuestkomponentenData = new List<EDC_RuestkomponentenData>();
				foreach (EDC_RuestkomponentenData i_enuRuestkomponentenDatum in i_enuRuestkomponentenData)
				{
					i_enuRuestkomponentenDatum.PRO_i64MachinenGruppenId = i64MaschinenGruppenId;
					List<EDC_RuestkomponentenData> list = lstRuestkomponentenData;
					list.Add(await m_edcRuestkomponentenDataAccess.Value.FUN_fdcRuestkomponentenDatenSatzHinzufuegenAsync(i_enuRuestkomponentenDatum, fdcTransaktion));
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				return lstRuestkomponentenData;
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcRuestkomponentenDatenSaetzeAendernAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcRuestkomponentenDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_RuestkomponentenData i_enuRuestkomponentenDatum in i_enuRuestkomponentenData)
				{
					await m_edcRuestkomponentenDataAccess.Value.FUN_fdcRuestkomponentenDatenSatzAendernAsync(i_enuRuestkomponentenDatum, fdcTransaktion);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcRuestkomponentenDatenSaetzeLoeschenAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcRuestkomponentenDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_RuestkomponentenData edcRuestkomponentenData in i_enuRuestkomponentenData)
				{
					await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcRuestwerkzeugDatenLoeschenFuerKomponentenIdAsync(edcRuestkomponentenData.PRO_i64RuestkomponentenId, fdcTransaktion);
					await m_edcRuestkomponentenDataAccess.Value.FUN_fdcRuestkomponentenDatenSatzLoeschenAsync(edcRuestkomponentenData.PRO_i64RuestkomponentenId, fdcTransaktion);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestkomponentenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcLeseRuestwerkzeugDatenFuerKomponentenIdAsync(long i_i64RuestkomponentenId)
		{
			return m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcLeseRuestwerkzeugeFuerKomponentenIdAusDatenbankAsync(i_i64RuestkomponentenId);
		}

		public async Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcRuestwerkzeugDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				List<EDC_RuestwerkzeugeData> lstRuestwerkzeugeData = new List<EDC_RuestwerkzeugeData>();
				foreach (EDC_RuestwerkzeugeData i_enuRuestwerkzeugDatum in i_enuRuestwerkzeugData)
				{
					List<EDC_RuestwerkzeugeData> list = lstRuestwerkzeugeData;
					list.Add(await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcRuestwerkzeugDatenSatzHinzufuegenAsync(i_enuRuestwerkzeugDatum, fdcTransaktion));
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				return lstRuestwerkzeugeData;
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcRuestwerkzeugDatenSaetzeAendernAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_RuestwerkzeugeData i_enuRuestwerkzeugeDatum in i_enuRuestwerkzeugeData)
				{
					await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcRuestwerkzeugDatenSatzAendernAsync(i_enuRuestwerkzeugeDatum, fdcTransaktion);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcRuestwerkzeugDatenSaetzeLoeschenAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_RuestwerkzeugeData i_enuRuestwerkzeugeDatum in i_enuRuestwerkzeugeData)
				{
					await m_edcRuestwerkzeugeDataAccess.Value.FUN_fdcRuestwerkzeugDatenSatzLoeschenAsync(i_enuRuestwerkzeugeDatum.PRO_i64RuestwerkzeugId, fdcTransaktion);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					m_edcRuestwerkzeugeDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp)
		{
			long i_i64MachineGroupId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerTypAsync(i_i64MachineGroupId, i_enmRuestkomponentenTyp);
		}

		public Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerNameAsync(string i_strName)
		{
			return m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerKomponentenNameAsync(i_strName);
		}

		public Task<EDC_RuestkomponentenAbfrageData> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerIdentifikationAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp, string i_strIdentifikation)
		{
			return m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponenteUndWerkzeugFuerIdentifikationAsync(i_enmRuestkomponentenTyp, i_strIdentifikation);
		}

		public async Task<IEnumerable<EDC_Ruestkomponente>> FUN_fdcLeseRuestkomponenteFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp)
		{
			List<EDC_Ruestkomponente> lstRuestkomponenten = new List<EDC_Ruestkomponente>();
			long i_i64MachineGroupId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			foreach (EDC_RuestkomponentenData edcRuestkomponentenData in await m_edcRuestkomponentenDataAccess.Value.FUN_fdcLeseRuestkomponentenFuerTypeAusDatenbankAsync(i_i64MachineGroupId, (int)i_enmRuestkomponentenTyp))
			{
				IEnumerable<EDC_RuestwerkzeugeData> source = await FUN_fdcLeseRuestwerkzeugDatenFuerKomponentenIdAsync(edcRuestkomponentenData.PRO_i64RuestkomponentenId);
				lstRuestkomponenten.Add(new EDC_Ruestkomponente
				{
					PRO_edcRuestkomponenteData = edcRuestkomponentenData,
					PRO_enuRuestwerkzeuge = from i_edcRuestwerkzeugeData in source
					select new EDC_Ruestwerkzeug
					{
						PRO_edcRuestwerkzeugeData = i_edcRuestwerkzeugeData
					}
				});
			}
			return lstRuestkomponenten;
		}

		public async Task<IEnumerable<EDC_Ruestkomponente>> FUN_fdcAktualisiereRuestkomponentenAsync(IEnumerable<EDC_Ruestkomponente> i_lstRuestkomponenten)
		{
			IDbTransaction fdcTransaktion = await m_edcRuestkomponentenDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			ENUM_RuestkomponentenTyp enmRuestkomponentenTyp = ENUM_RuestkomponentenTyp.Niederhalter;
			try
			{
				foreach (EDC_Ruestkomponente edcRuestkomponente in i_lstRuestkomponenten)
				{
					enmRuestkomponentenTyp = edcRuestkomponente.PRO_edcRuestkomponenteData.PRO_enmTyp;
					if (edcRuestkomponente.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId == 0L && !edcRuestkomponente.PRO_blnGeloescht)
					{
						await FUN_fdcBehandleNeueRuestkomponenteAsync(edcRuestkomponente, fdcTransaktion);
					}
					else if (edcRuestkomponente.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId != 0L && edcRuestkomponente.PRO_blnGeloescht)
					{
						await FUN_fdcRuestkomponentenDatenSaetzeLoeschenAsync(new List<EDC_RuestkomponentenData>
						{
							edcRuestkomponente.PRO_edcRuestkomponenteData
						}, fdcTransaktion);
					}
					else if (edcRuestkomponente.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId != 0L)
					{
						await FUN_fdcBehandleGeaenderteRuestkomponenteAsync(edcRuestkomponente, fdcTransaktion);
					}
				}
				m_edcRuestkomponentenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcRuestkomponentenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
			return await FUN_fdcLeseRuestkomponenteFuerTypAsync(enmRuestkomponentenTyp);
		}

		private async Task FUN_fdcBehandleNeueRuestkomponenteAsync(EDC_Ruestkomponente i_edcRuestkomponente, IDbTransaction i_fdcTransaktion)
		{
			IEnumerable<EDC_RuestkomponentenData> source = await FUN_fdcRuestkomponentenDatenSaetzeHinzufuegenAsync(new List<EDC_RuestkomponentenData>
			{
				i_edcRuestkomponente.PRO_edcRuestkomponenteData
			}, i_fdcTransaktion);
			if (i_edcRuestkomponente.PRO_enuRuestwerkzeuge != null)
			{
				long pRO_i64RuestkomponentenId = source.First().PRO_i64RuestkomponentenId;
				List<EDC_RuestwerkzeugeData> list = new List<EDC_RuestwerkzeugeData>();
				foreach (EDC_Ruestwerkzeug item in i_edcRuestkomponente.PRO_enuRuestwerkzeuge)
				{
					EDC_RuestwerkzeugeData pRO_edcRuestwerkzeugeData = item.PRO_edcRuestwerkzeugeData;
					pRO_edcRuestwerkzeugeData.PRO_i64RuestkomponentenId = pRO_i64RuestkomponentenId;
					list.Add(pRO_edcRuestwerkzeugeData);
				}
				await FUN_fdcRuestwerkzeugDatenSaetzeHinzufuegenAsync(list, i_fdcTransaktion);
			}
		}

		private async Task FUN_fdcBehandleGeaenderteRuestkomponenteAsync(EDC_Ruestkomponente i_edcRuestkomponente, IDbTransaction i_fdcTransaktion)
		{
			if (i_edcRuestkomponente.PRO_blnGeaendert)
			{
				await FUN_fdcRuestkomponentenDatenSaetzeAendernAsync(new List<EDC_RuestkomponentenData>
				{
					i_edcRuestkomponente.PRO_edcRuestkomponenteData
				}, i_fdcTransaktion);
			}
			if (i_edcRuestkomponente.PRO_enuRuestwerkzeuge != null)
			{
				List<EDC_RuestwerkzeugeData> list = new List<EDC_RuestwerkzeugeData>();
				List<EDC_RuestwerkzeugeData> lstGeloeschteRuestwerkzeugeData = new List<EDC_RuestwerkzeugeData>();
				List<EDC_RuestwerkzeugeData> lstGeaenderteRuestwerkzeugeData = new List<EDC_RuestwerkzeugeData>();
				foreach (EDC_Ruestwerkzeug item in i_edcRuestkomponente.PRO_enuRuestwerkzeuge)
				{
					EDC_RuestwerkzeugeData pRO_edcRuestwerkzeugeData = item.PRO_edcRuestwerkzeugeData;
					if (pRO_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId == 0L && !item.PRO_blnGeloescht)
					{
						list.Add(pRO_edcRuestwerkzeugeData);
					}
					else if (pRO_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId != 0L && item.PRO_blnGeloescht)
					{
						lstGeloeschteRuestwerkzeugeData.Add(pRO_edcRuestwerkzeugeData);
					}
					else if (pRO_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId != 0L && item.PRO_blnGeaendert)
					{
						lstGeaenderteRuestwerkzeugeData.Add(pRO_edcRuestwerkzeugeData);
					}
				}
				if (list.Any())
				{
					await FUN_fdcRuestwerkzeugDatenSaetzeHinzufuegenAsync(list, i_fdcTransaktion);
				}
				if (lstGeloeschteRuestwerkzeugeData.Any())
				{
					await FUN_fdcRuestwerkzeugDatenSaetzeLoeschenAsync(lstGeloeschteRuestwerkzeugeData, i_fdcTransaktion);
				}
				if (lstGeaenderteRuestwerkzeugeData.Any())
				{
					await FUN_fdcRuestwerkzeugDatenSaetzeAendernAsync(lstGeaenderteRuestwerkzeugeData, i_fdcTransaktion);
				}
			}
		}
	}
}
