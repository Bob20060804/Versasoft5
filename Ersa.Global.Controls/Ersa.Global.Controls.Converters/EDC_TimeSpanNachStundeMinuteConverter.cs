using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(TimeSpan), typeof(string))]
	public class EDC_TimeSpanNachStundeMinuteConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (!(i_objValue is TimeSpan))
			{
				return DependencyProperty.UnsetValue;
			}
			TimeSpan timeSpan = (TimeSpan)i_objValue;
			return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:00}";
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
