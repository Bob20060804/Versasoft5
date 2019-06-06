using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_PopupButton : ComboBox
	{
		public static readonly DependencyProperty PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_PopupButton), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_cmdButtonCommandProperty = DependencyProperty.Register("PRO_cmdButtonCommand", typeof(ICommand), typeof(EDU_PopupButton), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_cmdChangedCommandProperty = DependencyProperty.Register("PRO_cmdChangedCommand", typeof(ICommand), typeof(EDU_PopupButton), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_blnOeffneSelektionWennKeinItemSelektiertProperty = DependencyProperty.Register("PRO_blnOeffneSelektionWennKeinItemSelektiert", typeof(bool), typeof(EDU_PopupButton), new PropertyMetadata(false));

		public static readonly DependencyProperty PRO_blnSelektionNachAuswahlZuruecksetzenProperty = DependencyProperty.Register("PRO_blnSelektionNachAuswahlZuruecksetzen", typeof(bool), typeof(EDU_PopupButton), new PropertyMetadata(false));

		private bool m_blnSelektionBehandeln = true;

		public bool PRO_blnSelektionNachAuswahlZuruecksetzen
		{
			get
			{
				return (bool)GetValue(PRO_blnSelektionNachAuswahlZuruecksetzenProperty);
			}
			set
			{
				SetValue(PRO_blnSelektionNachAuswahlZuruecksetzenProperty, value);
			}
		}

		public bool PRO_blnOeffneSelektionWennKeinItemSelektiert
		{
			get
			{
				return (bool)GetValue(PRO_blnOeffneSelektionWennKeinItemSelektiertProperty);
			}
			set
			{
				SetValue(PRO_blnOeffneSelektionWennKeinItemSelektiertProperty, value);
			}
		}

		public ICommand PRO_cmdChangedCommand
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdChangedCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdChangedCommandProperty, value);
			}
		}

		public ICommand PRO_cmdButtonCommand
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdButtonCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdButtonCommandProperty, value);
			}
		}

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

		public EDU_PopupButton()
		{
			base.SelectionChanged += SUB_SelectionChanged;
			base.DropDownClosed += SUB_DropDownClosed;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Button button = base.Template.FindName("PART_ActionButton", this) as Button;
			if (button != null)
			{
				button.Click += SUB_ActionButtonClick;
			}
		}

		private void SUB_ActionButtonClick(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (PRO_cmdButtonCommand == null || (base.SelectedItem == null && PRO_blnOeffneSelektionWennKeinItemSelektiert))
			{
				base.IsDropDownOpen = true;
			}
			else if (PRO_cmdButtonCommand.CanExecute(base.SelectedItem))
			{
				PRO_cmdButtonCommand.Execute(base.SelectedItem);
			}
		}

		private void SUB_SelectionChanged(object i_objSender, SelectionChangedEventArgs i_fdcArgs)
		{
			m_blnSelektionBehandeln = !base.IsDropDownOpen;
			SUB_SelektionBehandeln(base.SelectedItem);
			if (PRO_blnSelektionNachAuswahlZuruecksetzen)
			{
				base.SelectedItem = null;
			}
		}

		private void SUB_DropDownClosed(object i_objSender, EventArgs i_fdcArgs)
		{
			if (m_blnSelektionBehandeln)
			{
				SUB_SelektionBehandeln(base.SelectedItem);
			}
			m_blnSelektionBehandeln = true;
		}

		private void SUB_SelektionBehandeln(object i_objSelektion)
		{
			if (PRO_cmdChangedCommand != null && PRO_cmdChangedCommand.CanExecute(i_objSelektion))
			{
				PRO_cmdChangedCommand.Execute(i_objSelektion);
			}
		}
	}
}
