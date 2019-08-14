using System;
using System.ComponentModel;

namespace Prism.Regions
{
	public interface IRegion : INavigateAsync, INotifyPropertyChanged
	{
		IViewsCollection Views
		{
			get;
		}

		IViewsCollection ActiveViews
		{
			get;
		}

		object Context
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		Comparison<object> SortComparison
		{
			get;
			set;
		}

		IRegionManager RegionManager
		{
			get;
			set;
		}

		IRegionBehaviorCollection Behaviors
		{
			get;
		}

		IRegionNavigationService NavigationService
		{
			get;
			set;
		}

		IRegionManager Add(object view);

		IRegionManager Add(object view, string viewName);

		IRegionManager Add(object view, string viewName, bool createRegionManagerScope);

		void Remove(object view);

		void RemoveAll();

		void Activate(object view);

		void Deactivate(object view);

		object GetView(string viewName);
	}
}
