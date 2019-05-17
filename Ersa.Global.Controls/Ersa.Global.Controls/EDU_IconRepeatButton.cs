using System.Windows;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls
{
	public class EDU_IconRepeatButton : RepeatButton
	{
		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_strPressedIconUriProperty;

		public static readonly DependencyProperty PRO_fdcEckRadienProperty;

		public string PRO_strIconUri
		{
			get
			{
				return (string)GetValue(PRO_strIconUriProperty);
			}
			set
			{
				SetValue(PRO_strIconUriProperty, value);
			}
		}

		public string PRO_strPressedIconUri
		{
			get
			{
				return (string)GetValue(PRO_strIconUriProperty);
			}
			set
			{
				SetValue(PRO_strIconUriProperty, value);
			}
		}

		public CornerRadius PRO_fdcEckRadien
		{
			get
			{
				return (CornerRadius)GetValue(PRO_fdcEckRadienProperty);
			}
			set
			{
				SetValue(PRO_fdcEckRadienProperty, value);
			}
		}

		static EDU_IconRepeatButton()
		{
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_IconRepeatButton));
			PRO_strPressedIconUriProperty = DependencyProperty.Register("PRO_strPressedIconUri", typeof(string), typeof(EDU_IconRepeatButton));
			PRO_fdcEckRadienProperty = DependencyProperty.Register("PRO_fdcEckRadien", typeof(CornerRadius), typeof(EDU_IconRepeatButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_IconRepeatButton), new FrameworkPropertyMetadata(typeof(EDU_IconRepeatButton)));
		}
	}
}
