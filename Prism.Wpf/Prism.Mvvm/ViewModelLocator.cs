using System.Windows;

namespace Prism.Mvvm
{
	public static class ViewModelLocator
	{
		public static DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

		public static bool GetAutoWireViewModel(DependencyObject obj)
		{
			return (bool)obj.GetValue(AutoWireViewModelProperty);
		}

		public static void SetAutoWireViewModel(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoWireViewModelProperty, value);
		}

		private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				ViewModelLocationProvider.AutoWireViewModelChanged(d, Bind);
			}
		}

		private static void Bind(object view, object viewModel)
		{
			FrameworkElement frameworkElement = view as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.DataContext = viewModel;
			}
		}
	}
}
