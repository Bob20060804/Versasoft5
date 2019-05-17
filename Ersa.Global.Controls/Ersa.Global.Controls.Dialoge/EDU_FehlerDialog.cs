using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_FehlerDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_fdcExceptionProperty = DependencyProperty.Register("PRO_fdcException", typeof(Exception), typeof(EDU_FehlerDialog));

		public static readonly DependencyProperty PRO_strAnwendungWeiterAusfuehrenTextProperty = DependencyProperty.Register("PRO_strAnwendungWeiterAusfuehrenText", typeof(string), typeof(EDU_FehlerDialog));

		public static readonly DependencyProperty PRO_strAnwendungBeendenTextProperty = DependencyProperty.Register("PRO_strAnwendungBeendenText", typeof(string), typeof(EDU_FehlerDialog));

		public static readonly DependencyProperty PRO_strInZwischenablageKopierenTextProperty = DependencyProperty.Register("PRO_strInZwischenablageKopierenText", typeof(string), typeof(EDU_FehlerDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_strServicefallAnlegenTextProperty = DependencyProperty.Register("PRO_strServicefallAnlegenText", typeof(string), typeof(EDU_FehlerDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_blnIstServicefallAnlegenSichtbarProperty = DependencyProperty.Register("PRO_blnIstServicefallAnlegenSichtbar", typeof(bool), typeof(EDU_FehlerDialog), new UIPropertyMetadata(false));

		public static readonly DependencyProperty PRO_blnIstServicefallAnlegenEnabledProperty = DependencyProperty.Register("PRO_blnIstServicefallAnlegenEnabled", typeof(bool), typeof(EDU_FehlerDialog), new UIPropertyMetadata(true));

		private readonly Action m_delServiceFallErstellen;

		private bool _contentLoaded;

		public Exception PRO_fdcException
		{
			get
			{
				return (Exception)GetValue(PRO_fdcExceptionProperty);
			}
			set
			{
				SetValue(PRO_fdcExceptionProperty, value);
			}
		}

		public string PRO_strAnwendungWeiterAusfuehrenText
		{
			get
			{
				return (string)GetValue(PRO_strAnwendungWeiterAusfuehrenTextProperty);
			}
			set
			{
				SetValue(PRO_strAnwendungWeiterAusfuehrenTextProperty, value);
			}
		}

		public string PRO_strInZwischenablageKopierenText
		{
			get
			{
				return (string)GetValue(PRO_strInZwischenablageKopierenTextProperty);
			}
			set
			{
				SetValue(PRO_strInZwischenablageKopierenTextProperty, value);
			}
		}

		public string PRO_strAnwendungBeendenText
		{
			get
			{
				return (string)GetValue(PRO_strAnwendungBeendenTextProperty);
			}
			set
			{
				SetValue(PRO_strAnwendungBeendenTextProperty, value);
			}
		}

		public string PRO_strServicefallAnlegenText
		{
			get
			{
				return (string)GetValue(PRO_strServicefallAnlegenTextProperty);
			}
			set
			{
				SetValue(PRO_strServicefallAnlegenTextProperty, value);
			}
		}

		public bool PRO_blnIstServicefallAnlegenSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnIstServicefallAnlegenSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnIstServicefallAnlegenSichtbarProperty, value);
			}
		}

		public bool PRO_blnIstServicefallAnlegenEnabled
		{
			get
			{
				return (bool)GetValue(PRO_blnIstServicefallAnlegenEnabledProperty);
			}
			set
			{
				SetValue(PRO_blnIstServicefallAnlegenEnabledProperty, value);
			}
		}

		public EDU_FehlerDialog(Window i_fdcOwner, Action i_delServiceFallErstellen = null)
			: base(i_fdcOwner)
		{
			m_delServiceFallErstellen = i_delServiceFallErstellen;
			InitializeComponent();
		}

		private void SUB_BeendenGeklickt(object sender, RoutedEventArgs e)
		{
			base.DialogResult = false;
			Close();
		}

		private void SUB_WeiterAusfuehrenGeklickt(object sender, RoutedEventArgs e)
		{
			base.DialogResult = true;
			Close();
		}

		private void SUB_InZwischenablageKopieren(object i_edcSender, RoutedEventArgs i_edcE)
		{
			Clipboard.SetText(PRO_fdcException.ToString());
		}

		private void SUB_ServicefallErstellen(object i_edcSender, RoutedEventArgs i_edcE)
		{
			m_delServiceFallErstellen?.Invoke();
			PRO_blnIstServicefallAnlegenEnabled = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_fehlerdialog.xaml", UriKind.Relative);
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
