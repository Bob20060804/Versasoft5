using System.Collections.Generic;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_AnwendungsBildschirmeDienst
	{
		IEnumerable<int> FUN_enuAnwendungsBildschirmeErmitteln();

		void SUB_AnwendungsBildchirmeFestlegen(IEnumerable<int> i_enuBildschirme);
	}
}
