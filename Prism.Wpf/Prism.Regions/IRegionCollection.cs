using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Prism.Regions
{
	public interface IRegionCollection : IEnumerable<IRegion>, IEnumerable, INotifyCollectionChanged
	{
		IRegion this[string regionName]
		{
			get;
		}

		void Add(IRegion region);

		bool Remove(string regionName);

		bool ContainsRegionWithName(string regionName);

		void Add(string regionName, IRegion region);
	}
}
