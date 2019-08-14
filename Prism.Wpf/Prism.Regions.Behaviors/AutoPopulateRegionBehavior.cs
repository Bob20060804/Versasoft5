using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prism.Regions.Behaviors
{
	public class AutoPopulateRegionBehavior : RegionBehavior
	{
		public const string BehaviorKey = "AutoPopulate";

		private readonly IRegionViewRegistry regionViewRegistry;

		public AutoPopulateRegionBehavior(IRegionViewRegistry regionViewRegistry)
		{
			this.regionViewRegistry = regionViewRegistry;
		}

		protected override void OnAttach()
		{
			if (string.IsNullOrEmpty(base.Region.Name))
			{
				base.Region.PropertyChanged += Region_PropertyChanged;
			}
			else
			{
				StartPopulatingContent();
			}
		}

		private void StartPopulatingContent()
		{
			foreach (object item in CreateViewsToAutoPopulate())
			{
				AddViewIntoRegion(item);
			}
			regionViewRegistry.ContentRegistered += OnViewRegistered;
		}

		protected virtual IEnumerable<object> CreateViewsToAutoPopulate()
		{
			return regionViewRegistry.GetContents(base.Region.Name);
		}

		protected virtual void AddViewIntoRegion(object viewToAdd)
		{
			base.Region.Add(viewToAdd);
		}

		private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name" && !string.IsNullOrEmpty(base.Region.Name))
			{
				base.Region.PropertyChanged -= Region_PropertyChanged;
				StartPopulatingContent();
			}
		}

		public virtual void OnViewRegistered(object sender, ViewRegisteredEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.RegionName == base.Region.Name)
			{
				AddViewIntoRegion(e.GetView());
			}
		}
	}
}
