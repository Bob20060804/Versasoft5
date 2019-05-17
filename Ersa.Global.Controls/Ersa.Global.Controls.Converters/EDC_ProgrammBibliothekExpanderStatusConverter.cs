using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_ProgrammBibliothekExpanderStatusConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			CollectionViewGroup collectionViewGroup = values[0] as CollectionViewGroup;
			List<string> list = values[1] as List<string>;
			bool flag;
			if (collectionViewGroup != null && list != null)
			{
				if (!list.Any())
				{
					return false;
				}
				string item = collectionViewGroup.Name.ToString();
				flag = list.Contains(item);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[2];
		}
	}
}
