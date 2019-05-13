using Ersa.Global.Common.Helper;
using Ersa.Platform.Lokalisierung.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Ersa.Platform.Lokalisierung
{
	[Export(typeof(INF_LokalisierungsDienst))]
	public class EDC_XmlLokalisierungsDienst : INF_LokalisierungsDienst
	{
		private readonly ILocalizationProvider m_fdcLokalisierungsProvider;

		[ImportingConstructor]
		public EDC_XmlLokalisierungsDienst(ILocalizationProvider i_fdcLokalisierungsProvider)
		{
			m_fdcLokalisierungsProvider = i_fdcLokalisierungsProvider;
		}

		public string FUN_strText(string i_strKey, CultureInfo i_fdcCulture)
		{
			return m_fdcLokalisierungsProvider.GetLocalizedObject(i_strKey, null, i_fdcCulture).ToString();
		}

		public string FUN_strText(string i_strKey)
		{
			return FUN_strText(i_strKey, LocalizeDictionary.Instance.Culture);
		}

		public string FUN_strText(string i_strKey, string i_strSuffix, CultureInfo i_fdcCulture)
		{
			return FUN_strText(i_strKey, i_fdcCulture) + " " + i_strSuffix;
		}

		public string FUN_strText(string i_strKey, string i_strSuffix)
		{
			return FUN_strText(i_strKey, i_strSuffix, LocalizeDictionary.Instance.Culture);
		}

		public string FUN_strEnumDescriptionText<T>(T i_objEnumValue) where T : struct, IConvertible
		{
			string i_strKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_objEnumValue);
			return FUN_strText(i_strKey);
		}

		public string FUN_strEnumDescriptionText<T>(T i_objEnumValue, CultureInfo i_fdcCulture) where T : struct, IConvertible
		{
			string i_strKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_objEnumValue);
			return FUN_strText(i_strKey, i_fdcCulture);
		}

		public CultureInfo FUN_fdcAktuelleCulture()
		{
			return LocalizeDictionary.Instance.Culture;
		}
	}
}
