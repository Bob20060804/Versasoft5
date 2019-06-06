using System.Windows;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls
{
	public class EDU_IconToggleButton : ToggleButton
	{
		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_fdcEckRadienProperty;

		public static readonly DependencyProperty PRO_strCheckedIconUriProperty;

		public static readonly DependencyProperty PRO_i32IconHoeheUndBreiteProperty;

		private const int mC_i32IconHoeheUndBreiteDefault = 24;

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

		public int PRO_i32IconHoeheUndBreite
		{
			get
			{
				return (int)GetValue(PRO_i32IconHoeheUndBreiteProperty);
			}
			set
			{
				SetValue(PRO_i32IconHoeheUndBreiteProperty, value);
			}
		}

		static EDU_IconToggleButton()
		{
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_IconToggleButton));
			PRO_fdcEckRadienProperty = DependencyProperty.Register("PRO_fdcEckRadien", typeof(CornerRadius), typeof(EDU_IconToggleButton));
			PRO_strCheckedIconUriProperty = DependencyProperty.Register("PRO_strCheckedIconUri", typeof(string), typeof(EDU_IconToggleButton));
			PRO_i32IconHoeheUndBreiteProperty = DependencyProperty.Register("PRO_i32IconHoeheUndBreiteProperty", typeof(int), typeof(EDU_IconToggleButton), new PropertyMetadata(24));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_IconToggleButton), new FrameworkPropertyMetadata(typeof(EDU_IconToggleButton)));
		}
	}
}
