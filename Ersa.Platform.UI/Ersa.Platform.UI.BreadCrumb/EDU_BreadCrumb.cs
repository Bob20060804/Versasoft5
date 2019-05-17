using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ersa.Platform.UI.BreadCrumb
{
	public class EDU_BreadCrumb : UserControl, IComponentConnector
	{
		public static readonly DependencyProperty PRO_cmdBreadCrumbEintragAusgewaehltProperty = DependencyProperty.Register("PRO_cmdBreadCrumbEintragAusgewaehlt", typeof(ICommand), typeof(EDU_BreadCrumb));

		public static readonly DependencyProperty PRO_lstBreadCrumbEintraegeProperty = DependencyProperty.Register("PRO_lstBreadCrumbEintraege", typeof(IList<EDC_BreadCrumbEintrag>), typeof(EDU_BreadCrumb));

		public static readonly DependencyProperty PRO_cmdUnterElementAusgewaehltProperty = DependencyProperty.Register("PRO_cmdUnterElementAusgewaehlt", typeof(ICommand), typeof(EDU_BreadCrumb));

		private bool _contentLoaded;

		public ObservableCollection<EDC_BreadCrumbEintrag> PRO_lstBreadCrumbEintraege
		{
			get
			{
				return (ObservableCollection<EDC_BreadCrumbEintrag>)GetValue(PRO_lstBreadCrumbEintraegeProperty);
			}
			set
			{
				SetValue(PRO_lstBreadCrumbEintraegeProperty, value);
			}
		}

		public ICommand PRO_cmdBreadCrumbEintragAusgewaehlt
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdBreadCrumbEintragAusgewaehltProperty);
			}
			set
			{
				SetValue(PRO_cmdBreadCrumbEintragAusgewaehltProperty, value);
			}
		}

		public ICommand PRO_cmdUnterElementAusgewaehlt
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdUnterElementAusgewaehltProperty);
			}
			set
			{
				SetValue(PRO_cmdUnterElementAusgewaehltProperty, value);
			}
		}

		public EDU_BreadCrumb()
		{
			PRO_lstBreadCrumbEintraege = new ObservableCollection<EDC_BreadCrumbEintrag>();
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/breadcrumb/edu_breadcrumb.xaml", UriKind.Relative);
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
