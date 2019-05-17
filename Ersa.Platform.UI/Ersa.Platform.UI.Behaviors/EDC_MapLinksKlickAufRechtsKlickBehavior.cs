using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Ersa.Platform.UI.Behaviors
{
	public static class EDC_MapLinksKlickAufRechtsKlickBehavior
	{
		public static readonly DependencyProperty IsLeftClickEnabledProperty = DependencyProperty.RegisterAttached("IsLeftClickEnabled", typeof(bool), typeof(EDC_MapLinksKlickAufRechtsKlickBehavior), new UIPropertyMetadata(false, OnIsLeftClickEnabledChanged));

		public static bool GetIsLeftClickEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsLeftClickEnabledProperty);
		}

		public static void SetIsLeftClickEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsLeftClickEnabledProperty, value);
		}

		private static void OnIsLeftClickEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement == null)
			{
				return;
			}
			if (e.NewValue is bool && (bool)e.NewValue)
			{
				if (uIElement is ButtonBase)
				{
					((ButtonBase)uIElement).Click += OnMouseLeftButtonDown;
				}
				else
				{
					uIElement.MouseLeftButtonDown += OnMouseLeftButtonDown;
				}
			}
			else if (uIElement is ButtonBase)
			{
				((ButtonBase)uIElement).Click -= OnMouseLeftButtonDown;
			}
			else
			{
				uIElement.MouseLeftButtonDown -= OnMouseLeftButtonDown;
			}
		}

		private static void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			MouseButtonEventArgs input = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right)
			{
				RoutedEvent = Mouse.MouseUpEvent
			};
			InputManager.Current.ProcessInput(input);
		}
	}
}
