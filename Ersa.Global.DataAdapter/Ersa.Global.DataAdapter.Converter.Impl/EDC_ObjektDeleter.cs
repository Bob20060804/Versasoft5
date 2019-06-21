using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System;
using System.Data.Common;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektDeleter<T> : INF_SqlStatementErsteller<T>
	{
		public string FUN_strErstelleSqlStatement(T i_edcItem)
		{
			string text = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleWhereStatement(i_edcItem);
			if (string.IsNullOrEmpty(text))
			{
				text = EDC_SqlErstellungsHelfer.FUN_strErstelleWhereStatementFuerPrimaryKeys(i_edcItem, text);
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentOutOfRangeException("i_edcItem", "Using EDC_ObjektDeleter without 'where-statement' is dangerous. Use truncate table instead");
				}
			}
			return EDC_SqlErstellungsHelfer.FUN_strErstelleDeleteStatement(i_edcItem.FUN_strHoleTabellenName(), text);
		}

		public void SUB_ErstelleSqlStatement(T i_edcItem, DbCommand i_fdcCommand)
		{
			i_fdcCommand.CommandText = FUN_strErstelleSqlStatement(i_edcItem);
		}
	}
}
