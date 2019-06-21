using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektAusReader<T> : INF_ObjektAusReader<T>
	{
		public T FUN_edcLeseObjektAusReader(DbDataReader i_fdcReader)
		{
			Type typeFromHandle = typeof(T);
			T val = (Activator.CreateInstance(typeFromHandle) is T) ? ((T)Activator.CreateInstance(typeFromHandle)) : default(T);
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(val);
			List<EDC_DynamischeSpalte> list = null;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item2 in dictionary)
			{
				if (!item2.Value.PRO_blnIsDynamischeSpalte)
				{
					val.SUB_LeseReaderInProperty(i_fdcReader, item2.Key, item2.Value.PRO_strName, item2.Value.PRO_objDefaultWert);
				}
				else
				{
					list = new List<EDC_DynamischeSpalte>();
					item2.Key.SetValue(val, list);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < i_fdcReader.FieldCount; i++)
				{
					string strSpalte = i_fdcReader.GetName(i).ToLower();
					Type fieldType = i_fdcReader.GetFieldType(i);
					if (!dictionary.Any(delegate(KeyValuePair<PropertyInfo, EDC_SpaltenInformation> fdcPaar)
					{
						if (!fdcPaar.Value.PRO_blnIsDynamischeSpalte)
						{
							return fdcPaar.Value.PRO_strName.ToLower().Equals(strSpalte);
						}
						return false;
					}))
					{
						EDC_DynamischeSpalte item = new EDC_DynamischeSpalte(strSpalte, fieldType.Name, EDC_PropertySetzenHelfer.FUN_objHoleReaderWert(i_fdcReader, null, i, fieldType.Name));
						list.Add(item);
					}
				}
			}
			return val;
		}
	}
}
