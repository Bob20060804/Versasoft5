using Ersa.Global.DataProvider.Helfer;
using System.Collections.Specialized;
using System.Data.Common;

namespace Ersa.Global.DataProvider.DatenbankProvider
{
	public abstract class EDC_BasisProvider
	{
        /// <summary>
        /// Settings
        /// </summary>
		protected NameValueCollection PRO_fdcEinstellungen
		{
			get;
			private set;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_fdcAppSettings"></param>
		public EDC_BasisProvider(NameValueCollection i_fdcAppSettings)
		{
			PRO_fdcEinstellungen = i_fdcAppSettings;
		}

        /// <summary>
        /// Get Provider Settings
        /// </summary>
        /// <returns></returns>
		public NameValueCollection FUN_fdcHoleProviderEinstellungen()
		{
			return PRO_fdcEinstellungen;
		}

        /// <summary>
        /// Get Database Name
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankName(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get Server Name
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleServerName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleServerName(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get Port Number
        /// </summary>
        /// <returns></returns>
		public int FUN_i32HolePortNummer()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_i32HolePortNummer(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get database username
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankBenutzerName()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankBenutzerName(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get database password
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankKennwort()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankKennwort(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get database source
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankSource()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankSource(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get database Directory
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankVerzeichnis()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankVerzeichnis(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Get database binary directory
        /// </summary>
        /// <returns></returns>
		public string FUN_strHoleDatenbankBinaerVerzeichnis()
		{
			return EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankBinaerVerzeichnis(PRO_fdcEinstellungen);
		}

        /// <summary>
        /// Create connection string builder
        /// </summary>
        /// <returns></returns>
		protected abstract DbConnectionStringBuilder FUN_fdcConnectionStringBuilderErstellen();
	}
}
