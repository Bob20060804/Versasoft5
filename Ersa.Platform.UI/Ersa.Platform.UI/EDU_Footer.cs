using System;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Platform.UI
{
	[Obsolete("Ersa.Platform.UI.Common.EDU_Footer verwenden")]
	public class EDU_Footer : ContentControl
	{
		public static readonly DependencyProperty PRO_blnSpeichernSichtbarProperty;

		public static readonly DependencyProperty PRO_blnSpeichernErlaubtProperty;

		public static readonly DependencyProperty PRO_blnVerwerfenSichtbarProperty;

		public static readonly DependencyProperty PRO_blnVerwerfenErlaubtProperty;

		public static readonly DependencyProperty PRO_blnVerlassenSichtbarProperty;

		public static readonly DependencyProperty PRO_blnVerlassenErlaubtProperty;

		public bool PRO_blnSpeichernSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnSpeichernSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnSpeichernSichtbarProperty, value);
			}
		}

		public bool PRO_blnSpeichernErlaubt
		{
			get
			{
				return (bool)GetValue(PRO_blnSpeichernErlaubtProperty);
			}
			set
			{
				SetValue(PRO_blnSpeichernErlaubtProperty, value);
			}
		}

		public bool PRO_blnVerwerfenSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnVerwerfenSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnVerwerfenSichtbarProperty, value);
			}
		}

		public bool PRO_blnVerwerfenErlaubt
		{
			get
			{
				return (bool)GetValue(PRO_blnVerwerfenErlaubtProperty);
			}
			set
			{
				SetValue(PRO_blnVerwerfenErlaubtProperty, value);
			}
		}

		public bool PRO_blnVerlassenSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnVerlassenSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnVerlassenSichtbarProperty, value);
			}
		}

		public bool PRO_blnVerlassenErlaubt
		{
			get
			{
				return (bool)GetValue(PRO_blnVerlassenErlaubtProperty);
			}
			set
			{
				SetValue(PRO_blnVerlassenErlaubtProperty, value);
			}
		}

		static EDU_Footer()
		{
			PRO_blnSpeichernSichtbarProperty = DependencyProperty.Register("PRO_blnSpeichernSichtbar", typeof(bool), typeof(EDU_Footer));
			PRO_blnSpeichernErlaubtProperty = DependencyProperty.Register("PRO_blnSpeichernErlaubt", typeof(bool), typeof(EDU_Footer), new PropertyMetadata(true));
			PRO_blnVerwerfenSichtbarProperty = DependencyProperty.Register("PRO_blnVerwerfenSichtbar", typeof(bool), typeof(EDU_Footer));
			PRO_blnVerwerfenErlaubtProperty = DependencyProperty.Register("PRO_blnVerwerfenErlaubt", typeof(bool), typeof(EDU_Footer), new PropertyMetadata(true));
			PRO_blnVerlassenSichtbarProperty = DependencyProperty.Register("PRO_blnVerlassenSichtbar", typeof(bool), typeof(EDU_Footer));
			PRO_blnVerlassenErlaubtProperty = DependencyProperty.Register("PRO_blnVerlassenErlaubt", typeof(bool), typeof(EDU_Footer), new PropertyMetadata(true));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_Footer), new FrameworkPropertyMetadata(typeof(EDU_Footer)));
		}
	}
}
