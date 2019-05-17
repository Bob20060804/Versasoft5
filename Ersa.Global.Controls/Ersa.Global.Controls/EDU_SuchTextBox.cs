using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_SuchTextBox : Control
	{
		public static readonly DependencyProperty PRO_strSuchTextProperty;

		public static readonly DependencyProperty PRO_strSuchTextBeschriftungProperty;

		public static readonly DependencyProperty PRO_blnIstSucheAktivProperty;

		public string PRO_strSuchText
		{
			get
			{
				return (string)GetValue(PRO_strSuchTextProperty);
			}
			set
			{
				SetValue(PRO_strSuchTextProperty, value);
			}
		}

		public string PRO_strSuchTextBeschriftung
		{
			get
			{
				return (string)GetValue(PRO_strSuchTextBeschriftungProperty);
			}
			set
			{
				SetValue(PRO_strSuchTextBeschriftungProperty, value);
			}
		}

		public bool PRO_blnIstSucheAktiv
		{
			get
			{
				return (bool)GetValue(PRO_blnIstSucheAktivProperty);
			}
			set
			{
				SetValue(PRO_blnIstSucheAktivProperty, value);
			}
		}

		static EDU_SuchTextBox()
		{
			PRO_strSuchTextProperty = DependencyProperty.Register("PRO_strSuchText", typeof(string), typeof(EDU_SuchTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			PRO_strSuchTextBeschriftungProperty = DependencyProperty.Register("PRO_strSuchTextBeschriftung", typeof(string), typeof(EDU_SuchTextBox));
			PRO_blnIstSucheAktivProperty = DependencyProperty.Register("PRO_blnIstSucheAktiv", typeof(bool), typeof(EDU_SuchTextBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_SuchTextBox), new FrameworkPropertyMetadata(typeof(EDU_SuchTextBox)));
		}
	}
}
