using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	[ValueConversion(typeof(ENUM_EditorToolType), typeof(bool))]
	public class EDC_ToolTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.GetName(typeof(ENUM_EditorToolType), value) == (string)parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new NotSupportedException(GetType().Name + "Rücktransformation wird nicht unterstützt");
		}
	}
}
