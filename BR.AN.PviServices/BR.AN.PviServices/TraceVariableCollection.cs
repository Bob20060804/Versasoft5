using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TraceVariableCollection : IDisposable
	{
		private TracePoint propParentTracePoint;

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

		public TracePointVariable this[int index]
		{
			get
			{
				if (index < propItems.Count)
				{
					return (TracePointVariable)propItems[index];
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
						arrayList.Add(((TracePointVariable)propItems[i]).Name);
					}
				}
				return arrayList;
			}
		}

		internal TraceVariableCollection(TracePoint tracePoint)
		{
			propParentTracePoint = tracePoint;
			propItems = new ArrayList();
			propTPKeys = new Hashtable();
		}

		internal int Add(string name)
		{
			if (propTPKeys.ContainsKey(name))
			{
				return -2;
			}
			propTPKeys.Add(name, propItems.Count);
			return propItems.Add(new TracePointVariable(propParentTracePoint, name));
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
				((TracePointVariable)propItems[num]).Dispose();
			}
			propItems.Clear();
		}

		public void Dispose()
		{
			int num = 0;
			propParentTracePoint = null;
			if (propItems != null)
			{
				for (num = 0; num < propItems.Count; num++)
				{
					((TracePointVariable)propItems[num]).Dispose();
				}
			}
			propItems = null;
			GC.SuppressFinalize(this);
		}
	}
}
