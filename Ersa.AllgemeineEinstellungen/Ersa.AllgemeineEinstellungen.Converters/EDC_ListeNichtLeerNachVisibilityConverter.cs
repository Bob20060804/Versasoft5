using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.AllgemeineEinstellungen.Converters
{
	public class EDC_ListeNichtLeerNachVisibilityConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			IEnumerable enumerable = i_objValue as IEnumerable;
			return (enumerable == null) ? Visibility.Collapsed : ((!enumerable.Cast<object>().Any()) ? Visibility.Collapsed : Visibility.Visible);
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return Binding.DoNothing;
		}
	}
}
