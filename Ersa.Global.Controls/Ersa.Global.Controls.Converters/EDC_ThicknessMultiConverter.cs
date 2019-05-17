using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_ThicknessMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] ia_objWerte, Type i_fdcZielTyp, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return new Thickness(System.Convert.ToDouble(ia_objWerte[0]), System.Convert.ToDouble(ia_objWerte[1]), System.Convert.ToDouble(ia_objWerte[2]), System.Convert.ToDouble(ia_objWerte[3]));
		}

		public object[] ConvertBack(object i_objWert, Type[] ia_fdcZielTypen, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
