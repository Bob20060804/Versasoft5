using Ersa.Global.Controls.Dialoge;
using Prism.Commands;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Dialoge
{
	public class EDU_UniversalDialog : EDU_Dialog, IComponentConnector
	{
		public static readonly DependencyProperty PRO_i32AusgewaehlterButtonProperty = DependencyProperty.Register("PRO_i32AusgewaehlterButton", typeof(int), typeof(EDU_UniversalDialog));

		public static readonly DependencyProperty PRO_cmdButtonGeklicktCommandProperty = DependencyProperty.Register("PRO_cmdButtonGeklicktCommand", typeof(ICommand), typeof(EDU_UniversalDialog));

		public static readonly DependencyProperty PROa_edcTextIconPaarProperty = DependencyProperty.Register("PROa_edcTextIconPaar", typeof(EDC_TextIconPaar[]), typeof(EDU_UniversalDialog));

		private bool _contentLoaded;

		public EDC_TextIconPaar[] PROa_edcTextIconPaar
		{
			get
			{
				return (EDC_TextIconPaar[])GetValue(PROa_edcTextIconPaarProperty);
			}
			set
			{
				SetValue(PROa_edcTextIconPaarProperty, value);
			}
		}

		public int PRO_i32AusgewaehlterButton
		{
			get
			{
				return (int)GetValue(PRO_i32AusgewaehlterButtonProperty);
			}
			set
			{
				SetValue(PRO_i32AusgewaehlterButtonProperty, value);
			}
		}

		public ICommand PRO_cmdButtonGeklicktCommand
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdButtonGeklicktCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdButtonGeklicktCommandProperty, value);
			}
		}

		public EDU_UniversalDialog(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			PRO_cmdButtonGeklicktCommand = new DelegateCommand<int?>(SUB_ButtonGeklickt);
			InitializeComponent();
		}

		private void SUB_ButtonGeklickt(int? i_i32ButtonIndex)
		{
			if (i_i32ButtonIndex.HasValue)
			{
				PRO_i32AusgewaehlterButton = i_i32ButtonIndex.Value;
				base.DialogResult = true;
			}
			else
			{
				base.DialogResult = false;
			}
			Close();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/dialoge/edu_universaldialog.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
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
