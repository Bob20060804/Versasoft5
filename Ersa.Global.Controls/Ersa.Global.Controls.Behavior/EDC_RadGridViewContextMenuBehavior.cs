using Ersa.Global.Controls.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_RadGridViewContextMenuBehavior : Behavior<RadGridView>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			if (base.AssociatedObject.ContextMenu != null)
			{
				base.AssociatedObject.ContextMenuOpening += SUB_ContextMenuOeffnend;
			}
		}

		protected override void OnDetaching()
		{
			if (base.AssociatedObject.ContextMenu != null)
			{
				base.AssociatedObject.ContextMenuOpening -= SUB_ContextMenuOeffnend;
			}
			base.OnDetaching();
		}

		private void SUB_ContextMenuOeffnend(object i_objSender, ContextMenuEventArgs i_fdcArgs)
		{
			UIElement uIElement = i_fdcArgs.OriginalSource as UIElement;
			if (uIElement == null)
			{
				return;
			}
			DependencyObject dependencyObject = uIElement.InputHitTest(new Point(i_fdcArgs.CursorLeft, i_fdcArgs.CursorTop)) as DependencyObject;
			if (dependencyObject == null)
			{
				return;
			}
			GridViewCell gridViewCell = dependencyObject.FUN_objElternElementErmitteln<GridViewCell>();
			if (gridViewCell != null)
			{
				gridViewCell.IsCurrent = true;
				if (!gridViewCell.IsSelected)
				{
					base.AssociatedObject.SelectedCells.Clear();
					gridViewCell.IsSelected = true;
					gridViewCell.IsCurrent = true;
					GridViewCellInfo item = new GridViewCellInfo(gridViewCell);
					if (!base.AssociatedObject.SelectedCells.Contains(item))
					{
						base.AssociatedObject.SelectedCells.Add(item);
					}
				}
				gridViewCell.IsCurrent = true;
			}
			else
			{
				i_fdcArgs.Handled = true;
			}
		}
	}
}
