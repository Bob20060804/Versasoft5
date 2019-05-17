using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(int))]
	public class EDC_SubtraktionsConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			int? num = FUN_i32KonvertiereNachInt(i_objValue);
			int? num2 = FUN_i32KonvertiereNachInt(i_objParameter);
			if (num.HasValue && num2.HasValue)
			{
				return num.Value - num2.Value;
			}
			return 0;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}

		private int? FUN_i32KonvertiereNachInt(object i_objWert)
		{
			double? num = null;
			double result;
			if (i_objWert is double)
			{
				num = (double)i_objWert;
			}
			else if (i_objWert is float)
			{
				num = (float)i_objWert;
			}
			else if (i_objWert is Enum)
			{
				num = (int)i_objWert;
			}
			else if (i_objWert is int)
			{
				num = (int)i_objWert;
			}
			else if (i_objWert is string && double.TryParse(i_objWert.ToString(), out result))
			{
				num = result;
			}
			return (int?)num;
		}
	}
}
