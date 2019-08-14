using System.Collections;
using System.Collections.Generic;

namespace Prism.Regions
{
	public interface IRegionBehaviorCollection : IEnumerable<KeyValuePair<string, IRegionBehavior>>, IEnumerable
	{
		IRegionBehavior this[string key]
		{
			get;
		}

		void Add(string key, IRegionBehavior regionBehavior);

		bool ContainsKey(string key);
	}
}
