using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_ToggleButton : EDU_IconButton
	{
		public static readonly DependencyProperty PRO_strIconAktivUriProperty;

		public static readonly DependencyProperty PRO_blnIstAktivProperty;

		public static readonly DependencyProperty PRO_blnIstTeilablaufAktivProperty;

		public static readonly DependencyProperty PRO_blnIstGesamtablaufAktivProperty;

		public static readonly DependencyProperty PRO_blnInfoTextAnzeigenProperty;

		public string PRO_strIconAktivUri
		{
			get
			{
				return (string)GetValue(PRO_strIconAktivUriProperty);
			}
			set
			{
				SetValue(PRO_strIconAktivUriProperty, value);
			}
		}

		public bool PRO_blnIstAktiv
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAktivProperty);
			}
			set
			{
				SetValue(PRO_blnIstAktivProperty, value);
			}
		}

		public bool PRO_blnTeilablaufAktiv
		{
			get
			{
				return (bool)GetValue(PRO_blnIstTeilablaufAktivProperty);
			}
			set
			{
				SetValue(PRO_blnIstTeilablaufAktivProperty, value);
			}
		}

		public bool PRO_blnGesamtablaufAktiv
		{
			get
			{
				return (bool)GetValue(PRO_blnIstGesamtablaufAktivProperty);
			}
			set
			{
				SetValue(PRO_blnIstGesamtablaufAktivProperty, value);
			}
		}

		public bool PRO_blnInfoTextAnzeigen
		{
			get
			{
				return (bool)GetValue(PRO_blnInfoTextAnzeigenProperty);
			}
			set
			{
				SetValue(PRO_blnInfoTextAnzeigenProperty, value);
			}
		}

		public ICommand PRO_cmdOrginalCommand
		{
			get;
			set;
		}

		public Binding PRO_fdcOrginalCommandParameter
		{
			get;
			set;
		}

		static EDU_ToggleButton()
		{
			PRO_strIconAktivUriProperty = DependencyProperty.Register("PRO_strIconAktivUri", typeof(string), typeof(EDU_ToggleButton));
			PRO_blnIstAktivProperty = DependencyProperty.Register("PRO_blnIstAktiv", typeof(bool), typeof(EDU_ToggleButton));
			PRO_blnIstTeilablaufAktivProperty = DependencyProperty.Register("PRO_blnTeilablaufAktiv", typeof(bool), typeof(EDU_ToggleButton));
			PRO_blnIstGesamtablaufAktivProperty = DependencyProperty.Register("PRO_blnGesamtablaufAktiv", typeof(bool), typeof(EDU_ToggleButton));
			PRO_blnInfoTextAnzeigenProperty = DependencyProperty.Register("PRO_blnInfoTextAnzeigen", typeof(bool), typeof(EDU_ToggleButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ToggleButton), new FrameworkPropertyMetadata(typeof(EDU_ToggleButton)));
		}
	}
}
