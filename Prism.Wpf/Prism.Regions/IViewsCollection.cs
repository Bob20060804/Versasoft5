using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Prism.Regions
{
	public interface IViewsCollection : IEnumerable<object>, IEnumerable, INotifyCollectionChanged
	{
		bool Contains(object value);
	}
}
