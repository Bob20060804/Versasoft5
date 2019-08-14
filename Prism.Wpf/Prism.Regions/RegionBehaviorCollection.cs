using System;
using System.Collections;
using System.Collections.Generic;

namespace Prism.Regions
{
	public class RegionBehaviorCollection : IRegionBehaviorCollection, IEnumerable<KeyValuePair<string, IRegionBehavior>>, IEnumerable
	{
		private readonly IRegion region;

		private readonly Dictionary<string, IRegionBehavior> behaviors = new Dictionary<string, IRegionBehavior>();

		public IRegionBehavior this[string key]
		{
			get
			{
				return behaviors[key];
			}
		}

		public RegionBehaviorCollection(IRegion region)
		{
			this.region = region;
		}

		public void Add(string key, IRegionBehavior regionBehavior)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (regionBehavior == null)
			{
				throw new ArgumentNullException("regionBehavior");
			}
			if (behaviors.ContainsKey(key))
			{
				throw new ArgumentException("Could not add duplicate behavior with same key.", "key");
			}
			behaviors.Add(key, regionBehavior);
			regionBehavior.Region = region;
			regionBehavior.Attach();
		}

		public bool ContainsKey(string key)
		{
			return behaviors.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<string, IRegionBehavior>> GetEnumerator()
		{
			return behaviors.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return behaviors.GetEnumerator();
		}
	}
}
