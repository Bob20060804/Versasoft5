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
	public class DefaultConfirmationWindow : Window, IComponentConnector
	{
		internal Grid LayoutRoot;

		internal Button OkButton;

		internal Button CancelButton;

		private bool _contentLoaded;

		public IConfirmation Confirmation
		{
			get
			{
				return base.DataContext as IConfirmation;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public DefaultConfirmationWindow()
		{
			InitializeComponent();
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			Confirmation.Confirmed = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Confirmation.Confirmed = false;
			Close();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Prism.Wpf;component/interactivity/defaultpopupwindows/defaultconfirmationwindow.xaml", UriKind.Relative);
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
				OkButton = (Button)target;
				OkButton.Click += OkButton_Click;
				break;
			case 3:
				CancelButton = (Button)target;
				CancelButton.Click += CancelButton_Click;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
