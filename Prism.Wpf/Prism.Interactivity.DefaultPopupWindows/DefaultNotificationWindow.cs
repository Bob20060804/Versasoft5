using Prism.Interactivity.InteractionRequest;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Prism.Interactivity.DefaultPopupWindows
{
	public class DefaultNotificationWindow : Window, IComponentConnector
	{
		internal Grid LayoutRoot;

		internal Button OKButton;

		private bool _contentLoaded;

		public INotification Notification
		{
			get
			{
				return base.DataContext as INotification;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public DefaultNotificationWindow()
		{
			InitializeComponent();
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Prism.Wpf;component/interactivity/defaultpopupwindows/defaultnotificationwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				LayoutRoot = (Grid)target;
				break;
			case 2:
				OKButton = (Button)target;
				OKButton.Click += OKButton_Click;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
