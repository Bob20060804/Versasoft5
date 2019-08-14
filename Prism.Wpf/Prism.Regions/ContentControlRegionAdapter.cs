using Prism.Properties;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prism.Regions
{
	public class ContentControlRegionAdapter : RegionAdapterBase<ContentControl>
	{
		public ContentControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, ContentControl regionTarget)
		{
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			if (regionTarget.Content != null || BindingOperations.GetBinding(regionTarget, ContentControl.ContentProperty) != null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.ContentControlHasContentException);
			}
			region.ActiveViews.CollectionChanged += delegate
			{
				regionTarget.Content = region.ActiveViews.FirstOrDefault();
			};
			region.Views.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action == NotifyCollectionChangedAction.Add && region.ActiveViews.Count() == 0)
				{
					region.Activate(e.NewItems[0]);
				}
			};
		}

		protected override IRegion CreateRegion()
		{
			return new SingleActiveRegion();
		}
	}
}
