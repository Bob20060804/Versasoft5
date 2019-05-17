using System.Windows;

namespace Ersa.Global.Controls
{
	public class EDU_BeschraenkterIconButton : EDU_IconButton
	{
		public static readonly DependencyProperty PRO_blnZugriffEingeschraenktProperty;

		public static readonly DependencyProperty PRO_strBeschraenkungsIconUriProperty;

		public bool PRO_blnZugriffEingeschraenkt
		{
			get
			{
				return (bool)GetValue(PRO_blnZugriffEingeschraenktProperty);
			}
			set
			{
				SetValue(PRO_blnZugriffEingeschraenktProperty, value);
			}
		}

		public string PRO_strBeschraenkungsIconUri
		{
			get
			{
				return (string)GetValue(PRO_strBeschraenkungsIconUriProperty);
			}
			set
			{
				SetValue(PRO_strBeschraenkungsIconUriProperty, value);
			}
		}

		static EDU_BeschraenkterIconButton()
		{
			PRO_blnZugriffEingeschraenktProperty = DependencyProperty.Register("PRO_blnZugriffEingeschraenkt", typeof(bool), typeof(EDU_BeschraenkterIconButton));
			PRO_strBeschraenkungsIconUriProperty = DependencyProperty.Register("PRO_strBeschraenkungsIconUri", typeof(string), typeof(EDU_BeschraenkterIconButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_BeschraenkterIconButton), new FrameworkPropertyMetadata(typeof(EDU_BeschraenkterIconButton)));
		}
	}
}
