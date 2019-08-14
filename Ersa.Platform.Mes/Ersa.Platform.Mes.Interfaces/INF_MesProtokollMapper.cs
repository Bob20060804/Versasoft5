using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Konfiguration;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesProtokollMapper
	{
        /// <summary>
        /// MES Protocol format
        /// </summary>
		ENUM_MesProtokollFormat PRO_enuFormat
		{
			get;
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <param name="i_dicArgumente"></param>
        /// <returns></returns>
		string FUN_strMap(ENUM_MesFunktionen i_enuFunktion, Dictionary<ENUM_MesMaschinenDatenArgumente, object> i_dicArgumente);
	}
}
