using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointCollection : IDisposable
	{
		private Hashtable propTPKeys;

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

		public TracePoint this[int index]
		{
			get
			{
				if (index < propItems.Count)
				{
					return (TracePoint)propItems[index];
				}
				return null;
			}
		}

		public ArrayList Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (propItems != null)
				{
					for (int i = 0; i < propItems.Count; i++)
					{
						arrayList.Add(((TracePoint)propItems[i]).Name);
					}
				}
				return arrayList;
			}
		}

		internal TracePointCollection(Task task)
		{
			propItems = new ArrayList();
			propTPKeys = new Hashtable();
		}

		internal int Add(TracePoint trcPoint)
		{
			if (propTPKeys.ContainsKey(trcPoint.Name))
			{
				return -2;
			}
			propTPKeys.Add(trcPoint.Name, propItems.Count);
			return propItems.Add(trcPoint);
		}

		public int Contains(string nameOfVariable)
		{
			if (propTPKeys.ContainsKey(nameOfVariable))
			{
				return (int)propTPKeys[nameOfVariable];
			}
			return -1;
		}

		public void Clear()
		{
			int num = 0;
			for (num = 0; num < propItems.Count; num++)
			{
				((TracePoint)propItems[num]).Dispose();
			}
			propItems.Clear();
			propTPKeys.Clear();
		}

		public void Dispose()
		{
			int num = 0;
			for (num = 0; num < propItems.Count; num++)
			{
				((TracePoint)propItems[num]).Dispose();
			}
			propItems = null;
			if (propTPKeys != null)
			{
				propTPKeys.Clear();
				propTPKeys = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
