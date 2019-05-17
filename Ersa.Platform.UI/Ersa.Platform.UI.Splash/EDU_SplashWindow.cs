using Ersa.Platform.UI.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Splash
{
	public class EDU_SplashWindow : Window, IComponentConnector
	{
		private bool _contentLoaded;

		public EDU_SplashWindow(EDC_SplashScreenViewModel i_strViewModel)
		{
			InitializeComponent();
			base.DataContext = i_strViewModel;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/splash/edu_splashwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
}
