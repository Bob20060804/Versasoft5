using Prism.Properties;
using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prism.Regions
{
	public class ItemsControlRegionAdapter : RegionAdapterBase<ItemsControl>
	{
		public ItemsControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, ItemsControl regionTarget)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			if (regionTarget.ItemsSource != null || BindingOperations.GetBinding(regionTarget, ItemsControl.ItemsSourceProperty) != null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.ItemsControlHasItemsSourceException);
			}
			if (regionTarget.Items.Count > 0)
			{
				foreach (object item in (IEnumerable)regionTarget.Items)
				{
					region.Add(item);
				}
				regionTarget.Items.Clear();
			}
			regionTarget.ItemsSource = region.Views;
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}
	}
}
