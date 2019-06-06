using System.Windows;

namespace Ersa.Global.Controls
{
	public class EDU_BenutzerComboBox : EDU_IconComboBox
	{
		public static readonly DependencyProperty PRO_blnIstBenutzerAngemeldetProperty;

		public static readonly DependencyProperty PRO_strBenutzernameProperty;

		public static readonly DependencyProperty PRO_strBenutzerrolleProperty;

		public static readonly DependencyProperty PRO_strAnmeldenTextProperty;

		public bool PRO_blnIstBenutzerAngemeldet
		{
			get
			{
				return (bool)GetValue(PRO_blnIstBenutzerAngemeldetProperty);
			}
			set
			{
				SetValue(PRO_blnIstBenutzerAngemeldetProperty, value);
			}
		}

		public string PRO_strBenutzername
		{
			get
			{
				return (string)GetValue(PRO_strBenutzernameProperty);
			}
			set
			{
				SetValue(PRO_strBenutzernameProperty, value);
			}
		}

		public string PRO_strBenutzerrolle
		{
			get
			{
				return (string)GetValue(PRO_strBenutzerrolleProperty);
			}
			set
			{
				SetValue(PRO_strBenutzerrolleProperty, value);
			}
		}

		static EDU_BenutzerComboBox()
		{
			PRO_blnIstBenutzerAngemeldetProperty = DependencyProperty.Register("PRO_blnIstBenutzerAngemeldet", typeof(bool), typeof(EDU_BenutzerComboBox));
			PRO_strBenutzernameProperty = DependencyProperty.Register("PRO_strBenutzername", typeof(string), typeof(EDU_BenutzerComboBox));
			PRO_strBenutzerrolleProperty = DependencyProperty.Register("PRO_strBenutzerrolle", typeof(string), typeof(EDU_BenutzerComboBox));
			PRO_strAnmeldenTextProperty = DependencyProperty.Register("PRO_strAnmeldenText", typeof(string), typeof(EDU_BenutzerComboBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_BenutzerComboBox), new FrameworkPropertyMetadata(typeof(EDU_BenutzerComboBox)));
		}
	}
}
