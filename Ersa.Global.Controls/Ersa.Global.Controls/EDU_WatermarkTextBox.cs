using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_WatermarkTextBox : TextBox
	{
		public static readonly DependencyProperty PRO_strWatermarkProperty;

		public string PRO_strWatermark
		{
			get
			{
				return (string)GetValue(PRO_strWatermarkProperty);
			}
			set
			{
				SetValue(PRO_strWatermarkProperty, value);
			}
		}

		static EDU_WatermarkTextBox()
		{
			PRO_strWatermarkProperty = DependencyProperty.Register("PRO_strWatermark", typeof(string), typeof(EDU_WatermarkTextBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_WatermarkTextBox), new FrameworkPropertyMetadata(typeof(EDU_WatermarkTextBox)));
		}
	}
}
