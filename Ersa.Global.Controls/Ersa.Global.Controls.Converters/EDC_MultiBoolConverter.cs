using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_MultiBoolConverter : IMultiValueConverter
	{
		public bool PRO_blnWerteVerodern
		{
			get;
			set;
		}

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

		public bool PRO_blnIgnoriereAndereTypen
		{
			get;
			set;
		}

		public object Convert(object[] ia_objWerte, Type i_fdcZielTyp, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (!PRO_blnIgnoriereAndereTypen && !ia_objWerte.All((object i_objWert) => i_objWert is bool))
			{
				if (PRO_blnVisibilityStattBool)
				{
					return Visibility.Collapsed;
				}
				return false;
			}
			bool flag = PRO_blnWerteVerodern ? ia_objWerte.OfType<bool>().Any((bool i_objWert) => i_objWert) : ia_objWerte.OfType<bool>().All((bool i_objWert) => i_objWert);
			if (PRO_blnVisibilityStattBool)
			{
				return (!(flag ^ PRO_blnInvertiert)) ? Visibility.Collapsed : Visibility.Visible;
			}
			return flag ^ PRO_blnInvertiert;
		}

		public object[] ConvertBack(object i_objWert, Type[] ia_fdcZielTypen, object i_objParameter, CultureInfo i_fdcCulture)
		{
			IList<object> list = new List<object>();
			for (int i = 0; i < ia_fdcZielTypen.Length; i++)
			{
				list.Add(DependencyProperty.UnsetValue);
			}
			return list.ToArray();
		}
	}
}
