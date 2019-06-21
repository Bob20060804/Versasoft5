using Ersa.Global.DataAdapter.Cache;
using System;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_TablespaceHelfer
	{
		public static string FUN_strHoleTabellenTablespace(Type i_fdcType)
		{
			return EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_fdcType).PRO_strTablespace;
		}

		public static string FUN_strHoleTabellenTablespace<T>(this T i_edcItem)
		{
			return EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_edcItem).PRO_strTablespace;
		}
	}
}
