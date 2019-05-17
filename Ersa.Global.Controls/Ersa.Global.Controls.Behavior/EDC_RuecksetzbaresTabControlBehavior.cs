using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_RuecksetzbaresTabControlBehavior : Behavior<TabControl>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.Loaded += SUB_TabControlGeladen;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.Loaded -= SUB_TabControlGeladen;
			foreach (TabItem item in base.AssociatedObject.Items.OfType<TabItem>())
			{
				item.IsVisibleChanged -= SUB_TabItemSichtbarkeitGeaendert;
			}
			base.OnDetaching();
		}

		private void SUB_TabControlGeladen(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			if (base.AssociatedObject.Items.Count > 0)
			{
				base.AssociatedObject.SelectedIndex = 0;
			}
			foreach (TabItem item in base.AssociatedObject.Items.OfType<TabItem>())
			{
				item.IsVisibleChanged += SUB_TabItemSichtbarkeitGeaendert;
			}
		}

		private void SUB_TabItemSichtbarkeitGeaendert(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			TabItem tabItem = i_objSender as TabItem;
			if (tabItem != null && i_fdcArgs.NewValue is bool && !(bool)i_fdcArgs.NewValue && tabItem.IsSelected)
			{
				base.AssociatedObject.SelectedIndex = 0;
			}
		}
	}
}
