using Ersa.AllgemeineEinstellungen.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.AllgemeineEinstellungen.Views
{
	[Export]
	public class EDV_BetriebsmittelVerwaltungView : UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		[Import]
		public EDC_BetriebsmittelVerwaltungViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_BetriebsmittelVerwaltungViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public EDV_BetriebsmittelVerwaltungView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.AllgemeineEinstellungen;component/views/edv_betriebsmittelverwaltungview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
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
