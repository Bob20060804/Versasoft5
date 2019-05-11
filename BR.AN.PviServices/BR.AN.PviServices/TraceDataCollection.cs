using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TraceDataCollection : IDisposable
	{
		private ArrayList propItems;

		public int Count
		{
			get
			{
				if (propItems == null)
				{
					return 0;
				}
				return propItems.Count;
			}
		}

		public TraceData this[int index]
		{
			get
			{
				if (index < propItems.Count)
				{
					return (TraceData)propItems[index];
				}
				return null;
			}
		}

		internal TraceDataCollection()
		{
			propItems = new ArrayList();
		}

		internal void Add(TraceData trcData)
		{
			propItems.Add(trcData);
		}

		public void Dispose()
		{
			if (propItems != null)
			{
				propItems.Clear();
				propItems = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
