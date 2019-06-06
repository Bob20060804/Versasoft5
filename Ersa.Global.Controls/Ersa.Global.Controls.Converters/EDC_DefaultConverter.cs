using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_DefaultConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return i_objValue;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return i_objValue;
		}
	}
}
