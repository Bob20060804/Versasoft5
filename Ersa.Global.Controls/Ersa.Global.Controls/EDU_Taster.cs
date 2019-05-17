using Ersa.Global.Controls.Extensions;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_Taster : EDU_ToggleButton
	{
		public static readonly DependencyProperty PRO_cmdTasterAktiviertProperty;

		public static readonly DependencyProperty PRO_cmdTasterDeaktiviertProperty;

		public ICommand PRO_cmdTasterAktiviert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdTasterAktiviertProperty);
			}
			set
			{
				SetValue(PRO_cmdTasterAktiviertProperty, value);
			}
		}

		public ICommand PRO_cmdTasterDeaktiviert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdTasterDeaktiviertProperty);
			}
			set
			{
				SetValue(PRO_cmdTasterDeaktiviertProperty, value);
			}
		}

		public bool PRO_blnClickAktiviertAktivStatus
		{
			get;
			set;
		}

		static EDU_Taster()
		{
			PRO_cmdTasterAktiviertProperty = DependencyProperty.Register("PRO_cmdTasterAktiviert", typeof(ICommand), typeof(EDU_Taster));
			PRO_cmdTasterDeaktiviertProperty = DependencyProperty.Register("PRO_cmdTasterDeaktiviert", typeof(ICommand), typeof(EDU_Taster));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_Taster), new FrameworkPropertyMetadata(typeof(EDU_Taster)));
		}

		public EDU_Taster()
		{
			base.IsEnabledChanged += SUB_EnabledChanged;
			base.MouseLeave += SUB_MouseLeave;
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs i_fdcArgs)
		{
			base.OnPreviewMouseDown(i_fdcArgs);
			SUB_TasterAktivieren();
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs i_fdcArgs)
		{
			base.OnPreviewMouseUp(i_fdcArgs);
			SUB_TasterDeaktivieren();
		}

		protected override void OnPreviewTouchDown(TouchEventArgs i_fdcArgs)
		{
			base.OnPreviewTouchDown(i_fdcArgs);
			SUB_TasterAktivieren();
		}

		protected override void OnPreviewTouchUp(TouchEventArgs i_fdcArgs)
		{
			base.OnPreviewTouchDown(i_fdcArgs);
			SUB_TasterDeaktivieren();
		}

		private void SUB_TasterAktivieren()
		{
			if (PRO_blnClickAktiviertAktivStatus)
			{
				base.PRO_blnIstAktiv = true;
			}
			if (PRO_cmdTasterAktiviert != null)
			{
				PRO_cmdTasterAktiviert.SUB_Execute(base.CommandParameter, this);
			}
		}

		private void SUB_TasterDeaktivieren()
		{
			if (PRO_blnClickAktiviertAktivStatus)
			{
				base.PRO_blnIstAktiv = false;
			}
			if (PRO_cmdTasterDeaktiviert != null)
			{
				PRO_cmdTasterDeaktiviert.SUB_Execute(base.CommandParameter, this);
			}
		}

		private void SUB_MouseLeave(object sender, MouseEventArgs e)
		{
			if (base.PRO_blnIstAktiv)
			{
				SUB_TasterDeaktivieren();
			}
		}

		private void SUB_EnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!base.IsEnabled && base.PRO_blnIstAktiv)
			{
				SUB_TasterDeaktivieren();
			}
		}
	}
}
