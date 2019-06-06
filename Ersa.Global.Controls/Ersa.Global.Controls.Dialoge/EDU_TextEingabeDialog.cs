using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_TextEingabeDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strBestaetigenTextProperty = DependencyProperty.Register("PRO_strBestaetigenText", typeof(string), typeof(EDU_TextEingabeDialog));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_TextEingabeDialog));

		public static readonly DependencyProperty PRO_strEingabeTextProperty = DependencyProperty.Register("PRO_strEingabeText", typeof(string), typeof(EDU_TextEingabeDialog), new FrameworkPropertyMetadata(SUB_OnEingabeTextChanged));

		public static readonly DependencyProperty PRO_blnReturnErlaubtProperty = DependencyProperty.Register("PRO_blnReturnErlaubt", typeof(bool), typeof(EDU_TextEingabeDialog));

		public static readonly DependencyProperty PRO_delValidierungProperty = DependencyProperty.Register("PRO_delValidierung", typeof(Func<string, string>), typeof(EDU_TextEingabeDialog));

		public static readonly DependencyProperty PRO_strValidierungsErgebnisProperty = DependencyProperty.Register("PRO_strValidierungsErgebnis", typeof(string), typeof(EDU_TextEingabeDialog));

		internal TextBox txtEingabe;

		internal TextBox txtEingabeMehrzeilig;

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

		public string PRO_strEingabeText
		{
			get
			{
				return (string)GetValue(PRO_strEingabeTextProperty);
			}
			set
			{
				SetValue(PRO_strEingabeTextProperty, value);
			}
		}

		public bool PRO_blnReturnErlaubt
		{
			get
			{
				return (bool)GetValue(PRO_blnReturnErlaubtProperty);
			}
			set
			{
				SetValue(PRO_blnReturnErlaubtProperty, value);
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

		public EDU_TextEingabeDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
			base.Loaded += delegate
			{
				if (PRO_blnReturnErlaubt)
				{
					txtEingabeMehrzeilig.SelectAll();
					txtEingabeMehrzeilig.Focus();
				}
				else
				{
					txtEingabe.SelectAll();
					txtEingabe.Focus();
				}
			};
		}

		public EDU_TextEingabeDialog(Window i_fdcOwner, string i_strInitialierText)
			: this(i_fdcOwner)
		{
			PRO_strEingabeText = i_strInitialierText;
		}

		private static void SUB_OnEingabeTextChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_objSender as EDU_TextEingabeDialog)?.FUN_blnValidieren();
		}

		private void SUB_PositivGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (FUN_blnValidieren())
			{
				base.DialogResult = true;
				Close();
			}
		}

		private bool FUN_blnValidieren()
		{
			PRO_strValidierungsErgebnis = ((PRO_delValidierung == null) ? string.Empty : PRO_delValidierung(PRO_strEingabeText));
			return string.IsNullOrEmpty(PRO_strValidierungsErgebnis);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_texteingabedialog.xaml", UriKind.Relative);
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
				txtEingabe = (TextBox)target;
				break;
			case 2:
				txtEingabeMehrzeilig = (TextBox)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
