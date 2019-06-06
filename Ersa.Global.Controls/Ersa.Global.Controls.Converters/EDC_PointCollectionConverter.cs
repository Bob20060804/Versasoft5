using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_PointCollectionConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			IEnumerable<Point> enumerable = i_objValue as IEnumerable<Point>;
			if (enumerable == null)
			{
				return null;
			}
			return new PointCollection(enumerable);
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return Binding.DoNothing;
		}
	}
}
