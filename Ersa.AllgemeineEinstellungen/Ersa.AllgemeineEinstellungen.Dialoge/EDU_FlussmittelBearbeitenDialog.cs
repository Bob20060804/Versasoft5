using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.AllgemeineEinstellungen.Dialoge
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDU_FlussmittelBearbeitenDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strNameProperty = DependencyProperty.Register("PRO_strName", typeof(string), typeof(EDU_FlussmittelBearbeitenDialog), new FrameworkPropertyMetadata(SUB_OnEingabeChanged));

		public static readonly DependencyProperty PRO_strSpezifikationProperty = DependencyProperty.Register("PRO_strSpezifikation", typeof(string), typeof(EDU_FlussmittelBearbeitenDialog), new FrameworkPropertyMetadata(SUB_OnEingabeChanged));

		public static readonly DependencyProperty PRO_delValidierungProperty = DependencyProperty.Register("PRO_delValidierung", typeof(Func<string, string, string>), typeof(EDU_FlussmittelBearbeitenDialog));

		public static readonly DependencyProperty PRO_strValidierungsErgebnisProperty = DependencyProperty.Register("PRO_strValidierungsErgebnis", typeof(string), typeof(EDU_FlussmittelBearbeitenDialog));

		internal TextBox txtName;

		private bool _contentLoaded;

		public string PRO_strName
		{
			get
			{
				return (string)GetValue(PRO_strNameProperty);
			}
			set
			{
				SetValue(PRO_strNameProperty, value);
			}
		}

		public string PRO_strSpezifikation
		{
			get
			{
				return (string)GetValue(PRO_strSpezifikationProperty);
			}
			set
			{
				SetValue(PRO_strSpezifikationProperty, value);
			}
		}

		public Func<string, string, string> PRO_delValidierung
		{
			get
			{
				return (Func<string, string, string>)GetValue(PRO_delValidierungProperty);
			}
			set
			{
				SetValue(PRO_delValidierungProperty, value);
			}
		}

		public string PRO_strValidierungsErgebnis
		{
			get
			{
				return (string)GetValue(PRO_strValidierungsErgebnisProperty);
			}
			set
			{
				SetValue(PRO_strValidierungsErgebnisProperty, value);
			}
		}

		public EDU_FlussmittelBearbeitenDialog()
			: base(Application.Current.MainWindow)
		{
			InitializeComponent();
		}

		private static void SUB_OnEingabeChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_objSender as EDU_FlussmittelBearbeitenDialog)?.FUN_blnValidieren();
		}

		private void SUB_UebernehmenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (FUN_blnValidieren())
			{
				base.DialogResult = true;
				Close();
			}
		}

		private void SUB_AbbrechenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			base.DialogResult = false;
			Close();
		}

		private bool FUN_blnValidieren()
		{
			PRO_strValidierungsErgebnis = ((PRO_delValidierung == null) ? string.Empty : PRO_delValidierung(PRO_strName, PRO_strSpezifikation));
			return string.IsNullOrEmpty(PRO_strValidierungsErgebnis);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.AllgemeineEinstellungen;component/dialoge/edu_flussmittelbearbeitendialog.xaml", UriKind.Relative);
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
				txtName = (TextBox)target;
				break;
			case 2:
				((EDU_IconButton)target).Click += SUB_AbbrechenGeklickt;
				break;
			case 3:
				((EDU_IconButton)target).Click += SUB_UebernehmenGeklickt;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
