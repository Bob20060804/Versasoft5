using System;
using System.Windows;

namespace Prism.Regions
{
	public interface IRegionManagerAccessor
	{
		event EventHandler UpdatingRegions;

		string GetRegionName(DependencyObject element);

		IRegionManager GetRegionManager(DependencyObject element);
	}
}
