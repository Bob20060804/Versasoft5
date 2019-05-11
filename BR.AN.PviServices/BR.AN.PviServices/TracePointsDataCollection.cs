using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointsDataCollection : ICollection, IEnumerable, IDisposable
	{
		private bool disposed;

		private ArrayList propArrayList;

		private Hashtable propMapIDToObj;

		public virtual object SyncRoot => propArrayList.SyncRoot;

		public virtual bool IsSynchronized => propArrayList.IsSynchronized;

		public virtual int Count => propArrayList.Count;

		public virtual ICollection Keys => propMapIDToObj.Keys;

		public TracePointsData this[int indexer]
		{
			get
			{
				if (indexer < propArrayList.Count)
				{
					return (TracePointsData)propArrayList[indexer];
				}
				return null;
			}
		}

		[CLSCompliant(false)]
		public TracePointsData this[uint key]
		{
			get
			{
				if (propMapIDToObj.ContainsKey(key))
				{
					return (TracePointsData)propMapIDToObj[key];
				}
				return null;
			}
		}

		public TracePointsDataCollection()
		{
			propArrayList = new ArrayList();
			propMapIDToObj = new Hashtable();
		}

		public virtual IEnumerator GetEnumerator()
		{
			return propArrayList.GetEnumerator();
		}

		public virtual void CopyTo(Array array, int count)
		{
		}

		internal uint ReadResponseData(IntPtr pData, uint dataLen)
		{
			uint result = uint.MaxValue;
			int dataOffset = 0;
			while (dataOffset < dataLen)
			{
				TracePointsData tracePointsData = new TracePointsData(pData, dataLen, ref dataOffset);
				propMapIDToObj.Add(tracePointsData.ID, tracePointsData);
				propArrayList.Add(tracePointsData);
			}
			return result;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					propArrayList.Clear();
					propMapIDToObj.Clear();
				}
				disposed = true;
			}
		}

		~TracePointsDataCollection()
		{
			Dispose(disposing: false);
		}
	}
}
