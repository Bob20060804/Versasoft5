using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(double), typeof(Thickness))]
	public class EDC_DoubleNachThicknessConverter : IValueConverter
	{
		public bool PRO_blnFuerTopVerwenden
		{
			get;
			set;
		}

		public bool PRO_blnFuerBottomVerwenden
		{
			get;
			set;
		}

		public bool PRO_blnFuerLeftVerwenden
		{
			get;
			set;
		}

		public bool PRO_blnFuerRightVerwenden
		{
			get;
			set;
		}

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (!(i_objValue is double))
			{
				return Binding.DoNothing;
			}
			double num = (double)i_objValue;
			Thickness thickness = default(Thickness);
			thickness.Top = (PRO_blnFuerTopVerwenden ? num : 0.0);
			thickness.Bottom = (PRO_blnFuerBottomVerwenden ? num : 0.0);
			thickness.Left = (PRO_blnFuerLeftVerwenden ? num : 0.0);
			thickness.Right = (PRO_blnFuerRightVerwenden ? num : 0.0);
			return thickness;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return Binding.DoNothing;
		}
	}
}
