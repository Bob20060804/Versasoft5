using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(double))]
	public class EDC_MultiplikationsConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			double? num = FUN_dblKonvertiereNachDouble(i_objValue);
			double? num2 = FUN_dblKonvertiereNachDouble(i_objParameter);
			if (num.HasValue && num2.HasValue)
			{
				return num.Value * num2.Value;
			}
			return 0;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}

		private double? FUN_dblKonvertiereNachDouble(object i_objWert)
		{
			double? result = null;
			if (i_objWert is double)
			{
				result = (double)i_objWert;
			}
			else if (i_objWert is float)
			{
				result = (float)i_objWert;
			}
			else if (i_objWert is int)
			{
				result = (int)i_objWert;
			}
			return result;
		}
	}
}
