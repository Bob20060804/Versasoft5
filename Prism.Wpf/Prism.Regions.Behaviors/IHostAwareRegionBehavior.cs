using System.Windows;

namespace Prism.Regions.Behaviors
{
	public interface IHostAwareRegionBehavior : IRegionBehavior
	{
		DependencyObject HostControl
		{
			get;
			set;
		}
	}
}
