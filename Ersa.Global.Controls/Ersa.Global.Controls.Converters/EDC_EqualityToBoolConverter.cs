using Ersa.Global.Controls.MarkupExtensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(bool))]
	public class EDC_EqualityToBoolConverter : EDC_ValueConverterMarkupExtensionBase, IValueConverter
	{
		public object PRO_objDefaultValue
		{
			get;
			set;
		}

		public object PRO_objObjectToCompareTo
		{
			get;
			set;
		}

		public bool PRO_blnInvertiert
		{
			get;
			set;
		}

		public EDC_EqualityToBoolConverter()
		{
		}

		public EDC_EqualityToBoolConverter(object i_objObjectToCompareTo)
		{
			PRO_objObjectToCompareTo = i_objObjectToCompareTo;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return object.Equals(value, PRO_objObjectToCompareTo) ^ PRO_blnInvertiert;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && (bool)value)
			{
				return PRO_objObjectToCompareTo;
			}
			return PRO_objDefaultValue;
		}
	}
}
