using Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Zeitschaltuhr;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.AllgemeineEinstellungen.Converters
{
	[ValueConversion(typeof(ENUM_WochenuhrZeileZustaende), typeof(bool))]
	public class EDC_WochenuhrZeileZustandNachBoolConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			switch ((ENUM_WochenuhrZeileZustaende)i_objValue)
			{
			case ENUM_WochenuhrZeileZustaende.enmEin:
				return true;
			case ENUM_WochenuhrZeileZustaende.enmAus:
				return false;
			default:
				return false;
			}
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (!(i_objValue is bool))
			{
				throw new InvalidOperationException("Value is not of type bool");
			}
			return ((bool)i_objValue) ? ENUM_WochenuhrZeileZustaende.enmEin : ENUM_WochenuhrZeileZustaende.enmAus;
		}
	}
}
