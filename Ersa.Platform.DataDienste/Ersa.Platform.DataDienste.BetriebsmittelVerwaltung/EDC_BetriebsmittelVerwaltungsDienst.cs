using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.BetriebsmittelVerwaltung
{
	[Export(typeof(INF_BetriebsmittelVerwaltungsDienst))]
	public class EDC_BetriebsmittelVerwaltungsDienst : EDC_DataDienst, INF_BetriebsmittelVerwaltungsDienst
	{
		private readonly Lazy<INF_BetriebsmittelDataAccess> m_edcBetriebsmittelDataAccess;

		[ImportingConstructor]
		public EDC_BetriebsmittelVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcBetriebsmittelDataAccess = new Lazy<INF_BetriebsmittelDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_BetriebsmittelDataAccess>);
		}

		public async Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcLeseBetriebsmittelDatenFuerTypAsync(ENUM_BetriebsmittelTyp i_enuBetriebsmittelTyp)
		{
			return await m_edcBetriebsmittelDataAccess.Value.FUN_fdcLeseBetriebsmittelFuerTypeAusDatenbankAsync((int)i_enuBetriebsmittelTyp);
		}

		public Task<EDC_BetriebsmittelData> FUN_fdcLeseBetriebsmittelDatenFuerIdAsync(long i_i64BetriebsmittelId)
		{
			return m_edcBetriebsmittelDataAccess.Value.FUN_fdcLeseBetriebsmittelDatenFuerIdAusDatenbankAsync(i_i64BetriebsmittelId);
		}

		public async Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcBetriebsmittelDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData)
		{
			IDbTransaction fdcTransaktion = await m_edcBetriebsmittelDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			try
			{
				List<EDC_BetriebsmittelData> lstBetriebsmittelData = new List<EDC_BetriebsmittelData>();
				foreach (EDC_BetriebsmittelData i_enuBetriebsmittelDatum in i_enuBetriebsmittelData)
				{
					List<EDC_BetriebsmittelData> list = lstBetriebsmittelData;
					list.Add(await m_edcBetriebsmittelDataAccess.Value.FUN_fdcBetriebsmittelDatenSatzHinzufuegenAsync(i_enuBetriebsmittelDatum, fdcTransaktion));
				}
				m_edcBetriebsmittelDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return lstBetriebsmittelData;
			}
			catch (Exception)
			{
				m_edcBetriebsmittelDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcBetriebsmittelDatenSaetzeAendernAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData)
		{
			IDbTransaction fdcTransaktion = await m_edcBetriebsmittelDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			try
			{
				foreach (EDC_BetriebsmittelData i_enuBetriebsmittelDatum in i_enuBetriebsmittelData)
				{
					await m_edcBetriebsmittelDataAccess.Value.FUN_fdcBetriebsmittelDatenSatzAendernAsync(i_enuBetriebsmittelDatum, fdcTransaktion);
				}
				m_edcBetriebsmittelDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcBetriebsmittelDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcBetriebsmittelDatenSaetzeLoeschenAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData)
		{
			IDbTransaction fdcTransaktion = await m_edcBetriebsmittelDataAccess.Value.FUN_fdcStarteTransaktionAsync();
			try
			{
				foreach (EDC_BetriebsmittelData i_enuBetriebsmittelDatum in i_enuBetriebsmittelData)
				{
					await m_edcBetriebsmittelDataAccess.Value.FUN_fdcBetriebsmittelDatenSatzLoeschenAsync(i_enuBetriebsmittelDatum.PRO_i64BetriebsmittelId, fdcTransaktion);
				}
				m_edcBetriebsmittelDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcBetriebsmittelDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}
	}
}
