using System;
using System.Globalization;

namespace Ersa.Platform.Lokalisierung.Interfaces
{
	public interface INF_LokalisierungsDienst
	{
		string FUN_strText(string i_strKey, CultureInfo i_fdcCulture);

		string FUN_strText(string i_strKey);

		string FUN_strText(string i_strKey, string i_strSuffix, CultureInfo i_fdcCulture);

		string FUN_strText(string i_strKey, string i_strSuffix);

		string FUN_strEnumDescriptionText<T>(T i_objEnumValue) where T : struct, IConvertible;

		string FUN_strEnumDescriptionText<T>(T i_objEnumValue, CultureInfo i_fdcCulture) where T : struct, IConvertible;

		CultureInfo FUN_fdcAktuelleCulture();
	}
}
