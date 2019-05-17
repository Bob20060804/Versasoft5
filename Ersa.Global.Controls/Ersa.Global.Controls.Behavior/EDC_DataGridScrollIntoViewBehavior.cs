using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_DataGridScrollIntoViewBehavior : Behavior<DataGrid>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.SelectionChanged += SUB_SelectionChanged;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.SelectionChanged -= SUB_SelectionChanged;
			base.OnDetaching();
		}

		private void SUB_SelectionChanged(object i_blnSender, SelectionChangedEventArgs i_blnE)
		{
			if (base.AssociatedObject.SelectedItem != null)
			{
				base.AssociatedObject.ScrollIntoView(base.AssociatedObject.SelectedItem);
			}
		}
	}
}
