using Ersa.Global.Controls.Extensions;
using Ersa.Global.Controls.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_DataGridRowAuswahlBehavior : Behavior<FrameworkElement>
	{
		private DataGridRow m_fdcDataGridRow;

		private DataGridRow PRO_fdcDataGridRow => m_fdcDataGridRow ?? (m_fdcDataGridRow = base.AssociatedObject.FUN_objElternElementErmitteln<DataGridRow>());

		protected override void OnAttached()
		{
			base.OnAttached();
			if (PRO_fdcDataGridRow != null)
			{
				PRO_fdcDataGridRow.Loaded += SUB_OnDataGridRowLoaded;
				PRO_fdcDataGridRow.Unloaded += SUB_OnDataGridRowUnloaded;
			}
		}

		protected override void OnDetaching()
		{
			PRO_fdcDataGridRow.Loaded -= SUB_OnDataGridRowLoaded;
			PRO_fdcDataGridRow.Unloaded -= SUB_OnDataGridRowUnloaded;
			DataGridRowHeader dataGridRowHeader = FUN_fdcRowHeaderErmitteln();
			if (dataGridRowHeader != null)
			{
				dataGridRowHeader.PreviewMouseDown -= SUB_OnDataGridRowPreviewMouseDown;
			}
			DataGridCellsPresenter dataGridCellsPresenter = FUN_fdcCellsPresenterErmitteln();
			if (dataGridCellsPresenter != null)
			{
				dataGridCellsPresenter.PreviewMouseDown -= SUB_OnDataGridRowPreviewMouseDown;
			}
			base.OnDetaching();
		}

		private void SUB_OnDataGridRowLoaded(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			DataGridRowHeader dataGridRowHeader = FUN_fdcRowHeaderErmitteln();
			if (dataGridRowHeader != null)
			{
				dataGridRowHeader.PreviewMouseDown += SUB_OnDataGridRowPreviewMouseDown;
			}
			DataGridCellsPresenter dataGridCellsPresenter = FUN_fdcCellsPresenterErmitteln();
			if (dataGridCellsPresenter != null)
			{
				dataGridCellsPresenter.PreviewMouseDown += SUB_OnDataGridRowPreviewMouseDown;
			}
		}

		private void SUB_OnDataGridRowUnloaded(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			DataGridRowHeader dataGridRowHeader = FUN_fdcRowHeaderErmitteln();
			if (dataGridRowHeader != null)
			{
				dataGridRowHeader.PreviewMouseDown -= SUB_OnDataGridRowPreviewMouseDown;
			}
			DataGridCellsPresenter dataGridCellsPresenter = FUN_fdcCellsPresenterErmitteln();
			if (dataGridCellsPresenter != null)
			{
				dataGridCellsPresenter.PreviewMouseDown -= SUB_OnDataGridRowPreviewMouseDown;
			}
		}

		private void SUB_OnDataGridRowPreviewMouseDown(object i_objSender, MouseButtonEventArgs i_fdcArgs)
		{
			if (PRO_fdcDataGridRow != null)
			{
				EDC_HitTestHelfer.SUB_PruefeObPositionUeberButton(i_objSender as Visual, i_fdcArgs.GetPosition(i_objSender as IInputElement), delegate(bool i_blnMausUeberButton)
				{
					if (!i_blnMausUeberButton)
					{
						if (PRO_fdcDataGridRow.IsSelected)
						{
							PRO_fdcDataGridRow.DetailsVisibility = ((PRO_fdcDataGridRow.DetailsVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible);
							EDC_RoutedCommands.ms_cmdDataGridRowAusgeklapptGeaendert.SUB_Execute(PRO_fdcDataGridRow.DataContext, PRO_fdcDataGridRow);
						}
						else
						{
							if (PRO_fdcDataGridRow.DetailsVisibility != 0)
							{
								PRO_fdcDataGridRow.DetailsVisibility = Visibility.Visible;
								EDC_RoutedCommands.ms_cmdDataGridRowAusgeklapptGeaendert.SUB_Execute(PRO_fdcDataGridRow.DataContext, PRO_fdcDataGridRow);
							}
							PRO_fdcDataGridRow.IsSelected = true;
						}
					}
				});
			}
		}

		private DataGridRowHeader FUN_fdcRowHeaderErmitteln()
		{
			return base.AssociatedObject.FUN_lstKindElementeSuchen<DataGridRowHeader>().FirstOrDefault();
		}

		private DataGridCellsPresenter FUN_fdcCellsPresenterErmitteln()
		{
			return base.AssociatedObject.FUN_lstKindElementeSuchen<DataGridCellsPresenter>().FirstOrDefault();
		}
	}
}
