using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Ersa.Global.Controls.Adorner
{
	public class EDU_AdornedControl : ContentControl
	{
		public static readonly RoutedCommand ShowAdornerCommand;

		public static readonly RoutedCommand HideAdornerCommand;

		public static readonly DependencyProperty IsAdornerVisibleProperty;

		public static readonly DependencyProperty AdornerContentProperty;

		public static readonly DependencyProperty HorizontalAdornerPlacementProperty;

		public static readonly DependencyProperty VerticalAdornerPlacementProperty;

		public static readonly DependencyProperty AdornerOffsetXProperty;

		public static readonly DependencyProperty AdornerOffsetYProperty;

		private static readonly CommandBinding ShowAdornerCommandBinding;

		private static readonly CommandBinding HideAdornerCommandBinding;

		private AdornerLayer m_fdcAdornerLayer;

		private EDU_FrameworkElementAdorner m_edcAdorner;

		public bool IsAdornerVisible
		{
			get
			{
				return (bool)GetValue(IsAdornerVisibleProperty);
			}
			set
			{
				SetValue(IsAdornerVisibleProperty, value);
			}
		}

		public FrameworkElement AdornerContent
		{
			get
			{
				return (FrameworkElement)GetValue(AdornerContentProperty);
			}
			set
			{
				SetValue(AdornerContentProperty, value);
			}
		}

		public ENUM_AdornerPlacement HorizontalAdornerPlacement
		{
			get
			{
				return (ENUM_AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty);
			}
			set
			{
				SetValue(HorizontalAdornerPlacementProperty, value);
			}
		}

		public ENUM_AdornerPlacement VerticalAdornerPlacement
		{
			get
			{
				return (ENUM_AdornerPlacement)GetValue(VerticalAdornerPlacementProperty);
			}
			set
			{
				SetValue(VerticalAdornerPlacementProperty, value);
			}
		}

		public double AdornerOffsetX
		{
			get
			{
				return (double)GetValue(AdornerOffsetXProperty);
			}
			set
			{
				SetValue(AdornerOffsetXProperty, value);
			}
		}

		public double AdornerOffsetY
		{
			get
			{
				return (double)GetValue(AdornerOffsetYProperty);
			}
			set
			{
				SetValue(AdornerOffsetYProperty, value);
			}
		}

		static EDU_AdornedControl()
		{
			ShowAdornerCommand = new RoutedCommand("ShowAdorner", typeof(EDU_AdornedControl));
			HideAdornerCommand = new RoutedCommand("HideAdorner", typeof(EDU_AdornedControl));
			IsAdornerVisibleProperty = DependencyProperty.Register("IsAdornerVisible", typeof(bool), typeof(EDU_AdornedControl), new FrameworkPropertyMetadata(SUB_IsAdornerVisiblePropertyChanged));
			AdornerContentProperty = DependencyProperty.Register("AdornerContent", typeof(FrameworkElement), typeof(EDU_AdornedControl), new FrameworkPropertyMetadata(SUB_AdornerContentPropertyChanged));
			HorizontalAdornerPlacementProperty = DependencyProperty.Register("HorizontalAdornerPlacement", typeof(ENUM_AdornerPlacement), typeof(EDU_AdornedControl), new FrameworkPropertyMetadata(ENUM_AdornerPlacement.enmInside));
			VerticalAdornerPlacementProperty = DependencyProperty.Register("VerticalAdornerPlacement", typeof(ENUM_AdornerPlacement), typeof(EDU_AdornedControl), new FrameworkPropertyMetadata(ENUM_AdornerPlacement.enmInside));
			AdornerOffsetXProperty = DependencyProperty.Register("AdornerOffsetX", typeof(double), typeof(EDU_AdornedControl));
			AdornerOffsetYProperty = DependencyProperty.Register("AdornerOffsetY", typeof(double), typeof(EDU_AdornedControl));
			ShowAdornerCommandBinding = new CommandBinding(ShowAdornerCommand, SUB_ShowAdornerCommandExecuted);
			HideAdornerCommandBinding = new CommandBinding(HideAdornerCommand, SUB_HideAdornerCommandExecuted);
			CommandManager.RegisterClassCommandBinding(typeof(EDU_AdornedControl), ShowAdornerCommandBinding);
			CommandManager.RegisterClassCommandBinding(typeof(EDU_AdornedControl), HideAdornerCommandBinding);
		}

		public EDU_AdornedControl()
		{
			base.Focusable = false;
			base.DataContextChanged += SUB_AdornedControlDataContextChanged;
		}

		public void ShowAdorner()
		{
			IsAdornerVisible = true;
		}

		public void HideAdorner()
		{
			IsAdornerVisible = false;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			SUB_ShowOrHideAdornerInternal();
		}

		private static void SUB_ShowAdornerCommandExecuted(object i_objTarget, ExecutedRoutedEventArgs i_fdcArgs)
		{
			((EDU_AdornedControl)i_objTarget).ShowAdorner();
		}

		private static void SUB_HideAdornerCommandExecuted(object i_objTarget, ExecutedRoutedEventArgs i_fdcArgs)
		{
			((EDU_AdornedControl)i_objTarget).HideAdorner();
		}

		private static void SUB_IsAdornerVisiblePropertyChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			((EDU_AdornedControl)i_objSender).SUB_ShowOrHideAdornerInternal();
		}

		private static void SUB_AdornerContentPropertyChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			((EDU_AdornedControl)i_objSender).SUB_ShowOrHideAdornerInternal();
		}

		private void SUB_ShowOrHideAdornerInternal()
		{
			if (IsAdornerVisible)
			{
				SUB_ShowAdornerInternal();
			}
			else
			{
				SUB_HideAdornerInternal();
			}
		}

		private void SUB_ShowAdornerInternal()
		{
			if (m_edcAdorner == null && AdornerContent != null)
			{
				if (m_fdcAdornerLayer == null)
				{
					m_fdcAdornerLayer = AdornerLayer.GetAdornerLayer(this);
				}
				if (m_fdcAdornerLayer != null)
				{
					m_edcAdorner = new EDU_FrameworkElementAdorner(AdornerContent, this, HorizontalAdornerPlacement, VerticalAdornerPlacement, AdornerOffsetX, AdornerOffsetY);
					m_fdcAdornerLayer.Add(m_edcAdorner);
					SUB_UpdateAdornerDataContext();
				}
			}
		}

		private void SUB_HideAdornerInternal()
		{
			if (m_fdcAdornerLayer != null && m_edcAdorner != null)
			{
				m_fdcAdornerLayer.Remove(m_edcAdorner);
				m_edcAdorner.SUB_DisconnectChild();
				m_edcAdorner = null;
				m_fdcAdornerLayer = null;
			}
		}

		private void SUB_AdornedControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			SUB_UpdateAdornerDataContext();
		}

		private void SUB_UpdateAdornerDataContext()
		{
			if (AdornerContent != null)
			{
				AdornerContent.DataContext = base.DataContext;
			}
		}
	}
}
