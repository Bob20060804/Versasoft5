using Ersa.Global.Common.Helper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_FloatNachTextConverter : IValueConverter
	{
		public int PRO_i32AnzahlNachkommaStellen
		{
			get;
			set;
		}

		public virtual object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			double i_dblWert = 0.0;
			if (i_objValue is float)
			{
				i_dblWert = (float)i_objValue;
			}
			if (i_objValue is double)
			{
				i_dblWert = (double)i_objValue;
			}
			return EDC_ZahlenFormatHelfer.FUN_strWert(i_dblWert, PRO_i32AnzahlNachkommaStellen);
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (float.TryParse(i_objValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
			{
				return result;
			}
			return i_objValue;
		}
	}
}
