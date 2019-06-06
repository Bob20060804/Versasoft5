using Ersa.Global.Controls.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_DataGridScrollBehavior : Behavior<DataGrid>
	{
		private readonly object m_objSyncObject = new object();

		private ScrollViewer m_fdcParentScrollViewer;

		private ScrollViewer m_fdcScrollViewer;

		private TouchDevice m_fdcTouchDevice;

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.Loaded += SUB_OnDataGridLoaded;
			base.AssociatedObject.Unloaded += SUB_OnDataGridUnloaded;
			base.AssociatedObject.ApplyTemplate();
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.Loaded -= SUB_OnDataGridLoaded;
			base.AssociatedObject.Unloaded -= SUB_OnDataGridUnloaded;
			base.OnDetaching();
		}

		private void SUB_OnDataGridLoaded(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			base.AssociatedObject.PreviewMouseWheel += SUB_OnMausradScrolling;
			ScrollViewer scrollViewer = FUN_fdcScrollViewerErmitteln();
			if (scrollViewer != null)
			{
				scrollViewer.PreviewTouchDown += SUB_OnPreviewTouchDown;
				scrollViewer.PreviewTouchUp += SUB_OnPreviewTouchUp;
				scrollViewer.ManipulationBoundaryFeedback += SUB_OnManipulationBoundaryFeedback;
			}
		}

		private void SUB_OnDataGridUnloaded(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			base.AssociatedObject.PreviewMouseWheel -= SUB_OnMausradScrolling;
			ScrollViewer scrollViewer = FUN_fdcScrollViewerErmitteln();
			if (scrollViewer != null)
			{
				scrollViewer.PreviewTouchDown -= SUB_OnPreviewTouchDown;
				scrollViewer.PreviewTouchUp -= SUB_OnPreviewTouchUp;
				scrollViewer.ManipulationBoundaryFeedback -= SUB_OnManipulationBoundaryFeedback;
			}
		}

		private void SUB_OnMausradScrolling(object i_objSender, MouseWheelEventArgs i_fdcArgs)
		{
			ScrollViewer scrollViewer = FUN_fdcScrollViewerErmitteln();
			if (scrollViewer != null && FUN_blnScrollEventWeiterleiten(scrollViewer, i_fdcArgs.Delta))
			{
				SUB_MausradEventBehandelnUndNeuErzeugen(i_fdcArgs);
			}
		}

		private bool FUN_blnScrollEventWeiterleiten(ScrollViewer i_fdcScrollViewer, int i_i32ScrollDelta)
		{
			if (i_fdcScrollViewer.ComputedVerticalScrollBarVisibility != 0)
			{
				return true;
			}
			if (i_i32ScrollDelta < 0 && i_fdcScrollViewer.VerticalOffset >= i_fdcScrollViewer.ScrollableHeight)
			{
				return true;
			}
			if (i_i32ScrollDelta > 0 && i_fdcScrollViewer.VerticalOffset <= 0.0)
			{
				return true;
			}
			return false;
		}

		private void SUB_MausradEventBehandelnUndNeuErzeugen(MouseWheelEventArgs i_fdcArgs)
		{
			i_fdcArgs.Handled = true;
			MouseWheelEventArgs e = new MouseWheelEventArgs(i_fdcArgs.MouseDevice, i_fdcArgs.Timestamp, i_fdcArgs.Delta)
			{
				RoutedEvent = UIElement.MouseWheelEvent
			};
			base.AssociatedObject.RaiseEvent(e);
		}

		private void SUB_OnPreviewTouchDown(object i_objSender, TouchEventArgs i_fdcArgs)
		{
			m_fdcTouchDevice = i_fdcArgs.TouchDevice;
			ScrollViewer scrollViewer = FUN_fdcParentScrollViewerErmitteln();
			if (scrollViewer != null)
			{
				ScrollViewer scrollViewer2 = FUN_fdcScrollViewerErmitteln();
				if (scrollViewer2 != null && scrollViewer2.ComputedVerticalScrollBarVisibility != 0)
				{
					scrollViewer.CaptureTouch(i_fdcArgs.TouchDevice);
				}
			}
		}

		private void SUB_OnPreviewTouchUp(object i_objSender, TouchEventArgs i_fdcArgs)
		{
			FUN_fdcParentScrollViewerErmitteln()?.ReleaseTouchCapture(i_fdcArgs.TouchDevice);
		}

		private void SUB_OnManipulationBoundaryFeedback(object i_objSender, ManipulationBoundaryFeedbackEventArgs i_fdcArgs)
		{
			ScrollViewer scrollViewer = FUN_fdcParentScrollViewerErmitteln();
			if (scrollViewer != null && m_fdcTouchDevice != null)
			{
				scrollViewer.CaptureTouch(m_fdcTouchDevice);
				i_fdcArgs.Handled = true;
			}
		}

		private ScrollViewer FUN_fdcScrollViewerErmitteln()
		{
			lock (m_objSyncObject)
			{
				return m_fdcScrollViewer ?? (m_fdcScrollViewer = base.AssociatedObject.FUN_objBenanntesKindElementSuchen<ScrollViewer>("DG_ScrollViewer").FirstOrDefault());
			}
		}

		private ScrollViewer FUN_fdcParentScrollViewerErmitteln()
		{
			lock (m_objSyncObject)
			{
				return m_fdcParentScrollViewer ?? (m_fdcParentScrollViewer = base.AssociatedObject.FUN_objElternElementErmitteln<ScrollViewer>());
			}
		}
	}
}
