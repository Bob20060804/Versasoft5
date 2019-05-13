using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Nutzen;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataContracts.Nutzen;
using Ersa.Platform.DataDienste.Nutzen.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Nutzen
{
	[Export(typeof(INF_NutzenDatenVerwaltungsDienst))]
	public class EDC_NutzenDatenVerwaltungsDienst : INF_NutzenDatenVerwaltungsDienst
	{
		private readonly Lazy<INF_NutzenDataAccess> m_edcNutzenDataAccess;

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_NutzenDatenVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider)
		{
			m_edcNutzenDataAccess = new Lazy<INF_NutzenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_NutzenDataAccess>);
		}

		public async Task FUN_fdcNestDatenFuerNutzenSpeichernAsync(string i_strHash, IEnumerable<EDC_NestDaten> i_enuNestDaten, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcNutzenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_NestDaten item in i_enuNestDaten)
				{
					await FUN_fdcNestDatenSpeichernAsync(i_strHash, item, fdcTransaktion);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcNutzenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					m_edcNutzenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task<EDC_NestDaten> FUN_fdcNestDatenErmittelnAsync(string i_strHash, string i_strNestCode, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_NutzenData eDC_NutzenData = await m_edcNutzenDataAccess.Value.FUN_fdcLeseNestDatenFuerNestCodeAusDatenbankAsync(i_strHash, i_strNestCode, i_fdcTransaktion);
			return PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_NestDaten>(eDC_NutzenData.PRO_strNestDaten);
		}

		public async Task<IEnumerable<EDC_NestDaten>> FUN_fdcNutzenDatenErmittelnAsync(string i_strHash, string i_strNutzenCode, IDbTransaction i_fdcTransaktion = null)
		{
			return FUN_fdcErzeugePcbDatenListe(await m_edcNutzenDataAccess.Value.FUN_fdcLeseNestDatenFuerNutzenCodeAusDatenbankAsync(i_strHash, i_strNutzenCode, i_fdcTransaktion));
		}

		public async Task<IEnumerable<EDC_NestDaten>> FUN_fdcDatenZuHashErmittelnAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null)
		{
			return FUN_fdcErzeugePcbDatenListe(await m_edcNutzenDataAccess.Value.FUN_fdcLeseNestDatenZuHashAusDatenbankAsync(i_strHash, i_fdcTransaktion));
		}

		private List<EDC_NestDaten> FUN_fdcErzeugePcbDatenListe(IEnumerable<EDC_NutzenData> i_enuNestData)
		{
			List<EDC_NestDaten> list = new List<EDC_NestDaten>();
			foreach (EDC_NutzenData i_enuNestDatum in i_enuNestData)
			{
				list.Add(PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_NestDaten>(i_enuNestDatum.PRO_strNestDaten));
			}
			return list;
		}

		private async Task FUN_fdcNestDatenSpeichernAsync(string i_strHash, EDC_NestDaten i_edcNestDaten, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strNestDaten = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcNestDaten);
			await m_edcNutzenDataAccess.Value.FUN_fdcSchreibeNutzenDataInDatenbankAsync(i_strHash, i_edcNestDaten.PRO_strNutzenCode, i_edcNestDaten.PRO_strNestCode, i_strNestDaten, i_fdcTransaktion);
		}
	}
}
