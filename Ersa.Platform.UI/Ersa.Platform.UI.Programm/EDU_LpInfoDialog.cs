using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Programm
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDU_LpInfoDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_i64ProgrammIdProperty = DependencyProperty.Register("PRO_i64ProgrammId", typeof(long), typeof(EDU_LpInfoDialog), new PropertyMetadata(0L));

		public static readonly DependencyProperty PRO_strProgrammNameProperty = DependencyProperty.Register("PRO_strProgrammName", typeof(string), typeof(EDU_LpInfoDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_strBibliotheksNameProperty = DependencyProperty.Register("PRO_strBibliotheksName", typeof(string), typeof(EDU_LpInfoDialog), new PropertyMetadata((object)null));

		private bool _contentLoaded;

		public long PRO_i64ProgrammId
		{
			get
			{
				return (long)GetValue(PRO_i64ProgrammIdProperty);
			}
			set
			{
				SetValue(PRO_i64ProgrammIdProperty, value);
			}
		}

		public string PRO_strProgrammName
		{
			get
			{
				return (string)GetValue(PRO_strProgrammNameProperty);
			}
			set
			{
				SetValue(PRO_strProgrammNameProperty, value);
			}
		}

		public string PRO_strBibliotheksName
		{
			get
			{
				return (string)GetValue(PRO_strBibliotheksNameProperty);
			}
			set
			{
				SetValue(PRO_strBibliotheksNameProperty, value);
			}
		}

		public EDU_LpInfoDialog()
			: base(Application.Current.MainWindow)
		{
			InitializeComponent();
		}

		private void SUB_SchliessenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
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
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/programm/edu_lpinfodialog.xaml", UriKind.Relative);
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
				((EDU_IconButton)target).Click += SUB_SchliessenGeklickt;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
