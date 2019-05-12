using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Konfiguration;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesProtokollMapper
	{
		ENUM_MesProtokollFormat PRO_enuFormat
		{
			get;
		}

		string FUN_strMap(ENUM_MesFunktionen i_enuFunktion, Dictionary<ENUM_MesMaschinenDatenArgumente, object> i_dicArgumente);
	}
}
