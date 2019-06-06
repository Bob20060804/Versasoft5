using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class EDC_ObjectsNachSindObjectsNullNachVisibilityMultiConverter : IMultiValueConverter
	{
		public bool PRO_blnIstInvertiert
		{
			get;
			set;
		}

		public bool PRO_blnPruefeAufAlleGleich
		{
			get;
			set;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (PRO_blnPruefeAufAlleGleich)
			{
				return (!((values?.All((object l_objValue) => l_objValue == null) ?? true) ^ PRO_blnIstInvertiert)) ? Visibility.Collapsed : Visibility.Visible;
			}
			return (!((values?.Any((object l_objValue) => l_objValue == null) ?? true) ^ PRO_blnIstInvertiert)) ? Visibility.Collapsed : Visibility.Visible;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
