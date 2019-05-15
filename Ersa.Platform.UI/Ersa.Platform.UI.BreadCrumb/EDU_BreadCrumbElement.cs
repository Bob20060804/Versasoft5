using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Platform.UI.BreadCrumb
{
	public class EDU_BreadCrumbElement : Button
	{
		public static readonly DependencyProperty PRO_lstUnterElementeProperty;

		public ObservableCollection<EDC_BreadCrumbUnterElement> PRO_lstUnterElemente
		{
			get
			{
				return (ObservableCollection<EDC_BreadCrumbUnterElement>)GetValue(PRO_lstUnterElementeProperty);
			}
			set
			{
				SetValue(PRO_lstUnterElementeProperty, value);
			}
		}

		static EDU_BreadCrumbElement()
		{
			PRO_lstUnterElementeProperty = DependencyProperty.Register("PRO_lstUnterElemente", typeof(ObservableCollection<EDC_BreadCrumbUnterElement>), typeof(EDU_BreadCrumbElement));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_BreadCrumbElement), new FrameworkPropertyMetadata(typeof(EDU_BreadCrumbElement)));
		}
	}
}
