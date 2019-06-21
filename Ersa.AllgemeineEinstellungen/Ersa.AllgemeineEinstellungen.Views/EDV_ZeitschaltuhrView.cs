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
	public class EDV_ZeitschaltuhrView : UserControl, IComponentConnector
	{
		internal EDV_ZeitschaltuhrView edcZeitschaltuhrControl;

		internal CheckBox chkNdaAktiv;

		private bool _contentLoaded;

		[Import]
		public EDC_ZeitschaltuhrViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_ZeitschaltuhrViewModel;
			}
			set
			{
				value.SUB_ViewModelInitialisieren();
				base.DataContext = value;
			}
		}

		public EDV_ZeitschaltuhrView()
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
				Uri resourceLocator = new Uri("/Ersa.AllgemeineEinstellungen;component/views/edv_zeitschaltuhrview.xaml", UriKind.Relative);
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
				edcZeitschaltuhrControl = (EDV_ZeitschaltuhrView)target;
				break;
			case 2:
				chkNdaAktiv = (CheckBox)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
