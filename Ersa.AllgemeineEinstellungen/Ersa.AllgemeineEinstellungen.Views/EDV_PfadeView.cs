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
	public class EDV_PfadeView : UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		[Import]
		public EDC_PfadeViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_PfadeViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public EDV_PfadeView()
		{
			InitializeComponent();
		}

		private void SUB_SelektionGeaendert(object i_objSender, SelectionChangedEventArgs i_fdcArgs)
		{
			i_fdcArgs.Handled = true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.AllgemeineEinstellungen;component/views/edv_pfadeview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((ComboBox)target).SelectionChanged += SUB_SelektionGeaendert;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
