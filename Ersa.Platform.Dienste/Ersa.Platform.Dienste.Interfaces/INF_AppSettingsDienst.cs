using System;

namespace Ersa.Platform.Dienste.Interfaces
{
	[Obsolete("Ersa.Global.Dienste.INF_AppSettingsDienst verwenden")]
	public interface INF_AppSettingsDienst
	{
		string FUN_strAppSettingErmitteln(string i_strKey);

		void SUB_AppSettingSpeichern(string i_strKey, string i_strWert);
	}
}
