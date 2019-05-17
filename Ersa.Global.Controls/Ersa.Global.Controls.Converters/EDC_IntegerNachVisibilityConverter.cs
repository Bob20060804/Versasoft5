using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(int), typeof(Visibility))]
	public class EDC_IntegerNachVisibilityConverter : EDC_IntegerNachBoolConverter
	{
		public override object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return (!(bool)base.Convert(i_objValue, i_fdcTargetType, i_objParameter, i_fdcCulture)) ? Visibility.Collapsed : Visibility.Visible;
		}

		public override object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			bool flag = (Visibility)i_objValue == Visibility.Visible;
			return base.ConvertBack(flag, i_fdcTargetType, i_objParameter, i_fdcCulture);
		}
	}
}
