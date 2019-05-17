using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Dialoge
{
	public class EDU_MeldungDetailsDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_strAnwendungWeiterAusfuehrenTextProperty = DependencyProperty.Register("PRO_strAnwendungWeiterAusfuehrenText", typeof(string), typeof(EDU_FehlerDialog));

		public static readonly DependencyProperty PRO_strInZwischenablageKopierenTextProperty = DependencyProperty.Register("PRO_strInZwischenablageKopierenText", typeof(string), typeof(EDU_FehlerDialog), new PropertyMetadata((object)null));

		private bool _contentLoaded;

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

		public EDU_MeldungDetailsDialog(Window i_fdcOwner, Action i_delServiceFallErstellen = null)
			: base(i_fdcOwner)
		{
			InitializeComponent();
		}

		private void SUB_WeiterAusfuehrenGeklickt(object sender, RoutedEventArgs e)
		{
			base.DialogResult = true;
			Close();
		}

		private void SUB_InZwischenablageKopieren(object i_edcSender, RoutedEventArgs i_edcE)
		{
			Clipboard.SetText(base.PRO_strText);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/dialoge/edu_meldungdetailsdialog.xaml", UriKind.Relative);
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
