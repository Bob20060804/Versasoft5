namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_AppSettingsDienst
	{
		string FUN_strAppSettingErmitteln(string i_strKey);

		void SUB_AppSettingSpeichern(string i_strKey, string i_strWert);
	}
}
