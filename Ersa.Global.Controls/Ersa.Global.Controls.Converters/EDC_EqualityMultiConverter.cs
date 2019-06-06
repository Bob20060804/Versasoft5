using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_EqualityMultiConverter : IMultiValueConverter
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

		public bool PRO_blnHiddenStattCollapsed
		{
			get;
			set;
		}

		public bool PRO_blnNullIgnorieren
		{
			get;
			set;
		}

		public object Convert(object[] ia_objWerte, Type i_fdcZielTyp, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (ia_objWerte.Length < 2)
			{
				return false;
			}
			object objVergleichsWert = ia_objWerte.FirstOrDefault();
			bool flag = PRO_blnNullIgnorieren ? ia_objWerte.Skip(1).All(delegate(object i_objWert)
			{
				if (i_objWert != null && objVergleichsWert != null)
				{
					return i_objWert.Equals(objVergleichsWert);
				}
				return true;
			}) : ia_objWerte.Skip(1).All((object i_objWert) => i_objWert?.Equals(objVergleichsWert) ?? (objVergleichsWert == null));
			flag ^= PRO_blnInvertiert;
			if (PRO_blnVisibilityStattBool)
			{
				return (!flag) ? (PRO_blnHiddenStattCollapsed ? Visibility.Hidden : Visibility.Collapsed) : Visibility.Visible;
			}
			return flag;
		}

		public object[] ConvertBack(object i_objWert, Type[] ia_fdcZielTypen, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return new object[0];
		}
	}
}
