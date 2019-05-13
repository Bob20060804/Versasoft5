using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Platform.UI.Common.Converters
{
	[ValueConversion(typeof(int), typeof(string))]
	public class EDC_IntegerToLokalisierungsKeyConverter : IValueConverter
	{
		public string PRO_strPrefix
		{
			get;
			set;
		}

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (i_objValue is int)
			{
				if (!string.IsNullOrEmpty(PRO_strPrefix))
				{
					return $"{PRO_strPrefix}_{((int)i_objValue).ToString(CultureInfo.InvariantCulture)}";
				}
				return ((int)i_objValue).ToString(CultureInfo.InvariantCulture);
			}
			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
