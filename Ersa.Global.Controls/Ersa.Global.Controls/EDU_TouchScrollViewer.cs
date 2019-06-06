using Ersa.Global.Controls.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_TouchScrollViewer : ScrollViewer
	{
		private PanningMode m_enmInitialerPanningMode;

		public EDU_TouchScrollViewer()
		{
			base.Loaded += delegate
			{
				m_enmInitialerPanningMode = base.PanningMode;
			};
		}

		protected override void OnManipulationStarted(ManipulationStartedEventArgs i_fdcArgs)
		{
			EDC_HitTestHelfer.SUB_PruefeObPositionUeberButton(this, i_fdcArgs.ManipulationOrigin, delegate(bool i_blnTouchInnerhalbButton)
			{
				base.PanningMode = ((!i_blnTouchInnerhalbButton) ? m_enmInitialerPanningMode : PanningMode.None);
			});
			base.OnManipulationStarted(i_fdcArgs);
		}

		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs i_fdcArgs)
		{
			base.OnManipulationCompleted(i_fdcArgs);
			base.PanningMode = m_enmInitialerPanningMode;
		}

		protected override void OnTouchDown(TouchEventArgs i_fdcArgs)
		{
			base.PanningMode = m_enmInitialerPanningMode;
			base.OnTouchDown(i_fdcArgs);
		}
	}
}
