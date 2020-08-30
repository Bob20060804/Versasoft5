using Ersa.Platform.Common.Meldungen;
using System;

namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_MeldungsHilfeDienst
	{
		Uri FUN_fdcErweiterteHilfeUriErmitteln(INF_Meldung i_edcMeldung, string i_strCultureName);
	}
}
