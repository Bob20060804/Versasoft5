using Ersa.Global.Common.Helper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_DoubleNachTextConverter : IMultiValueConverter
	{
		public object Convert(object[] ia_objValues, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (ia_objValues == null)
			{
				return null;
			}
			if (ia_objValues.Length < 1)
			{
				return null;
			}
			double i_dblWert = 0.0;
			int i_i32AnzahlNachkommaZahlen = 0;
			if (ia_objValues.Length != 0)
			{
				if (ia_objValues[0] is double)
				{
					i_dblWert = (double)ia_objValues[0];
				}
				if (ia_objValues[0] is float)
				{
					i_dblWert = (float)ia_objValues[0];
				}
				if (ia_objValues[0] is int)
				{
					i_dblWert = (int)ia_objValues[0];
				}
			}
			if (ia_objValues.Length > 1 && ia_objValues[1] is int)
			{
				i_i32AnzahlNachkommaZahlen = (int)ia_objValues[1];
			}
			return EDC_ZahlenFormatHelfer.FUN_strWert(i_dblWert, i_i32AnzahlNachkommaZahlen);
		}

		public object[] ConvertBack(object i_objValue, Type[] ia_fdcTargetTypes, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
