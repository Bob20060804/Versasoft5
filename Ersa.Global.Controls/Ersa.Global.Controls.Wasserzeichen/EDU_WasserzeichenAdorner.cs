using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Ersa.Global.Controls.Wasserzeichen
{
	internal class EDU_WasserzeichenAdorner : System.Windows.Documents.Adorner
	{
		private const double mC_dblAbstandLinks = 8.0;

		private readonly ContentPresenter m_fdcContentPresenter;

		protected override int VisualChildrenCount => 1;

		private Control PRO_fdcAdornedControl => (Control)base.AdornedElement;

		public EDU_WasserzeichenAdorner(UIElement i_fdcAdornedElement, object i_objWasserzeichen, ResourceDictionary i_fdcResources = null)
			: base(i_fdcAdornedElement)
		{
			base.IsHitTestVisible = false;
			m_fdcContentPresenter = new ContentPresenter
			{
				Content = i_objWasserzeichen,
				Margin = new Thickness(PRO_fdcAdornedControl.Margin.Left + PRO_fdcAdornedControl.Padding.Left + 8.0, PRO_fdcAdornedControl.Margin.Top + PRO_fdcAdornedControl.Padding.Top, 0.0, 0.0)
			};
			if (i_fdcResources != null)
			{
				m_fdcContentPresenter.Resources = i_fdcResources;
			}
			Binding binding = new Binding("IsVisible")
			{
				Source = i_fdcAdornedElement,
				Converter = new BooleanToVisibilityConverter()
			};
			SetBinding(UIElement.VisibilityProperty, binding);
		}

		protected override Visual GetVisualChild(int i_i32Index)
		{
			return m_fdcContentPresenter;
		}

		protected override Size MeasureOverride(Size i_fdcConstraint)
		{
			m_fdcContentPresenter.Measure(PRO_fdcAdornedControl.RenderSize);
			return PRO_fdcAdornedControl.RenderSize;
		}

		protected override Size ArrangeOverride(Size i_fdcFinalSize)
		{
			m_fdcContentPresenter.Arrange(new Rect(i_fdcFinalSize));
			return i_fdcFinalSize;
		}
	}
}
