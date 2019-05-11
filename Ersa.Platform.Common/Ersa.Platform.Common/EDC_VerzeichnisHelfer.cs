using System.Configuration;
using System.IO;

namespace Ersa.Platform.Common
{
	public static class EDC_VerzeichnisHelfer
	{
		private const string mC_strDateiAblageRootVerzeichnis = "\\Ersa\\Visu\\ERSA_Data";

		private const string mC_strAppDataVerzeichnisName = "AppData";

		private const string mC_strMachineDataVerzeichnisName = "MachineData";

		private const string mC_strProductionDataVerzeichnisName = "ProductionData";

		private const string mC_strPluginsVerzeichnisName = "Plugins";

		private const string mC_strHilfeDataVerzeichnisName = "\\Ersa\\product_data";

		public static string FUN_strAppDataVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strMaschinenRootVerzeichnisErmitteln(), "AppData");
		}

		public static string FUN_strPluginsVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strAppDataVerzeichnisErmitteln(), "Plugins");
		}

		public static string FUN_strDefaultMachineDataVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strMaschinenRootVerzeichnisErmitteln(), "MachineData");
		}

		public static string FUN_strDefaultProductionDataVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strMaschinenRootVerzeichnisErmitteln(), "ProductionData");
		}

		public static string FUN_strDatenbankVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strAppDataVerzeichnisErmitteln(), "Data");
		}

		public static string FUN_strDefaultKonfigVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strAppDataVerzeichnisErmitteln(), "Default", "Configuration");
		}

		public static string FUN_strDefaultResourcenVerzeichnisErmitteln()
		{
			return Path.Combine(FUN_strAppDataVerzeichnisErmitteln(), "Default", "AdditionalResources");
		}

		public static string FUN_strDefaultHilfeVerzeichnisErmitteln()
		{
			return Path.GetFullPath("\\Ersa\\product_data");
		}

		private static string FUN_strMaschinenRootVerzeichnisErmitteln()
		{
			string fullPath = Path.GetFullPath("\\Ersa\\Visu\\ERSA_Data");
			string text = ConfigurationManager.AppSettings["MachineBaseType"];
			if (!string.IsNullOrEmpty(text))
			{
				return Path.Combine(fullPath, text);
			}
			return fullPath;
		}
	}
}
