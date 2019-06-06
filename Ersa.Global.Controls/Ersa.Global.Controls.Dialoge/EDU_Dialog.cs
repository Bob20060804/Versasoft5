using Ersa.Global.Controls.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Dialoge
{
	public abstract class EDU_Dialog : Window
	{
		public static readonly DependencyProperty ms_strTitelProperty = DependencyProperty.Register("PRO_strTitel", typeof(string), typeof(EDU_Dialog));

		public static readonly DependencyProperty ms_strTextProperty = DependencyProperty.Register("PRO_strText", typeof(string), typeof(EDU_Dialog));

		private readonly bool m_blnDialogSchliessenMitEscapeUnterbinden;

		public string PRO_strTitel
		{
			get
			{
				return (string)GetValue(ms_strTitelProperty);
			}
			set
			{
				SetValue(ms_strTitelProperty, value);
			}
		}

		public string PRO_strText
		{
			get
			{
				return (string)GetValue(ms_strTextProperty);
			}
			set
			{
				SetValue(ms_strTextProperty, value);
			}
		}

		protected EDU_Dialog()
		{
			base.Initialized += SUB_DialogInitialisiert;
			base.Loaded += SUB_DialogGeladen;
			base.PreviewKeyUp += SUB_TasteGedrueckt;
			base.ResizeMode = ResizeMode.NoResize;
			base.AllowsTransparency = true;
			base.WindowStyle = WindowStyle.None;
			base.Background = null;
			base.WindowStartupLocation = WindowStartupLocation.Manual;
			base.ShowInTaskbar = false;
			base.IsManipulationEnabled = false;
		}

		protected EDU_Dialog(Window i_fdcOwner)
			: this()
		{
			base.Owner = i_fdcOwner;
		}

		protected EDU_Dialog(Window i_fdcOwner, bool i_blnDialogSchliessenMitEscapeUnterbinden)
			: this(i_fdcOwner)
		{
			m_blnDialogSchliessenMitEscapeUnterbinden = i_blnDialogSchliessenMitEscapeUnterbinden;
		}

		private void SUB_DialogInitialisiert(object i_objSender, EventArgs i_fdcArgs)
		{
			if (base.Owner != null)
			{
				base.MinWidth = base.Owner.MinWidth;
				base.MaxWidth = base.Owner.MaxWidth;
				base.Width = base.Owner.Width;
				base.MinHeight = base.Owner.MinHeight;
				base.MaxHeight = base.Owner.MaxHeight;
				base.Height = base.Owner.Height;
				base.Left = base.Owner.Left;
				base.Top = base.Owner.Top;
				if (base.Owner.WindowState == WindowState.Maximized)
				{
					base.WindowState = WindowState.Maximized;
				}
			}
		}

		private void SUB_DialogGeladen(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			EDU_DialogGroupBox eDU_DialogGroupBox = this.FUN_lstKindElementeSuchen<EDU_DialogGroupBox>().FirstOrDefault();
			if (eDU_DialogGroupBox != null && eDU_DialogGroupBox.Width > base.ActualWidth)
			{
				eDU_DialogGroupBox.Width = base.ActualWidth;
			}
		}

		private void SUB_TasteGedrueckt(object i_objSender, KeyEventArgs i_fdcArgs)
		{
			if (!m_blnDialogSchliessenMitEscapeUnterbinden && i_fdcArgs.Key == Key.Escape)
			{
				if (base.DialogResult.HasValue)
				{
					base.DialogResult = false;
				}
				Close();
			}
		}
	}
}
