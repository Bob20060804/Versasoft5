using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using Ersa.Platform.UI.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Dialoge
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDU_LoetprogrammAuswahlDialog : EDU_Dialog, IComponentConnector
	{
		private bool _contentLoaded;

		[Import]
		public EDC_ProgrammAuswahlViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_ProgrammAuswahlViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public long? PRO_i64PrgId
		{
			get;
			set;
		}

		public EDU_LoetprogrammAuswahlDialog()
			: base(Application.Current.MainWindow)
		{
			InitializeComponent();
		}

		private void SUB_AbbrechenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			PRO_i64PrgId = null;
			base.DialogResult = false;
			Close();
		}

		private void SUB_UebernehmenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			PRO_i64PrgId = PRO_edcViewModel.PRO_edcAusgewaehltesProgramm?.PRO_i64Id;
			base.DialogResult = true;
			Close();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/dialoge/edu_loetprogrammauswahldialog.xaml", UriKind.Relative);
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
			switch (connectionId)
			{
			case 1:
				((EDU_IconButton)target).Click += SUB_AbbrechenGeklickt;
				break;
			case 2:
				((EDU_IconButton)target).Click += SUB_UebernehmenGeklickt;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
