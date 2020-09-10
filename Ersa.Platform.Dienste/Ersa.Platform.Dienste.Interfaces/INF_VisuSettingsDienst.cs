namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_VisuSettingsDienst
	{
		void SUB_Initialisieren(string i_strSettingsFilePath);

		void SUB_SettingsAnwenden();

		void SUB_Speichern();

		string FUN_strGlobalSettingWertErmitteln(string i_strKey);
	}
}
