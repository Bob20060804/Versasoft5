using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_KennwortAendernDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_blnAltesKennwortEingebenProperty = DependencyProperty.Register("PRO_blnAltesKennwortEingeben", typeof(bool), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strAltesKennwortTextProperty = DependencyProperty.Register("PRO_strAltesKennwortText", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strNeuesKennwortTextProperty = DependencyProperty.Register("PRO_strNeuesKennwortText", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strNeuesKennwortWiederholungTextProperty = DependencyProperty.Register("PRO_strNeuesKennwortWiederholungText", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strAltesKennwortProperty = DependencyProperty.Register("PRO_strAltesKennwort", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strNeuesKennwortProperty = DependencyProperty.Register("PRO_strNeuesKennwort", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strNeuesKennwortWiederholungProperty = DependencyProperty.Register("PRO_strNeuesKennwortWiederholung", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strAendernTextProperty = DependencyProperty.Register("PRO_strAendernText", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_KennwortAendernDialog));

		public static readonly DependencyProperty PRO_strFehlerProperty = DependencyProperty.Register("PRO_strFehler", typeof(string), typeof(EDU_KennwortAendernDialog));

		internal PasswordBox pwbAltesKennwort;

		private bool _contentLoaded;

		public bool PRO_blnAltesKennwortEingeben
		{
			get
			{
				return (bool)GetValue(PRO_blnAltesKennwortEingebenProperty);
			}
			set
			{
				SetValue(PRO_blnAltesKennwortEingebenProperty, value);
			}
		}

		public string PRO_strAltesKennwortText
		{
			get
			{
				return (string)GetValue(PRO_strAltesKennwortTextProperty);
			}
			set
			{
				SetValue(PRO_strAltesKennwortTextProperty, value);
			}
		}

		public string PRO_strNeuesKennwortText
		{
			get
			{
				return (string)GetValue(PRO_strNeuesKennwortTextProperty);
			}
			set
			{
				SetValue(PRO_strNeuesKennwortTextProperty, value);
			}
		}

		public string PRO_strNeuesKennwortWiederholungText
		{
			get
			{
				return (string)GetValue(PRO_strNeuesKennwortWiederholungTextProperty);
			}
			set
			{
				SetValue(PRO_strNeuesKennwortWiederholungTextProperty, value);
			}
		}

		public string PRO_strAltesKennwort
		{
			get
			{
				return (string)GetValue(PRO_strAltesKennwortProperty);
			}
			set
			{
				SetValue(PRO_strAltesKennwortProperty, value);
			}
		}

		public string PRO_strNeuesKennwort
		{
			get
			{
				return (string)GetValue(PRO_strNeuesKennwortProperty);
			}
			set
			{
				SetValue(PRO_strNeuesKennwortProperty, value);
			}
		}

		public string PRO_strNeuesKennwortWiederholung
		{
			get
			{
				return (string)GetValue(PRO_strNeuesKennwortWiederholungProperty);
			}
			set
			{
				SetValue(PRO_strNeuesKennwortWiederholungProperty, value);
			}
		}

		public string PRO_strAendernText
		{
			get
			{
				return (string)GetValue(PRO_strAendernTextProperty);
			}
			set
			{
				SetValue(PRO_strAendernTextProperty, value);
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

		public EDU_KennwortAendernDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
		}

		private void SUB_AendernGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
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
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_kennwortaenderndialog.xaml", UriKind.Relative);
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
				pwbAltesKennwort = (PasswordBox)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
