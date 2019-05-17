using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_DivisionsMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] ia_objValues, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (ia_objValues == null || ia_objValues.Length != 2)
			{
				return Binding.DoNothing;
			}
			double? num = FUN_dblKonvertiereNachDouble(ia_objValues[0]);
			double? num2 = FUN_dblKonvertiereNachDouble(ia_objValues[1]);
			if (!num.HasValue || !num2.HasValue)
			{
				return Binding.DoNothing;
			}
			return num.Value / num2.Value;
		}

		public object[] ConvertBack(object i_objValue, Type[] ia_fdcTargetTypes, object i_objParameter, CultureInfo i_fdcCulture)
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
