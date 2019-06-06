using System.Windows;

namespace Ersa.Global.Controls
{
	public class EDU_PopupToggleButton : EDU_PopupButton
	{
		public static readonly DependencyProperty PRO_strCheckedIconUriProperty = DependencyProperty.Register("PRO_strCheckedIconUri", typeof(string), typeof(EDU_PopupToggleButton));

		public static readonly DependencyProperty PRO_blnIsCheckedProperty = DependencyProperty.Register("PRO_blnIsChecked", typeof(bool), typeof(EDU_PopupToggleButton));

		public bool PRO_blnIsChecked
		{
			get
			{
				return (bool)GetValue(PRO_blnIsCheckedProperty);
			}
			set
			{
				SetValue(PRO_blnIsCheckedProperty, value);
			}
		}

		public string PRO_strCheckedIconUri
		{
			get
			{
				return (string)GetValue(PRO_strCheckedIconUriProperty);
			}
			set
			{
				SetValue(PRO_strCheckedIconUriProperty, value);
			}
		}
	}
}
