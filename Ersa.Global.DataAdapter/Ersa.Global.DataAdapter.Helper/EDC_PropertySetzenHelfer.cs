using Ersa.Global.Common.Helper;
using Ersa.Global.DataProvider.Factories.StrategieFactory;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_PropertySetzenHelfer
	{
		public static void SUB_LeseReaderInProperty<T>(this T i_edcPparent, DbDataReader i_fdcReader, PropertyInfo i_fdcPropertyInfo, string i_strSpalte, object i_objDefaultWert)
		{
			int ordinal = i_fdcReader.GetOrdinal(i_strSpalte);
			object value = FUN_objHoleReaderWert(i_fdcReader, i_fdcPropertyInfo, ordinal) ?? i_objDefaultWert;
			i_fdcPropertyInfo.SetValue(i_edcPparent, value);
		}

		public static object FUN_objHoleReaderWert(DbDataReader i_fdcReader, PropertyInfo i_fdcPropertyInfo, int i_i16Splate, string i_strDatentyp = "")
		{
			if (i_fdcReader[i_i16Splate] == DBNull.Value)
			{
				return null;
			}
			string text = (i_fdcPropertyInfo == null) ? i_strDatentyp : FUN_strBestimmeDatentypAusPropertyInfo(i_fdcPropertyInfo);
			switch (text)
			{
			case "String":
				return i_fdcReader.GetString(i_i16Splate);
			case "Int64":
				return i_fdcReader.GetInt64(i_i16Splate);
			case "DateTime":
				return i_fdcReader.GetDateTime(i_i16Splate);
			case "Boolean":
				return i_fdcReader.GetBoolean(i_i16Splate);
			case "Bitmap":
				return EDC_BildConverterHelfer.FUN_fdcByteArrayToImage((byte[])i_fdcReader[i_i16Splate]);
			case "Byte[]":
				return i_fdcReader[i_i16Splate];
			case "Int32":
				return i_fdcReader.GetInt32(i_i16Splate);
			case "Int16":
				return i_fdcReader.GetInt16(i_i16Splate);
			case "Single":
				return i_fdcReader.GetFloat(i_i16Splate);
			case "Object":
				return i_fdcReader.GetValue(i_i16Splate);
			default:
				throw new ArgumentOutOfRangeException("strDatentyp", $"The datatype '{text}' is invalid or not supported.");
			}
		}

		public static void SUB_LeseDataRowInProperty<T>(this T i_edcParent, DataRow i_fdcZeile, PropertyInfo i_fdcPropertyInfo, string i_strSpalte)
		{
			if (!i_fdcZeile.Table.Columns.Contains(i_strSpalte))
			{
				return;
			}
			if (i_fdcZeile.Table.Columns[i_strSpalte].DataType == typeof(byte[]))
			{
				i_fdcPropertyInfo.SetValue(i_edcParent, FUN_objKonvertiereDatentyp(i_fdcPropertyInfo, i_fdcZeile[i_strSpalte]));
				return;
			}
			string i_strWert = Convert.ToString(i_fdcZeile[i_strSpalte]);
			object obj = FUN_objKonvertiereStringDatentyp(i_fdcPropertyInfo, i_strWert);
			if (obj != null)
			{
				i_fdcPropertyInfo.SetValue(i_edcParent, obj);
			}
		}

		public static object FUN_objKonvertiereStringDatentyp(PropertyInfo i_fdcPropertyInfo, string i_strWert)
		{
			string text = FUN_strBestimmeDatentypAusPropertyInfo(i_fdcPropertyInfo);
			switch (text)
			{
			case "String":
				return i_strWert;
			case "Int64":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.ToInt64(i_strWert);
				}
				return null;
			case "DateTime":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.ToDateTime(i_strWert);
				}
				return null;
			case "Boolean":
				return Convert.ToBoolean(i_strWert);
			case "Bool":
				return Convert.ToBoolean(i_strWert);
			case "Bitmap":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return EDC_BildConverterHelfer.FUN_fdcByteArrayToImage(Convert.FromBase64String(i_strWert));
				}
				return null;
			case "Byte[]":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.FromBase64String(i_strWert);
				}
				return null;
			case "Byte":
				return Convert.ToByte(i_strWert);
			case "Int32":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.ToInt32(i_strWert);
				}
				return null;
			case "Int16":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.ToInt16(i_strWert);
				}
				return null;
			case "Single":
				if (!string.IsNullOrEmpty(i_strWert))
				{
					return Convert.ToSingle(i_strWert);
				}
				return null;
			default:
				throw new ArgumentOutOfRangeException("strDatentyp", $"The datatype '{text}' is invalid or not supported.");
			}
		}

		public static object FUN_objKonvertiereDatentyp(PropertyInfo i_fdcPropertyInfo, object i_objWert, string i_strDatentyp = "")
		{
			string text = (i_fdcPropertyInfo == null) ? i_strDatentyp : FUN_strBestimmeDatentypAusPropertyInfo(i_fdcPropertyInfo);
			switch (text)
			{
			case "String":
				if (i_objWert != DBNull.Value)
				{
					return i_objWert;
				}
				return string.Empty;
			case "Int64":
				return (i_objWert == DBNull.Value) ? 0 : ((long)i_objWert);
			case "DateTime":
				if (i_objWert != DBNull.Value)
				{
					return (DateTime)i_objWert;
				}
				return null;
			case "Boolean":
				return i_objWert != DBNull.Value && (bool)i_objWert;
			case "Bool":
				return i_objWert != DBNull.Value && (bool)i_objWert;
			case "Bitmap":
				if (i_objWert != DBNull.Value)
				{
					return EDC_BildConverterHelfer.FUN_fdcByteArrayToImage((byte[])i_objWert);
				}
				return DBNull.Value;
			case "Byte[]":
				if (i_objWert != DBNull.Value)
				{
					return i_objWert;
				}
				return null;
			case "Byte":
				if (i_objWert != DBNull.Value)
				{
					return i_objWert;
				}
				return null;
			case "Int32":
				return (i_objWert != DBNull.Value) ? ((int)i_objWert) : 0;
			case "Int16":
				return (i_objWert != DBNull.Value) ? ((short)i_objWert) : 0;
			case "Single":
				return (i_objWert == DBNull.Value) ? 0f : ((float)i_objWert);
			default:
				throw new ArgumentOutOfRangeException("strDatentyp", $"The datatype '{text}' is invalid or not supported.");
			}
		}

		public static void SUB_SetzteCommandParameter<T>(this T i_edcParent, DbCommand i_fdcCommand, PropertyInfo i_fdcPropertyInfo, string i_strSpalte)
		{
			SUB_SetzteCommandParameter(i_fdcCommand, i_fdcPropertyInfo, i_strSpalte, i_fdcPropertyInfo.GetValue(i_edcParent));
		}

		public static void SUB_SetzteCommandParameter(DbCommand i_fdcCommand, PropertyInfo i_fdcPropertyInfo, string i_strSpalte, object i_objValue, string i_strDatentyp = "")
		{
			if (i_fdcPropertyInfo != null && i_fdcPropertyInfo.PropertyType.IsGenericType && i_fdcPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (Nullable.GetUnderlyingType(i_fdcPropertyInfo.PropertyType) != null)
				{
					DbParameter dbParameter = i_fdcCommand.CreateParameter();
					dbParameter.DbType = DbType.DateTime;
					dbParameter.ParameterName = $"@{i_strSpalte}";
					if (i_objValue == null || (DateTime)i_objValue == DateTime.MinValue)
					{
						dbParameter.Value = DBNull.Value;
					}
					else
					{
						dbParameter.Value = i_objValue;
					}
					i_fdcCommand.Parameters.Add(dbParameter);
				}
				return;
			}
			string text = (i_fdcPropertyInfo == null) ? i_strDatentyp : FUN_strBestimmeDatentypAusPropertyInfo(i_fdcPropertyInfo);
			DbParameter dbParameter2 = EDC_DatenbankDialektFactory.FUN_edcHoleDatenbankDialekt(i_fdcCommand).FUN_edcErstelleDbParameterMitDatentyp(i_fdcCommand, text);
			dbParameter2.ParameterName = $"@{i_strSpalte}";
			switch (text)
			{
			case "String":
				dbParameter2.Value = (i_objValue ?? DBNull.Value);
				break;
			case "Int64":
				dbParameter2.Value = Convert.ToInt64(i_objValue ?? ((object)0));
				break;
			case "Single":
				dbParameter2.Value = Convert.ToSingle(i_objValue ?? ((object)0));
				break;
			case "DateTime":
				if (i_objValue == null)
				{
					dbParameter2.Value = DBNull.Value;
				}
				else
				{
					dbParameter2.Value = Convert.ToDateTime(i_objValue);
				}
				break;
			case "Boolean":
			{
				bool result;
				if (i_objValue is bool)
				{
					dbParameter2.Value = Convert.ToBoolean(i_objValue);
				}
				else if (bool.TryParse(i_objValue as string, out result))
				{
					dbParameter2.Value = result;
				}
				else
				{
					dbParameter2.Value = DBNull.Value;
				}
				break;
			}
			case "Bitmap":
				if (i_objValue == null)
				{
					dbParameter2.Value = DBNull.Value;
				}
				else
				{
					dbParameter2.Value = EDC_BildConverterHelfer.FUNa_bytImageToByteArray((Bitmap)i_objValue);
				}
				break;
			case "Byte[]":
				dbParameter2.Value = (i_objValue ?? DBNull.Value);
				break;
			case "Int32":
				dbParameter2.Value = Convert.ToInt32(i_objValue ?? ((object)0));
				break;
			case "Int16":
				dbParameter2.Value = Convert.ToInt16(i_objValue ?? ((object)0));
				break;
			case "Byte":
				if (i_objValue == null)
				{
					dbParameter2.Value = DBNull.Value;
				}
				else
				{
					dbParameter2.Value = Convert.ToByte(i_objValue);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("strDatentyp", $"The datatype '{text}' is invalid or not supported.");
			}
			i_fdcCommand.Parameters.Add(dbParameter2);
		}

		private static string FUN_strBestimmeDatentypAusPropertyInfo(PropertyInfo i_fdcPropertyInfo)
		{
			string text = (!(i_fdcPropertyInfo != null) || !i_fdcPropertyInfo.PropertyType.IsGenericType || !(i_fdcPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))) ? i_fdcPropertyInfo.PropertyType.Name : Nullable.GetUnderlyingType(i_fdcPropertyInfo.PropertyType).Name;
			return text.Replace("System.", string.Empty);
		}
	}
}
