using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_AuswahlDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strBestaetigenTextProperty = DependencyProperty.Register("PRO_strBestaetigenText", typeof(string), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_lstAuswahlListeProperty = DependencyProperty.Register("PRO_lstAuswahlListe", typeof(IList<string>), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_strAuswahlProperty = DependencyProperty.Register("PRO_strAuswahl", typeof(string), typeof(EDU_AuswahlDialog), new FrameworkPropertyMetadata(SUB_OnAuswahlChanged));

		public static readonly DependencyProperty PRO_delValidierungProperty = DependencyProperty.Register("PRO_delValidierung", typeof(Func<string, string>), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_blnValidierungVorhandenProperty = DependencyProperty.Register("PRO_blnValidierungVorhanden", typeof(bool), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_strValidierungsErgebnisProperty = DependencyProperty.Register("PRO_strValidierungsErgebnis", typeof(string), typeof(EDU_AuswahlDialog));

		public static readonly DependencyProperty PRO_blnBestaetigenMoeglichProperty = DependencyProperty.Register("PRO_blnBestaetigenMoeglich", typeof(bool), typeof(EDU_AuswahlDialog));

		private readonly string m_strInitialeAuswahl;

		internal ListBox lstAuswahl;

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

		public IList<string> PRO_lstAuswahlListe
		{
			get
			{
				return (IList<string>)GetValue(PRO_lstAuswahlListeProperty);
			}
			set
			{
				SetValue(PRO_lstAuswahlListeProperty, value);
			}
		}

		public string PRO_strAuswahl
		{
			get
			{
				return (string)GetValue(PRO_strAuswahlProperty);
			}
			set
			{
				SetValue(PRO_strAuswahlProperty, value);
			}
		}

		public Func<string, string> PRO_delValidierung
		{
			get
			{
				return (Func<string, string>)GetValue(PRO_delValidierungProperty);
			}
			set
			{
				SetValue(PRO_delValidierungProperty, value);
			}
		}

		public bool PRO_blnValidierungVorhanden
		{
			get
			{
				return (bool)GetValue(PRO_blnValidierungVorhandenProperty);
			}
			set
			{
				SetValue(PRO_blnValidierungVorhandenProperty, value);
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

		public bool PRO_blnBestaetigenMoeglich
		{
			get
			{
				return (bool)GetValue(PRO_blnBestaetigenMoeglichProperty);
			}
			set
			{
				SetValue(PRO_blnBestaetigenMoeglichProperty, value);
			}
		}

		public bool PRO_blnBestaetigenVerhindernOhneAenderung
		{
			get;
			set;
		}

		public Func<string, string> PRO_delBestaetigenText
		{
			get;
			set;
		}

		public EDU_AuswahlDialog(Window i_fdcOwner, string i_strInitialeAuswahl)
			: base(i_fdcOwner)
		{
			m_strInitialeAuswahl = i_strInitialeAuswahl;
			PRO_strValidierungsErgebnis = string.Empty;
			PRO_blnBestaetigenMoeglich = true;
			InitializeComponent();
			base.Loaded += delegate
			{
				PRO_blnValidierungVorhanden = (PRO_delValidierung != null);
				if (!string.IsNullOrEmpty(m_strInitialeAuswahl))
				{
					PRO_strAuswahl = m_strInitialeAuswahl;
					lstAuswahl.ScrollIntoView(PRO_strAuswahl);
				}
				SUB_GueltigeAuswahlPruefen();
			};
		}

		private static void SUB_OnAuswahlChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_objSender as EDU_AuswahlDialog)?.SUB_AuswahlAenderungVerarbeiten();
		}

		private void SUB_BestaetigenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (FUN_blnValidieren())
			{
				base.DialogResult = true;
				Close();
			}
		}

		private void SUB_AuswahlAenderungVerarbeiten()
		{
			FUN_blnValidieren();
			SUB_GueltigeAuswahlPruefen();
		}

		private void SUB_GueltigeAuswahlPruefen()
		{
			if (string.IsNullOrEmpty(PRO_strAuswahl) || (PRO_blnBestaetigenVerhindernOhneAenderung && PRO_strAuswahl == m_strInitialeAuswahl))
			{
				PRO_blnBestaetigenMoeglich = false;
			}
			SUB_BestaetigenTextSetzen();
		}

		private bool FUN_blnValidieren()
		{
			PRO_strValidierungsErgebnis = ((PRO_delValidierung == null) ? string.Empty : PRO_delValidierung(PRO_strAuswahl));
			PRO_blnBestaetigenMoeglich = string.IsNullOrEmpty(PRO_strValidierungsErgebnis);
			return string.IsNullOrEmpty(PRO_strValidierungsErgebnis);
		}

		private void SUB_BestaetigenTextSetzen()
		{
			if (PRO_delBestaetigenText != null)
			{
				PRO_strBestaetigenText = PRO_delBestaetigenText(PRO_strAuswahl);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_auswahldialog.xaml", UriKind.Relative);
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
			if (connectionId == 1)
			{
				lstAuswahl = (ListBox)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
