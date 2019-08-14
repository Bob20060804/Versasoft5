using System;
using System.Collections;
using System.Collections.Generic;

namespace Prism.Regions
{
	public interface IRegionBehaviorFactory : IEnumerable<string>, IEnumerable
	{
		void AddIfMissing(string behaviorKey, Type behaviorType);

		bool ContainsKey(string behaviorKey);

		IRegionBehavior CreateFromKey(string key);
	}
}
