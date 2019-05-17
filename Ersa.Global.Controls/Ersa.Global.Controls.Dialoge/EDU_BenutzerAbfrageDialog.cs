using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_BenutzerAbfrageDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strBestaetigenPositivTextProperty = DependencyProperty.Register("PRO_strBestaetigenPositivText", typeof(string), typeof(EDU_BenutzerAbfrageDialog));

		public static readonly DependencyProperty PRO_strBestaetigenNegativTextProperty = DependencyProperty.Register("PRO_strBestaetigenNegativText", typeof(string), typeof(EDU_BenutzerAbfrageDialog));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_BenutzerAbfrageDialog));

		public static readonly DependencyProperty PRO_objTemplateProperty = DependencyProperty.Register("PRO_objTemplate", typeof(DataTemplate), typeof(EDU_BenutzerAbfrageDialog));

		public static readonly DependencyProperty PRO_blnIstNegativeAuswahlSichtbarProperty = DependencyProperty.Register("PRO_blnIstNegativeAuswahlSichtbar", typeof(bool), typeof(EDU_BenutzerAbfrageDialog));

		public static readonly DependencyProperty PRO_blnIstAbbrechenSichtbarProperty = DependencyProperty.Register("PRO_blnIstAbbrechenSichtbar", typeof(bool), typeof(EDU_BenutzerAbfrageDialog), new UIPropertyMetadata(true));

		private bool _contentLoaded;

		public string PRO_strBestaetigenPositivText
		{
			get
			{
				return (string)GetValue(PRO_strBestaetigenPositivTextProperty);
			}
			set
			{
				SetValue(PRO_strBestaetigenPositivTextProperty, value);
			}
		}

		public string PRO_strBestaetigenNegativText
		{
			get
			{
				return (string)GetValue(PRO_strBestaetigenNegativTextProperty);
			}
			set
			{
				SetValue(PRO_strBestaetigenNegativTextProperty, value);
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

		public bool PRO_blnIstNegativeAuswahlSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnIstNegativeAuswahlSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnIstNegativeAuswahlSichtbarProperty, value);
			}
		}

		public bool PRO_blnIstAbbrechenSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAbbrechenSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnIstAbbrechenSichtbarProperty, value);
			}
		}

		public bool PRO_blnWurdeNegativBestatigt
		{
			get;
			set;
		}

		public EDU_BenutzerAbfrageDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
		}

		private void SUB_PositivGeklickt(object sender, RoutedEventArgs e)
		{
			PRO_blnWurdeNegativBestatigt = false;
			base.DialogResult = true;
			Close();
		}

		private void SUB_NegativGeklickt(object sender, RoutedEventArgs e)
		{
			PRO_blnWurdeNegativBestatigt = true;
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
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_benutzerabfragedialog.xaml", UriKind.Relative);
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
			_contentLoaded = true;
		}
	}
}
