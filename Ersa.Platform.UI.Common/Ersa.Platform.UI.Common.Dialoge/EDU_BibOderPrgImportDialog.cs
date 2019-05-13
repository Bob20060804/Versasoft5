using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using Ersa.Platform.Infrastructure.Prism;
using Ersa.Platform.UI.Common.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Common.Dialoge
{
	public class EDU_BibOderPrgImportDialog : EDU_Dialog, IComponentConnector
	{
		private bool _contentLoaded;

		public EDC_BibOderPrgImportDialogViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_BibOderPrgImportDialogViewModel;
			}
			private set
			{
				base.DataContext = value;
			}
		}

		public EDU_BibOderPrgImportDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
			PRO_edcViewModel = EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<EDC_BibOderPrgImportDialogViewModel>();
		}

		private void SUB_SchliessenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			base.DialogResult = true;
			Close();
		}

		private void SUB_AbbrechenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			base.DialogResult = false;
			Close();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI.Common;component/dialoge/edu_biboderprgimportdialog.xaml", UriKind.Relative);
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
				((EDU_IconButton)target).Click += SUB_AbbrechenGeklickt;
				break;
			case 2:
				((EDU_IconButton)target).Click += SUB_SchliessenGeklickt;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
