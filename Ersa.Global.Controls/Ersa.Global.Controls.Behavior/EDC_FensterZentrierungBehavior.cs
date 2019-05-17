using System.Windows;
using System.Windows.Interactivity;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_FensterZentrierungBehavior : Behavior<Window>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.SizeChanged += SUB_OnWindowSizeChanged;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.SizeChanged -= SUB_OnWindowSizeChanged;
			base.OnDetaching();
		}

		private static void SUB_OnWindowSizeChanged(object i_objSender, SizeChangedEventArgs i_fdcArgs)
		{
			Window window = i_objSender as Window;
			if (window != null)
			{
				window.WindowStartupLocation = WindowStartupLocation.Manual;
				window.Left = (SystemParameters.WorkArea.Width - window.ActualWidth) / 2.0 + SystemParameters.WorkArea.Left;
				window.Top = (SystemParameters.WorkArea.Height - window.ActualHeight) / 2.0 + SystemParameters.WorkArea.Top;
			}
		}
	}
}
