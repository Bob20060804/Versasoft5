using Prism.Properties;
using System;

namespace Prism.Regions
{
	public class AllActiveRegion : Region
	{
		public override IViewsCollection ActiveViews => Views;

		public override void Deactivate(object view)
		{
			throw new InvalidOperationException(Prism.Properties.Resources.DeactiveNotPossibleException);
		}
	}
}
