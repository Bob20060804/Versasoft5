using System.Linq;

namespace Prism.Regions
{
	public class SingleActiveRegion : Region
	{
		public override void Activate(object view)
		{
			object obj = ActiveViews.FirstOrDefault();
			if (obj != null && obj != view && Views.Contains(obj))
			{
				base.Deactivate(obj);
			}
			base.Activate(view);
		}
	}
}
