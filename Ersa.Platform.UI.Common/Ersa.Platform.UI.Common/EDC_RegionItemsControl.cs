using System.Windows;
using System.Windows.Controls;

namespace Ersa.Platform.UI.Common
{
	public class EDC_RegionItemsControl : ItemsControl
	{
		protected override bool IsItemItsOwnContainerOverride(object i_objItem)
		{
			return false;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject i_fdcElement, object i_objItem)
		{
			base.PrepareContainerForItemOverride(i_fdcElement, i_objItem);
			((ContentPresenter)i_fdcElement).ContentTemplate = base.ItemTemplate;
		}
	}
}
