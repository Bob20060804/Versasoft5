using Ersa.Global.DataAdapter.Adapter.Impl;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using Ersa.Global.DataProvider.Factories.ProviderFactory;
using System.Collections.Specialized;

namespace Ersa.Global.DataAdapter.Factories
{
	public static class EDC_AdapterFactory
	{
		public static INF_DatenbankAdapter FUN_edcErstelleDatenbankAdapter(NameValueCollection i_fdcDatenbankEinstellungen, INF_DatenbankModel i_edcDatenbankModell)
		{
			return FUN_edcErstelleDenAdapter(i_fdcDatenbankEinstellungen, i_edcDatenbankModell);
		}

		public static INF_ReadonlyDatenbankAdapter FUN_edcErstelleReadonlyDatenbankAdapter(NameValueCollection i_fdcDatenbankEinstellungen, INF_DatenbankModel i_edcDatenbankModell)
		{
			return FUN_edcErstelleDenAdapter(i_fdcDatenbankEinstellungen, i_edcDatenbankModell);
		}

		public static INF_OrganisationsAdapter FUN_edcErstelleOrganisationsAdapter(NameValueCollection i_fdcDatenbankEinstellungen, INF_DatenbankModel i_edcDatenbankModell)
		{
			return FUN_edcErstelleDenAdapter(i_fdcDatenbankEinstellungen, i_edcDatenbankModell);
		}

		private static EDC_DatenbankAdapter FUN_edcErstelleDenAdapter(NameValueCollection i_fdcDatenbankEinstellungen, INF_DatenbankModel i_edcDatenbankModell)
		{
			return new EDC_DatenbankAdapter(EDC_ProviderFactory.FUN_fdcErstelleDatenbankProvider(i_fdcDatenbankEinstellungen), i_edcDatenbankModell);
		}
	}
}
