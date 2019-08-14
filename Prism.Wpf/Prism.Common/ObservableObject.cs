using System.ComponentModel;
using System.Windows;

namespace Prism.Common
{
	public class ObservableObject<T> : FrameworkElement, INotifyPropertyChanged
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(ObservableObject<T>), new PropertyMetadata(ValueChangedCallback));

		public T Value
		{
			get
			{
				return (T)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ObservableObject<T> observableObject = (ObservableObject<T>)d;
			observableObject.PropertyChanged?.Invoke(observableObject, new PropertyChangedEventArgs("Value"));
		}
	}
}
