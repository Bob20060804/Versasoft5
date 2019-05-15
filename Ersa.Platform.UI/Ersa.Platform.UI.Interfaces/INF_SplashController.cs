using System;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_SplashController
	{
		bool PRO_blnIsVisible
		{
			get;
		}

		void SUB_ErstelleDenSplash(Action i_fdcCloseAction);

		void SUB_BeendeDenSplashAsync();

		Task FUN_fdcBeendeDenSplashMitSchliessenAktionAsync();
	}
}
