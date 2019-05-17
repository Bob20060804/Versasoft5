using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_NavigationsKreuz : Control
	{
		public static readonly DependencyProperty PRO_cmdNavigationNachLinksProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachRechtsProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachObenProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachUntenProperty;

		public ICommand PRO_cmdNavigationNachLinks
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdNavigationNachLinksProperty);
			}
			set
			{
				SetValue(PRO_cmdNavigationNachLinksProperty, value);
			}
		}

		public ICommand PRO_cmdNavigationNachRechts
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdNavigationNachRechtsProperty);
			}
			set
			{
				SetValue(PRO_cmdNavigationNachRechtsProperty, value);
			}
		}

		public ICommand PRO_cmdNavigationNachOben
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdNavigationNachObenProperty);
			}
			set
			{
				SetValue(PRO_cmdNavigationNachObenProperty, value);
			}
		}

		public ICommand PRO_cmdNavigationNachUnten
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdNavigationNachUntenProperty);
			}
			set
			{
				SetValue(PRO_cmdNavigationNachUntenProperty, value);
			}
		}

		static EDU_NavigationsKreuz()
		{
			PRO_cmdNavigationNachLinksProperty = DependencyProperty.Register("PRO_cmdNavigationNachLinks", typeof(ICommand), typeof(EDU_NavigationsKreuz));
			PRO_cmdNavigationNachRechtsProperty = DependencyProperty.Register("PRO_cmdNavigationNachRechts", typeof(ICommand), typeof(EDU_NavigationsKreuz));
			PRO_cmdNavigationNachObenProperty = DependencyProperty.Register("PRO_cmdNavigationNachOben", typeof(ICommand), typeof(EDU_NavigationsKreuz));
			PRO_cmdNavigationNachUntenProperty = DependencyProperty.Register("PRO_cmdNavigationNachUnten", typeof(ICommand), typeof(EDU_NavigationsKreuz));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_NavigationsKreuz), new FrameworkPropertyMetadata(typeof(EDU_NavigationsKreuz)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Popup popup = base.Template.FindName("Popup", this) as Popup;
			if (popup != null)
			{
				popup.PreviewMouseDown += SUB_PopupGeklickt;
			}
		}

		private void SUB_PopupGeklickt(object sender, MouseButtonEventArgs e)
		{
			e.Handled = !(e.Source is ButtonBase);
		}
	}
}
