using Ersa.Global.DataProvider.Helfer;
using System.Collections.Specialized;
using System.Data.Common;

namespace Ersa.Global.DataProvider.DatenbankProvider
{
	public abstract class EDC_BasisProvider
	{
		protected NameValueCollection PRO_fdcEinstellungen
		{
			get;
			private set;
		}

		public EDC_BasisProvider(NameValueCollection i_fdcAppSettings)
		{
			PRO_fdcEinstellungen = i_fdcAppSettings;
		}

		public NameValueCollection FUN_fdcHoleProviderEinstellungen()
		{
			return PRO_fdcEinstellungen;
		}

		public string FUN_strHoleDatenbankName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankName(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleServerName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleServerName(PRO_fdcEinstellungen);
		}

		public int FUN_i32HolePortNummer()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_i32HolePortNummer(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleDatenbankBenutzerName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankBenutzerName(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleDatenbankKennwort()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankKennwort(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleDatenbankSource()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankSource(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleDatenbankVerzeichnis()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankVerzeichnis(PRO_fdcEinstellungen);
		}

		public string FUN_strHoleDatenbankBinaerVerzeichnis()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankBinaerVerzeichnis(PRO_fdcEinstellungen);
		}

		protected abstract DbConnectionStringBuilder FUN_fdcConnectionStringBuilderErstellen();
	}
}
