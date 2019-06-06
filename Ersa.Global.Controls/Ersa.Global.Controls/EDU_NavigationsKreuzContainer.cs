using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls
{
	public class EDU_NavigationsKreuzContainer : ContentControl
	{
		public static readonly DependencyProperty PRO_objContentGrossProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachLinksProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachRechtsProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachObenProperty;

		public static readonly DependencyProperty PRO_cmdNavigationNachUntenProperty;

		public static readonly DependencyProperty PRO_fdcContentGrossHintergrundProperty;

		private Popup m_fdcPopup;

		public object PRO_objContentGross
		{
			get
			{
				return GetValue(PRO_objContentGrossProperty);
			}
			set
			{
				SetValue(PRO_objContentGrossProperty, value);
			}
		}

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

		public Brush PRO_fdcContentGrossHintergrund
		{
			get
			{
				return (Brush)GetValue(PRO_fdcContentGrossHintergrundProperty);
			}
			set
			{
				SetValue(PRO_fdcContentGrossHintergrundProperty, value);
			}
		}

		static EDU_NavigationsKreuzContainer()
		{
			PRO_objContentGrossProperty = DependencyProperty.Register("PRO_objContentGross", typeof(object), typeof(EDU_NavigationsKreuzContainer));
			PRO_cmdNavigationNachLinksProperty = DependencyProperty.Register("PRO_cmdNavigationNachLinks", typeof(ICommand), typeof(EDU_NavigationsKreuzContainer));
			PRO_cmdNavigationNachRechtsProperty = DependencyProperty.Register("PRO_cmdNavigationNachRechts", typeof(ICommand), typeof(EDU_NavigationsKreuzContainer));
			PRO_cmdNavigationNachObenProperty = DependencyProperty.Register("PRO_cmdNavigationNachOben", typeof(ICommand), typeof(EDU_NavigationsKreuzContainer));
			PRO_cmdNavigationNachUntenProperty = DependencyProperty.Register("PRO_cmdNavigationNachUnten", typeof(ICommand), typeof(EDU_NavigationsKreuzContainer));
			PRO_fdcContentGrossHintergrundProperty = DependencyProperty.Register("PRO_fdcContentGrossHintergrund", typeof(Brush), typeof(EDU_NavigationsKreuzContainer));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_NavigationsKreuzContainer), new FrameworkPropertyMetadata(typeof(EDU_NavigationsKreuzContainer)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Popup popup = m_fdcPopup = (base.Template.FindName("popAnsichtGross", this) as Popup);
			if (popup != null)
			{
				popup.MouseUp += SUB_PopupGeklickt;
			}
		}

		private void SUB_PopupGeklickt(object sender, MouseButtonEventArgs e)
		{
			if (m_fdcPopup != null && !(e.Source is ButtonBase))
			{
				m_fdcPopup.IsOpen = false;
			}
		}
	}
}
