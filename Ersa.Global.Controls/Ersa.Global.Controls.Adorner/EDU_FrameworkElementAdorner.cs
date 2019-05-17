using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Ersa.Global.Controls.Adorner
{
	public class EDU_FrameworkElementAdorner : System.Windows.Documents.Adorner
	{
		private FrameworkElement m_fdcChild;

		private ENUM_AdornerPlacement m_enmHorizontalAdornerPlacement;

		private ENUM_AdornerPlacement m_enmVerticalAdornerPlacement;

		private double m_dblOffsetX;

		private double m_dblOffsetY;

		private double m_dblPositionX = double.NaN;

		private double m_dblPositionY = double.NaN;

		public double PRO_dblPositionX
		{
			get
			{
				return m_dblPositionX;
			}
			set
			{
				m_dblPositionX = value;
			}
		}

		public double PRO_dblPositionY
		{
			get
			{
				return m_dblPositionY;
			}
			set
			{
				m_dblPositionY = value;
			}
		}

		public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;

		protected internal override IEnumerator LogicalChildren
		{
			protected get
			{
				return new ArrayList
				{
					m_fdcChild
				}.GetEnumerator();
			}
		}

		protected override int VisualChildrenCount => 1;

		public EDU_FrameworkElementAdorner(FrameworkElement i_fdcAdornerChildElement, FrameworkElement i_fdcAdornedElement, ENUM_AdornerPlacement i_enmHorizontalAdornerPlacement, ENUM_AdornerPlacement i_enmVerticalAdornerPlacement, double i_dblOffsetX, double i_dblOffsetY)
			: base(i_fdcAdornedElement)
		{
			m_fdcChild = i_fdcAdornerChildElement;
			m_enmHorizontalAdornerPlacement = i_enmHorizontalAdornerPlacement;
			m_enmVerticalAdornerPlacement = i_enmVerticalAdornerPlacement;
			m_dblOffsetX = i_dblOffsetX;
			m_dblOffsetY = i_dblOffsetY;
			i_fdcAdornedElement.SizeChanged += SUB_AdornedElementSizeChanged;
			AddLogicalChild(i_fdcAdornerChildElement);
			AddVisualChild(i_fdcAdornerChildElement);
			Binding binding = new Binding("IsVisible")
			{
				Source = i_fdcAdornedElement,
				Converter = new BooleanToVisibilityConverter()
			};
			SetBinding(UIElement.VisibilityProperty, binding);
		}

		public void SUB_DisconnectChild()
		{
			RemoveLogicalChild(m_fdcChild);
			RemoveVisualChild(m_fdcChild);
		}

		protected override Size ArrangeOverride(Size i_fdcFinalSize)
		{
			double num = PRO_dblPositionX;
			if (double.IsNaN(num))
			{
				num = FUN_dblDetermineX();
			}
			double num2 = PRO_dblPositionY;
			if (double.IsNaN(num2))
			{
				num2 = FUN_dblDetermineY();
			}
			double width = FUN_dblDetermineWidth();
			double height = FUN_dblDetermineHeight();
			m_fdcChild.Arrange(new Rect(num, num2, width, height));
			return i_fdcFinalSize;
		}

		protected override Visual GetVisualChild(int i_i32Index)
		{
			return m_fdcChild;
		}

		protected override Size MeasureOverride(Size i_fdcConstraint)
		{
			m_fdcChild.Measure(i_fdcConstraint);
			return m_fdcChild.DesiredSize;
		}

		private double FUN_dblDetermineX()
		{
			switch (m_fdcChild.HorizontalAlignment)
			{
			case HorizontalAlignment.Left:
				if (m_enmHorizontalAdornerPlacement == ENUM_AdornerPlacement.enmOutside)
				{
					return 0.0 - m_fdcChild.DesiredSize.Width + m_dblOffsetX;
				}
				return m_dblOffsetX;
			case HorizontalAlignment.Right:
			{
				if (m_enmHorizontalAdornerPlacement == ENUM_AdornerPlacement.enmOutside)
				{
					return AdornedElement.ActualWidth + m_dblOffsetX;
				}
				double width2 = m_fdcChild.DesiredSize.Width;
				return AdornedElement.ActualWidth - width2 + m_dblOffsetX;
			}
			case HorizontalAlignment.Center:
			{
				double width = m_fdcChild.DesiredSize.Width;
				return AdornedElement.ActualWidth / 2.0 - width / 2.0 + m_dblOffsetX;
			}
			case HorizontalAlignment.Stretch:
				return 0.0;
			default:
				return 0.0;
			}
		}

		private void SUB_AdornedElementSizeChanged(object i_objSender, SizeChangedEventArgs i_fdcArgs)
		{
			InvalidateMeasure();
		}

		private double FUN_dblDetermineY()
		{
			switch (m_fdcChild.VerticalAlignment)
			{
			case VerticalAlignment.Top:
				if (m_enmVerticalAdornerPlacement == ENUM_AdornerPlacement.enmOutside)
				{
					return 0.0 - m_fdcChild.DesiredSize.Height + m_dblOffsetY;
				}
				return m_dblOffsetY;
			case VerticalAlignment.Bottom:
			{
				if (m_enmVerticalAdornerPlacement == ENUM_AdornerPlacement.enmOutside)
				{
					return AdornedElement.ActualHeight + m_dblOffsetY;
				}
				double height2 = m_fdcChild.DesiredSize.Height;
				return AdornedElement.ActualHeight - height2 + m_dblOffsetY;
			}
			case VerticalAlignment.Center:
			{
				double height = m_fdcChild.DesiredSize.Height;
				return AdornedElement.ActualHeight / 2.0 - height / 2.0 + m_dblOffsetY;
			}
			case VerticalAlignment.Stretch:
				return 0.0;
			default:
				return 0.0;
			}
		}

		private double FUN_dblDetermineWidth()
		{
			if (!double.IsNaN(PRO_dblPositionX))
			{
				return m_fdcChild.DesiredSize.Width;
			}
			switch (m_fdcChild.HorizontalAlignment)
			{
			case HorizontalAlignment.Left:
				return m_fdcChild.DesiredSize.Width;
			case HorizontalAlignment.Right:
				return m_fdcChild.DesiredSize.Width;
			case HorizontalAlignment.Center:
				return m_fdcChild.DesiredSize.Width;
			case HorizontalAlignment.Stretch:
				return AdornedElement.ActualWidth;
			default:
				return 0.0;
			}
		}

		private double FUN_dblDetermineHeight()
		{
			if (!double.IsNaN(PRO_dblPositionY))
			{
				return m_fdcChild.DesiredSize.Height;
			}
			switch (m_fdcChild.VerticalAlignment)
			{
			case VerticalAlignment.Top:
				return m_fdcChild.DesiredSize.Height;
			case VerticalAlignment.Bottom:
				return m_fdcChild.DesiredSize.Height;
			case VerticalAlignment.Center:
				return m_fdcChild.DesiredSize.Height;
			case VerticalAlignment.Stretch:
				return AdornedElement.ActualHeight;
			default:
				return 0.0;
			}
		}
	}
}
