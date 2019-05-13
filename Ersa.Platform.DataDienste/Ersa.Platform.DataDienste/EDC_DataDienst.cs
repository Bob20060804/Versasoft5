using Ersa.Platform.CapabilityContracts.BenutzerVerwaltung;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste
{
	public abstract class EDC_DataDienst
	{
		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		private readonly Lazy<INF_BenutzerVerwaltungCapability> m_edcBenutzerVerwaltungCapability;

		protected EDC_DataDienst(INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			m_edcBenutzerVerwaltungCapability = new Lazy<INF_BenutzerVerwaltungCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_BenutzerVerwaltungCapability>);
		}

		protected static void SUB_WertInTabellenSpalteErsetzen<T>(DataSet i_fdcDataSet, string i_strTabellenName, string i_strSpaltenName, T i_objAlterWert, T i_objNeuerWert)
		{
			SUB_WertInTabellenSpalteErsetzen(i_fdcDataSet?.Tables[i_strTabellenName], i_strSpaltenName, i_objAlterWert, i_objNeuerWert);
		}

		protected static void SUB_WertInTabellenSpalteErsetzen<T>(DataTable i_fdcDataTable, string i_strSpaltenName, T i_objAlterWert, T i_objNeuerWert)
		{
			if (i_fdcDataTable != null)
			{
				foreach (DataRow item in i_fdcDataTable.AsEnumerable().Where(delegate(DataRow i_fdcZeile)
				{
					if (i_fdcZeile.Field<T>(i_strSpaltenName) != null)
					{
						return i_fdcZeile.Field<T>(i_strSpaltenName).Equals(i_objAlterWert);
					}
					return false;
				}))
				{
					item[i_strSpaltenName] = i_objNeuerWert;
				}
			}
		}

		protected static void SUB_ZeilenMitWertEntfernen<T>(DataSet i_fdcDataSet, string i_strTabellenName, string i_strSpaltenName, T i_objWert)
		{
			SUB_ZeilenMitWertEntfernen(i_fdcDataSet?.Tables[i_strTabellenName], i_strSpaltenName, i_objWert);
		}

		protected static void SUB_ZeilenMitWertEntfernen<T>(DataTable i_fdcDataTable, string i_strSpaltenName, T i_objWert)
		{
			if (i_fdcDataTable != null)
			{
				foreach (DataRow item in i_fdcDataTable.AsEnumerable().Where(delegate(DataRow i_fdcZeile)
				{
					if (i_fdcZeile.Field<T>(i_strSpaltenName) != null)
					{
						return i_fdcZeile.Field<T>(i_strSpaltenName).Equals(i_objWert);
					}
					return false;
				}).ToList())
				{
					i_fdcDataTable.Rows.Remove(item);
				}
			}
		}

		protected Task<long> FUN_i64HoleAktuelleMaschinenIdAsync()
		{
			return m_edcMaschinenBasisDatenCapability.Value?.FUN_fdcHoleMaschinenIdAsync() ?? Task.FromResult(0L);
		}

		protected async Task<long> FUN_i64HoleZugewieseneGruppenIdAsync()
		{
			if (m_edcMaschinenBasisDatenCapability.Value == null)
			{
				return 0L;
			}
			return (await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleZugewieseneGruppenIdsAsync().ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
		}

		protected Task<long> FUN_i64HoleAktuelleBenutzerIdAsync()
		{
			return Task.FromResult(m_edcBenutzerVerwaltungCapability.Value?.FUN_i64AktuelleBenutzerIdHolen() ?? 0);
		}
	}
}
