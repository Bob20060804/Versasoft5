using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_ProxyMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] ia_objValues, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (ia_objValues.Length < 2)
			{
				return Binding.DoNothing;
			}
			object value = ia_objValues[0];
			IValueConverter valueConverter = ia_objValues[1] as IValueConverter;
			if (valueConverter == null)
			{
				return Binding.DoNothing;
			}
			return valueConverter.Convert(value, i_fdcTargetType, i_objParameter, i_fdcCulture);
		}

		public object[] ConvertBack(object i_objValue, Type[] ia_fdcTargetTypes, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
