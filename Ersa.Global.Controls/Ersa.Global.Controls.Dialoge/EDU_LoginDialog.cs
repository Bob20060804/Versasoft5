using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_LoginDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strAnmeldenTextProperty = DependencyProperty.Register("PRO_strAnmeldenText", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strFehlendesRechtTextProperty = DependencyProperty.Register("PRO_strFehlendesRechtText", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strFehlerProperty = DependencyProperty.Register("PRO_strFehler", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strBenutzerNameTextProperty = DependencyProperty.Register("PRO_strBenutzerNameText", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strPasswortTextProperty = DependencyProperty.Register("PRO_strPasswortText", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strBenutzerNameEingabeProperty = DependencyProperty.Register("PRO_strBenutzerNameEingabe", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_strPasswortEingabeProperty = DependencyProperty.Register("PRO_strPasswortEingabe", typeof(string), typeof(EDU_LoginDialog));

		public static readonly DependencyProperty PRO_blnBenutzerNameFixiertProperty = DependencyProperty.Register("PRO_blnBenutzerNameFixiert", typeof(bool), typeof(EDU_LoginDialog));

		internal TextBox txtBenutzername;

		private bool _contentLoaded;

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

		public string PRO_strAnmeldenText
		{
			get
			{
				return (string)GetValue(PRO_strAnmeldenTextProperty);
			}
			set
			{
				SetValue(PRO_strAnmeldenTextProperty, value);
			}
		}

		public string PRO_strFehlendesRechtText
		{
			get
			{
				return (string)GetValue(PRO_strFehlendesRechtTextProperty);
			}
			set
			{
				SetValue(PRO_strFehlendesRechtTextProperty, value);
			}
		}

		public string PRO_strFehler
		{
			get
			{
				return (string)GetValue(PRO_strFehlerProperty);
			}
			set
			{
				SetValue(PRO_strFehlerProperty, value);
			}
		}

		public string PRO_strBenutzerNameText
		{
			get
			{
				return (string)GetValue(PRO_strBenutzerNameTextProperty);
			}
			set
			{
				SetValue(PRO_strBenutzerNameTextProperty, value);
			}
		}

		public string PRO_strPasswortText
		{
			get
			{
				return (string)GetValue(PRO_strPasswortTextProperty);
			}
			set
			{
				SetValue(PRO_strPasswortTextProperty, value);
			}
		}

		public string PRO_strBenutzerNameEingabe
		{
			get
			{
				return (string)GetValue(PRO_strBenutzerNameEingabeProperty);
			}
			set
			{
				SetValue(PRO_strBenutzerNameEingabeProperty, value);
			}
		}

		public string PRO_strPasswortEingabe
		{
			get
			{
				return (string)GetValue(PRO_strPasswortEingabeProperty);
			}
			set
			{
				SetValue(PRO_strPasswortEingabeProperty, value);
			}
		}

		public bool PRO_blnBenutzerNameFixiert
		{
			get
			{
				return (bool)GetValue(PRO_blnBenutzerNameFixiertProperty);
			}
			set
			{
				SetValue(PRO_blnBenutzerNameFixiertProperty, value);
			}
		}

		public EDU_LoginDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
		}

		private void SUB_AnmeldenGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
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
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_logindialog.xaml", UriKind.Relative);
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
				txtBenutzername = (TextBox)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
