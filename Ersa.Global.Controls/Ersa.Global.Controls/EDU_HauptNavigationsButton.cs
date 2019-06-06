using System.Windows;

namespace Ersa.Global.Controls
{
	public class EDU_HauptNavigationsButton : EDU_BeschraenkterIconButton
	{
		public static readonly DependencyProperty PRO_blnIstAusgewaehltProperty;

		public static readonly DependencyProperty PRO_strBeschraenkungsIconAusgewaehltUriProperty;

		public static readonly DependencyProperty PRO_blnIstHervorgehobenProperty;

		public bool PRO_blnIstAusgewaehlt
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAusgewaehltProperty);
			}
			set
			{
				SetValue(PRO_blnIstAusgewaehltProperty, value);
			}
		}

		public string PRO_strBeschraenkungsIconAusgewaehltUri
		{
			get
			{
				return (string)GetValue(PRO_strBeschraenkungsIconAusgewaehltUriProperty);
			}
			set
			{
				SetValue(PRO_strBeschraenkungsIconAusgewaehltUriProperty, value);
			}
		}

		public bool PRO_blnIstHervorgehoben
		{
			get
			{
				return (bool)GetValue(PRO_blnIstHervorgehobenProperty);
			}
			set
			{
				SetValue(PRO_blnIstHervorgehobenProperty, value);
			}
		}

		static EDU_HauptNavigationsButton()
		{
			PRO_blnIstAusgewaehltProperty = DependencyProperty.Register("PRO_blnIstAusgewaehlt", typeof(bool), typeof(EDU_HauptNavigationsButton));
			PRO_strBeschraenkungsIconAusgewaehltUriProperty = DependencyProperty.Register("PRO_strBeschraenkungsIconAusgewaehltUri", typeof(string), typeof(EDU_HauptNavigationsButton));
			PRO_blnIstHervorgehobenProperty = DependencyProperty.Register("PRO_blnIstHervorgehoben", typeof(bool), typeof(EDU_HauptNavigationsButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_HauptNavigationsButton), new FrameworkPropertyMetadata(typeof(EDU_HauptNavigationsButton)));
		}
	}
}
