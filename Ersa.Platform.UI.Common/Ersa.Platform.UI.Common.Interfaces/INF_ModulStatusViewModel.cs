using Ersa.Platform.Infrastructure;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_ModulStatusViewModel
	{
		void SUB_AktualisiereDasModul(string i_strModulNameKey, ENUM_ModulStatus i_enmModulStatus, string i_strMeldungKey);
	}
}
