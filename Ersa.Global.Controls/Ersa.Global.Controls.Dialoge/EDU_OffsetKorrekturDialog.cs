using Ersa.Global.Common.Helper;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_OffsetKorrekturDialog : EDU_Dialog, IDataErrorInfo, IComponentConnector
	{
		public static readonly DependencyProperty ms_fdcBestaetigenTextProperty = DependencyProperty.Register("PRO_strBestaetigenText", typeof(string), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcValidierungsFehlerTextProperty = DependencyProperty.Register("PRO_strValidierungsFehlerText", typeof(string), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcOffsetTextProperty = DependencyProperty.Register("PRO_strOffsetText", typeof(string), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcOffsetAbsolutVerwendenTextProperty = DependencyProperty.Register("PRO_strOffsetAbsolutVerwendenText", typeof(string), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcOffsetAbsolutVerwendenProperty = DependencyProperty.Register("PRO_blnOffsetAbsolutVerwenden", typeof(bool), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcOffsetProperty = DependencyProperty.Register("PRO_dblOffset", typeof(double), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcStrOffsetProperty = DependencyProperty.Register("PRO_strOffset", typeof(string), typeof(EDU_OffsetKorrekturDialog), new PropertyMetadata("0"));

		public static readonly DependencyProperty ms_fdcOhneVirtKeyboardProperty = DependencyProperty.Register("PRO_blnOhneVirtKeyboard", typeof(bool), typeof(EDU_OffsetKorrekturDialog));

		public static readonly DependencyProperty ms_fdcKeinFehler = DependencyProperty.Register("PRO_blnKeinFehler", typeof(bool), typeof(EDU_OffsetKorrekturDialog));

		internal TextBox txtBoxOffsetOhneVirtKeyboard;

		private bool _contentLoaded;

		public string PRO_strBestaetigenText
		{
			get
			{
				return (string)GetValue(ms_fdcBestaetigenTextProperty);
			}
			set
			{
				SetValue(ms_fdcBestaetigenTextProperty, value);
			}
		}

		public string PRO_strAbbrechenText
		{
			get
			{
				return (string)GetValue(ms_fdcAbbrechenTextProperty);
			}
			set
			{
				SetValue(ms_fdcAbbrechenTextProperty, value);
			}
		}

		public string PRO_strValidierungsFehlerText
		{
			get
			{
				return (string)GetValue(ms_fdcValidierungsFehlerTextProperty);
			}
			set
			{
				SetValue(ms_fdcValidierungsFehlerTextProperty, value);
			}
		}

		public string PRO_strOffsetText
		{
			get
			{
				return (string)GetValue(ms_fdcOffsetTextProperty);
			}
			set
			{
				SetValue(ms_fdcOffsetTextProperty, value);
			}
		}

		public string PRO_strOffsetAbsolutVerwendenText
		{
			get
			{
				return (string)GetValue(ms_fdcOffsetAbsolutVerwendenTextProperty);
			}
			set
			{
				SetValue(ms_fdcOffsetAbsolutVerwendenTextProperty, value);
			}
		}

		public bool PRO_blnOffsetAbsolutVerwenden
		{
			get
			{
				return (bool)GetValue(ms_fdcOffsetAbsolutVerwendenProperty);
			}
			set
			{
				SetValue(ms_fdcOffsetAbsolutVerwendenProperty, value);
			}
		}

		public double PRO_dblOffset
		{
			get
			{
				return (double)GetValue(ms_fdcOffsetProperty);
			}
			set
			{
				SetValue(ms_fdcOffsetProperty, value);
			}
		}

		public string PRO_strOffset
		{
			get
			{
				return (string)GetValue(ms_fdcStrOffsetProperty);
			}
			set
			{
				SetValue(ms_fdcStrOffsetProperty, value);
			}
		}

		public bool PRO_blnOhneVirtKeyboard
		{
			get
			{
				return (bool)GetValue(ms_fdcOhneVirtKeyboardProperty);
			}
			set
			{
				SetValue(ms_fdcOhneVirtKeyboardProperty, value);
			}
		}

		public bool PRO_blnKeinFehler
		{
			get
			{
				return (bool)GetValue(ms_fdcKeinFehler);
			}
			set
			{
				SetValue(ms_fdcKeinFehler, value);
			}
		}

		public string Error => string.Empty;

		public string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_strOffset" && !EDC_ZahlenFormatHelfer.FUN_blnIstZahl(PRO_strOffset))
				{
					PRO_blnKeinFehler = false;
					return PRO_strValidierungsFehlerText;
				}
				PRO_blnKeinFehler = true;
				return string.Empty;
			}
		}

		public EDU_OffsetKorrekturDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			PRO_strOffset = "0";
			base.Loaded += delegate
			{
				txtBoxOffsetOhneVirtKeyboard.SelectAll();
			};
			InitializeComponent();
		}

		public EDU_OffsetKorrekturDialog()
		{
			InitializeComponent();
		}

		private void SUB_OkGeklickt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (PRO_blnOhneVirtKeyboard)
			{
				PRO_dblOffset = EDC_ZahlenFormatHelfer.FUN_dblNachkommaWert(PRO_strOffset);
			}
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
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_offsetkorrekturdialog.xaml", UriKind.Relative);
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
				txtBoxOffsetOhneVirtKeyboard = (TextBox)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
