using System;

namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_HilfeDienst
	{
		Uri FUN_fdcHilfeUriErmitteln(string i_strHilfeKey);

		Uri FUN_fdcAnleitungUriErmitteln();
	}
}
