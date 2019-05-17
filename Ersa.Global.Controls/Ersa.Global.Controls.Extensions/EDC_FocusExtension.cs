using System.Windows;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_FocusExtension
	{
		public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("PRO_blnIstFokusiert", typeof(bool), typeof(EDC_FocusExtension), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

		public static bool GetPRO_blnIstFokusiert(DependencyObject i_fdcElement)
		{
			return (bool)i_fdcElement.GetValue(IsFocusedProperty);
		}

		public static void SetPRO_blnIstFokusiert(DependencyObject i_fdcElement, bool i_blnWert)
		{
			i_fdcElement.SetValue(IsFocusedProperty, i_blnWert);
		}

		private static void OnIsFocusedPropertyChanged(DependencyObject i_fdcElement, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			UIElement uIElement = (UIElement)i_fdcElement;
			if ((bool)i_fdcArgs.NewValue)
			{
				uIElement.Focus();
			}
		}
	}
}
