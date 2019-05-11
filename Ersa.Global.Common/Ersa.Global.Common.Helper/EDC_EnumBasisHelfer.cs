using System;
using System.ComponentModel;
using System.Linq;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_EnumBasisHelfer
	{
		public static string FUN_strEnumWertBeschreibungErmitteln<T>(T i_objEnumValue) where T : struct, IConvertible
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsEnum)
			{
				throw new ArgumentException("T must be an enum type");
			}
			return FUN_strEnumWertBeschreibungErmitteln(typeFromHandle, i_objEnumValue);
		}

		public static string FUN_strEnumWertBeschreibungErmitteln(Type i_fdcEnumTyp, object i_objEnumValue)
		{
			return (i_fdcEnumTyp.GetField(i_objEnumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault() as DescriptionAttribute)?.Description ?? string.Empty;
		}
	}
}
