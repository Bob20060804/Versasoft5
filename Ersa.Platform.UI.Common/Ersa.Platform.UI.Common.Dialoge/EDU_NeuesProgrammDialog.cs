using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Common.Dialoge
{
	public class EDU_NeuesProgrammDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_lstBibliothekenProperty = DependencyProperty.Register("PRO_lstBibliotheken", typeof(IList<string>), typeof(EDU_NeuesProgrammDialog));

		public static readonly DependencyProperty PRO_strProgrammNameProperty = DependencyProperty.Register("PRO_strProgrammName", typeof(string), typeof(EDU_NeuesProgrammDialog), new FrameworkPropertyMetadata(SUB_OnEingabeChanged));

		public static readonly DependencyProperty PRO_strBibliothekNameProperty = DependencyProperty.Register("PRO_strBibliothekName", typeof(string), typeof(EDU_NeuesProgrammDialog), new FrameworkPropertyMetadata(SUB_OnEingabeChanged));

		public static readonly DependencyProperty PRO_delValidierungProperty = DependencyProperty.Register("PRO_delValidierung", typeof(Func<string, string, string>), typeof(EDU_NeuesProgrammDialog));

		public static readonly DependencyProperty PRO_strValidierungsErgebnisProperty = DependencyProperty.Register("PRO_strValidierungsErgebnis", typeof(string), typeof(EDU_NeuesProgrammDialog));

		internal TextBox txtProgrammName;

		private bool _contentLoaded;

		public IList<string> PRO_lstBibliotheken
		{
			get
			{
				return (IList<string>)GetValue(PRO_lstBibliothekenProperty);
			}
			set
			{
				SetValue(PRO_lstBibliothekenProperty, value);
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

		public string PRO_strBibliothekName
		{
			get
			{
				return (string)GetValue(PRO_strBibliothekNameProperty);
			}
			set
			{
				SetValue(PRO_strBibliothekNameProperty, value);
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

		public EDU_NeuesProgrammDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
			base.Loaded += delegate
			{
				if (string.IsNullOrEmpty(PRO_strBibliothekName) && PRO_lstBibliotheken.Any())
				{
					PRO_strBibliothekName = PRO_lstBibliotheken.First();
				}
			};
		}

		private static void SUB_OnEingabeChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_objSender as EDU_NeuesProgrammDialog)?.FUN_blnValidieren();
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
			PRO_strValidierungsErgebnis = ((PRO_delValidierung == null) ? string.Empty : PRO_delValidierung(PRO_strBibliothekName, PRO_strProgrammName));
			return string.IsNullOrEmpty(PRO_strValidierungsErgebnis);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI.Common;component/dialoge/edu_neuesprogrammdialog.xaml", UriKind.Relative);
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
				txtProgrammName = (TextBox)target;
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
