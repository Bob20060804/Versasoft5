using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(double), typeof(Rect))]
	public class EDC_BreiteNachRectConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				return new Rect((double)value, 0.0, 0.0, 0.0);
			}
			return default(Rect);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
