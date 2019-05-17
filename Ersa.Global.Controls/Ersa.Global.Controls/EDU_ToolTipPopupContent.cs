using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_ToolTipPopupContent : ContentControl
	{
		static EDU_ToolTipPopupContent()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ToolTipPopupContent), new FrameworkPropertyMetadata(typeof(EDU_ToolTipPopupContent)));
		}
	}
}
