using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Konfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Mes.Mapper
{
	[Export(typeof(INF_MesProtokollMapper))]
	public class EDC_ErsaJsonMapper : INF_MesProtokollMapper
	{
		public ENUM_MesProtokollFormat PRO_enuFormat => ENUM_MesProtokollFormat.Json;

		public string FUN_strMap(ENUM_MesFunktionen i_enuFunktion, Dictionary<ENUM_MesMaschinenDatenArgumente, object> i_dicArgumente)
		{
			throw new NotImplementedException();
		}
	}
}
