using Ersa.Platform.Infrastructure.Prism;
using Ersa.Platform.Lokalisierung.Interfaces;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Platform.UI.Common.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class EDC_LokalisierungsConverter : IValueConverter
	{
		private INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		public object Convert(object i_objValue, Type i_objTargetType, object i_objParameter, CultureInfo i_objCultureInfo)
		{
			string result = null;
			string text = i_objValue as string;
			if (text != null)
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

		public object ConvertBack(object i_objValue, Type i_objTargetType, object i_objParameter, CultureInfo i_objCultureInfo)
		{
			throw new NotImplementedException();
		}
	}
}
