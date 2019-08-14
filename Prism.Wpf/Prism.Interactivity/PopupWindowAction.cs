using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Interactivity.DefaultPopupWindows;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Prism.Interactivity
{
	public class PopupWindowAction : TriggerAction<FrameworkElement>
	{
		public static readonly DependencyProperty WindowContentProperty = DependencyProperty.Register("WindowContent", typeof(FrameworkElement), typeof(PopupWindowAction), new PropertyMetadata(null));

		public static readonly DependencyProperty WindowContentTypeProperty = DependencyProperty.Register("WindowContentType", typeof(Type), typeof(PopupWindowAction), new PropertyMetadata(null));

		public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register("IsModal", typeof(bool), typeof(PopupWindowAction), new PropertyMetadata(null));

		public static readonly DependencyProperty CenterOverAssociatedObjectProperty = DependencyProperty.Register("CenterOverAssociatedObject", typeof(bool), typeof(PopupWindowAction), new PropertyMetadata(null));

		public static readonly DependencyProperty WindowStartupLocationProperty = DependencyProperty.Register("WindowStartupLocation", typeof(WindowStartupLocation?), typeof(PopupWindowAction), new PropertyMetadata(null));

		public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register("WindowStyle", typeof(Style), typeof(PopupWindowAction), new PropertyMetadata(null));

		public FrameworkElement WindowContent
		{
			get
			{
				return (FrameworkElement)GetValue(WindowContentProperty);
			}
			set
			{
				SetValue(WindowContentProperty, value);
			}
		}

		public Type WindowContentType
		{
			get
			{
				return (Type)GetValue(WindowContentTypeProperty);
			}
			set
			{
				SetValue(WindowContentTypeProperty, value);
			}
		}

		public bool IsModal
		{
			get
			{
				return (bool)GetValue(IsModalProperty);
			}
			set
			{
				SetValue(IsModalProperty, value);
			}
		}

		public bool CenterOverAssociatedObject
		{
			get
			{
				return (bool)GetValue(CenterOverAssociatedObjectProperty);
			}
			set
			{
				SetValue(CenterOverAssociatedObjectProperty, value);
			}
		}

		public WindowStartupLocation? WindowStartupLocation
		{
			get
			{
				return (WindowStartupLocation?)GetValue(WindowStartupLocationProperty);
			}
			set
			{
				SetValue(WindowStartupLocationProperty, value);
			}
		}

		public Style WindowStyle
		{
			get
			{
				return (Style)GetValue(WindowStyleProperty);
			}
			set
			{
				SetValue(WindowStyleProperty, value);
			}
		}

		protected override void Invoke(object parameter)
		{
			InteractionRequestedEventArgs interactionRequestedEventArgs = parameter as InteractionRequestedEventArgs;
			if (interactionRequestedEventArgs != null && (WindowContent == null || WindowContent.Parent == null))
			{
				Window wrapperWindow = GetWindow(interactionRequestedEventArgs.Context);
				Action callback = interactionRequestedEventArgs.Callback;
				EventHandler handler = null;
				handler = delegate
				{
					wrapperWindow.Closed -= handler;
					wrapperWindow.Content = null;
					if (callback != null)
					{
						callback();
					}
				};
				wrapperWindow.Closed += handler;
				if (CenterOverAssociatedObject && base.AssociatedObject != null)
				{
					SizeChangedEventHandler sizeHandler = null;
					sizeHandler = delegate
					{
						wrapperWindow.SizeChanged -= sizeHandler;
						Window owner = wrapperWindow.Owner;
						if (owner != null && owner.WindowState == WindowState.Minimized)
						{
							wrapperWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
						}
						else
						{
							FrameworkElement associatedObject = base.AssociatedObject;
							Point point = associatedObject.PointToScreen(new Point(0.0, 0.0));
							point = PresentationSource.FromVisual(associatedObject).CompositionTarget.TransformFromDevice.Transform(point);
							Point point2 = new Point(point.X + associatedObject.ActualWidth / 2.0, point.Y + associatedObject.ActualHeight / 2.0);
							wrapperWindow.Left = point2.X - wrapperWindow.ActualWidth / 2.0;
							wrapperWindow.Top = point2.Y - wrapperWindow.ActualHeight / 2.0;
						}
					};
					wrapperWindow.SizeChanged += sizeHandler;
				}
				if (IsModal)
				{
					wrapperWindow.ShowDialog();
				}
				else
				{
					wrapperWindow.Show();
				}
			}
		}

		protected virtual Window GetWindow(INotification notification)
		{
			Window window;
			if (WindowContent != null || WindowContentType != null)
			{
				window = CreateWindow();
				if (window == null)
				{
					throw new NullReferenceException("CreateWindow cannot return null");
				}
				window.DataContext = notification;
				window.Title = notification.Title;
				PrepareContentForWindow(notification, window);
			}
			else
			{
				window = CreateDefaultWindow(notification);
			}
			if (base.AssociatedObject != null)
			{
				window.Owner = Window.GetWindow(base.AssociatedObject);
			}
			if (WindowStyle != null)
			{
				window.Style = WindowStyle;
			}
			if (WindowStartupLocation.HasValue)
			{
				window.WindowStartupLocation = WindowStartupLocation.Value;
			}
			return window;
		}

		protected virtual void PrepareContentForWindow(INotification notification, Window wrapperWindow)
		{
			if (WindowContent != null)
			{
				wrapperWindow.Content = WindowContent;
			}
			else
			{
				if (!(WindowContentType != null))
				{
					return;
				}
				wrapperWindow.Content = ServiceLocator.Current.GetInstance(WindowContentType);
			}
			Action<IInteractionRequestAware> action = delegate(IInteractionRequestAware iira)
			{
				iira.Notification = notification;
				iira.FinishInteraction = delegate
				{
					wrapperWindow.Close();
				};
			};
			MvvmHelpers.ViewAndViewModelAction(wrapperWindow.Content, action);
		}

		protected virtual Window CreateWindow()
		{
			return new DefaultWindow();
		}

		protected Window CreateDefaultWindow(INotification notification)
		{
			Window window = null;
			if (notification is IConfirmation)
			{
				return new DefaultConfirmationWindow
				{
					Confirmation = (IConfirmation)notification
				};
			}
			return new DefaultNotificationWindow
			{
				Notification = notification
			};
		}
	}
}
