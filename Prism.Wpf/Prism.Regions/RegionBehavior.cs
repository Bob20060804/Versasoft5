using Prism.Properties;
using System;

namespace Prism.Regions
{
	public abstract class RegionBehavior : IRegionBehavior
	{
		private IRegion region;

		public IRegion Region
		{
			get
			{
				return region;
			}
			set
			{
				if (IsAttached)
				{
					throw new InvalidOperationException(Prism.Properties.Resources.RegionBehaviorRegionCannotBeSetAfterAttach);
				}
				region = value;
			}
		}

		public bool IsAttached
		{
			get;
			private set;
		}

		public void Attach()
		{
			if (region == null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.RegionBehaviorAttachCannotBeCallWithNullRegion);
			}
			IsAttached = true;
			OnAttach();
		}

		protected abstract void OnAttach();
	}
}
