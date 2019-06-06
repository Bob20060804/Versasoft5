using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(int), typeof(bool))]
	public class EDC_IntegerNachBoolConverter : IValueConverter
	{
		public bool PRO_blnInvertiert
		{
			get;
			set;
		}

		public virtual object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if ((i_objParameter is string || i_objParameter is int) && i_objValue is int && int.TryParse(i_objParameter.ToString(), out int result))
			{
				return ((int)i_objValue == result) ^ PRO_blnInvertiert;
			}
			if (i_objParameter is Enum && (i_objValue is int || i_objValue is Enum))
			{
				result = (int)i_objParameter;
				return ((int)i_objValue == result) ^ PRO_blnInvertiert;
			}
			return false;
		}

		public virtual object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if ((i_objParameter is string || i_objParameter is int) && i_objValue is bool && int.TryParse(i_objParameter.ToString(), out int result))
			{
				if (!((bool)i_objValue ^ PRO_blnInvertiert))
				{
					return DependencyProperty.UnsetValue;
				}
				return result;
			}
			if (i_objParameter is Enum && i_objValue is bool)
			{
				if ((bool)i_objValue)
				{
					if (!PRO_blnInvertiert)
					{
						return i_objParameter;
					}
					return DependencyProperty.UnsetValue;
				}
				if (!PRO_blnInvertiert)
				{
					return DependencyProperty.UnsetValue;
				}
				return i_objParameter;
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
