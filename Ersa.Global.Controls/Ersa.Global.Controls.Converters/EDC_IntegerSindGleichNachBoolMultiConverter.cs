using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_IntegerSindGleichNachBoolMultiConverter : IMultiValueConverter
	{
		public bool PRO_blnInvertiert
		{
			get;
			set;
		}

		public bool PRO_blnVisibilityStattBool
		{
			get;
			set;
		}

		public object Convert(object[] ia_objWerte, Type i_fdcZielTyp, object i_objParameter, CultureInfo i_fdcCulture)
		{
			bool flag = false;
			List<int> lstIntWerte = ia_objWerte.OfType<int>().ToList();
			if (ia_objWerte.Length == lstIntWerte.Count && lstIntWerte.All((int i_i32Wert) => i_i32Wert == lstIntWerte.First()))
			{
				flag = true;
			}
			flag ^= PRO_blnInvertiert;
			if (PRO_blnVisibilityStattBool)
			{
				return (!flag) ? Visibility.Collapsed : Visibility.Visible;
			}
			return flag;
		}

		public object[] ConvertBack(object i_objWert, Type[] ia_fdcZielTypen, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
