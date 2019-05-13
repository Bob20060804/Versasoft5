using Ersa.Platform.Infrastructure.Prism;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Common.Helfer;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Ersa.Platform.UI.Common.Converters
{
	[ValueConversion(typeof(object), typeof(string))]
	public class EDC_EnumBeschreibungToLokalisierungConverter : IValueConverter
	{
		private INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			string text = FUN_edcEnumMemberErmitteln(i_objValue)?.PRO_strDescription ?? string.Empty;
			string result = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				if (m_edcLokalisierungsDienst == null)
				{
					m_edcLokalisierungsDienst = EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<INF_LokalisierungsDienst>();
				}
				if (m_edcLokalisierungsDienst != null)
				{
					result = m_edcLokalisierungsDienst.FUN_strText(text);
				}
			}
			return result;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}

		private static EDC_EnumMember FUN_edcEnumMemberErmitteln(object i_objValue)
		{
			if (i_objValue == null)
			{
				return null;
			}
			Type type = i_objValue.GetType();
			Type type2 = Nullable.GetUnderlyingType(type) ?? type;
			if (!type2.IsEnum)
			{
				return null;
			}
			return EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(type2).SingleOrDefault((EDC_EnumMember i_edcEnumMember) => i_edcEnumMember.PRO_i32Value == (int)i_objValue);
		}
	}
}
