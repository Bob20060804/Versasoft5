using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektSelector<T> : INF_SqlStatementErsteller<T>
	{
		public string FUN_strErstelleSqlStatement(T i_edcItem)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			EDC_TabellenInformation eDC_TabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_edcItem);
			string text = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleWhereStatement(i_edcItem);
			if (eDC_TabellenInformation.PRO_blnIsQuery)
			{
				return $"{i_edcItem.FUN_strHoleDatenbankAbfrage(eDC_TabellenInformation)} {text}";
			}
			return i_edcItem.FUN_strErstelleSelectStatement(i_dicMapping, i_edcItem.FUN_strHoleTabellenName(), text);
		}

		public void SUB_ErstelleSqlStatement(T i_edcItem, DbCommand i_fdcCommand)
		{
			i_fdcCommand.CommandText = FUN_strErstelleSqlStatement(i_edcItem);
		}
	}
}
