using Ersa.Global.Controls;
using Ersa.Global.Controls.Dialoge;
using Ersa.Platform.UI.Benutzereingabe;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Dialoge
{
	public class EDU_OptionDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strBestaetigenTextProperty = DependencyProperty.Register("PRO_strBestaetigenText", typeof(string), typeof(EDU_OptionDialog));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_OptionDialog));

		public static readonly DependencyProperty PRO_lstOptionListeProperty = DependencyProperty.Register("PRO_lstOptionListe", typeof(IList<EDC_OptionEingabe>), typeof(EDU_OptionDialog), new PropertyMetadata(SUB_OptionListeGeaendert));

		public static readonly DependencyProperty PRO_blnAbbrechenMoeglichProperty = DependencyProperty.Register("PRO_blnAbbrechenMoeglich", typeof(bool), typeof(EDU_OptionDialog));

		private bool _contentLoaded;

		public string PRO_strBestaetigenText
		{
			get
			{
				return (string)GetValue(PRO_strBestaetigenTextProperty);
			}
			set
			{
				SetValue(PRO_strBestaetigenTextProperty, value);
			}
		}

		public string PRO_strAbbrechenText
		{
			get
			{
				return (string)GetValue(PRO_strAbbrechenTextProperty);
			}
			set
			{
				SetValue(PRO_strAbbrechenTextProperty, value);
			}
		}

		public IList<EDC_OptionEingabe> PRO_lstOptionListe
		{
			get
			{
				return (IList<EDC_OptionEingabe>)GetValue(PRO_lstOptionListeProperty);
			}
			set
			{
				SetValue(PRO_lstOptionListeProperty, value);
			}
		}

		public bool PRO_blnAbbrechenMoeglich
		{
			get
			{
				return (bool)GetValue(PRO_blnAbbrechenMoeglichProperty);
			}
			set
			{
				SetValue(PRO_blnAbbrechenMoeglichProperty, value);
			}
		}

		public EDU_OptionDialog(Window i_fdcOwner, bool i_blnDialogSchliessenMitEscapeUnterbinden)
			: base(i_fdcOwner, i_blnDialogSchliessenMitEscapeUnterbinden)
		{
			InitializeComponent();
		}

		private static void SUB_OptionListeGeaendert(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_OptionDialog eDU_OptionDialog = i_objSender as EDU_OptionDialog;
			if (eDU_OptionDialog != null && eDU_OptionDialog.PRO_lstOptionListe.Any() && !eDU_OptionDialog.PRO_lstOptionListe.Any((EDC_OptionEingabe i_edcOption) => i_edcOption.PRO_blnAusgewaehlt))
			{
				eDU_OptionDialog.PRO_lstOptionListe.First().PRO_blnAusgewaehlt = true;
			}
		}

		private void SUB_BestaetigenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (!PRO_lstOptionListe.Any() || PRO_lstOptionListe.Any((EDC_OptionEingabe i_edcOption) => i_edcOption.PRO_blnAusgewaehlt))
			{
				base.DialogResult = true;
				Close();
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/dialoge/edu_optiondialog.xaml", UriKind.Relative);
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
				((EDU_IconButton)target).Click += SUB_BestaetigenGeklickt;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
