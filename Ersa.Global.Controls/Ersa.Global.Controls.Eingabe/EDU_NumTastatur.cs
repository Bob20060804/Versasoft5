using Ersa.Global.Controls.Dialoge;
using Ersa.Global.Controls.Eingabe.ViewModels;
using Ersa.Global.Controls.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Eingabe
{
	public class EDU_NumTastatur : EDU_Dialog, IComponentConnector
	{
		private bool _contentLoaded;

		public EDC_TastaturViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_TastaturViewModel;
			}
			set
			{
				value.PRO_cmdAbbrechen = new EDC_DelegateCommand(delegate
				{
					SUB_Close();
				});
				value.PRO_cmdUebernehmen = new EDC_DelegateCommand(delegate
				{
					SUB_Uebernehmen();
				});
				value.PRO_blnDialogResult = false;
				base.DataContext = value;
			}
		}

		public EDU_NumTastatur(Window i_fdcOwner)
			: base(i_fdcOwner)
		{
			InitializeComponent();
		}

		private void SUB_Uebernehmen()
		{
			PRO_edcViewModel.PRO_blnDialogResult = true;
			Close();
		}

		private void SUB_Close()
		{
			Close();
		}

		private void SUB_Dialog_KeyUp(object i_objSender, KeyEventArgs i_fdcKeyEventArgs)
		{
			string i_objCommandParameter;
			switch (i_fdcKeyEventArgs.Key)
			{
			default:
				return;
			case Key.D0:
			case Key.NumPad0:
				i_objCommandParameter = "0";
				break;
			case Key.D1:
			case Key.NumPad1:
				i_objCommandParameter = "1";
				break;
			case Key.D2:
			case Key.NumPad2:
				i_objCommandParameter = "2";
				break;
			case Key.D3:
			case Key.NumPad3:
				i_objCommandParameter = "3";
				break;
			case Key.D4:
			case Key.NumPad4:
				i_objCommandParameter = "4";
				break;
			case Key.D5:
			case Key.NumPad5:
				i_objCommandParameter = "5";
				break;
			case Key.D6:
			case Key.NumPad6:
				i_objCommandParameter = "6";
				break;
			case Key.D7:
			case Key.NumPad7:
				i_objCommandParameter = "7";
				break;
			case Key.D8:
			case Key.NumPad8:
				i_objCommandParameter = "8";
				break;
			case Key.D9:
			case Key.NumPad9:
				i_objCommandParameter = "9";
				break;
			case Key.Separator:
			case Key.Decimal:
			case Key.OemComma:
			case Key.OemPeriod:
				i_objCommandParameter = ".";
				break;
			case Key.Subtract:
			case Key.OemMinus:
				i_objCommandParameter = "-";
				break;
			case Key.Back:
				i_objCommandParameter = "back";
				break;
			case Key.Delete:
			case Key.OemClear:
				i_objCommandParameter = "C";
				break;
			}
			PRO_edcViewModel.PRO_cmdTextEingabe.SUB_Execute(i_objCommandParameter, this);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/eingabe/edu_numtastatur.xaml", UriKind.Relative);
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
