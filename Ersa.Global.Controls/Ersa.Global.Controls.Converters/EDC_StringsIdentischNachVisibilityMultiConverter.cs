using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_StringsIdentischNachVisibilityMultiConverter : IMultiValueConverter
	{
		public bool PRO_blnStringDarfNichtLeerSein
		{
			get;
			set;
		}

		public bool PRO_blnInvertiert
		{
			get;
			set;
		}

		public object Convert(object[] ia_objValues, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			List<string> list = ia_objValues?.OfType<string>().ToList();
			if (list != null && list.Any())
			{
				bool flag = list.Distinct().Count() == 1;
				return (!(((!PRO_blnStringDarfNichtLeerSein) ? flag : (flag && !string.IsNullOrEmpty(list.First()))) ^ PRO_blnInvertiert)) ? Visibility.Collapsed : Visibility.Visible;
			}
			return Visibility.Visible;
		}

		public object[] ConvertBack(object i_objValues, Type[] ia_fdcTargetTypes, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
