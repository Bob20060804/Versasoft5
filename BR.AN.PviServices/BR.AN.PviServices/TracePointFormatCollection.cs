using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointFormatCollection : ICollection, IEnumerable, IDisposable
	{
		private bool disposed;

		private ArrayList propArrayList;

		private Hashtable propMapIDToObj;

		public virtual object SyncRoot => propArrayList.SyncRoot;

		public virtual bool IsSynchronized => propArrayList.IsSynchronized;

		public virtual int Count => propArrayList.Count;

		public virtual ICollection Keys => propMapIDToObj.Keys;

		public TracePointFormat this[int indexer]
		{
			get
			{
				if (indexer < propArrayList.Count)
				{
					return (TracePointFormat)propArrayList[indexer];
				}
				return null;
			}
		}

		[CLSCompliant(false)]
		public TracePointFormat this[uint key]
		{
			get
			{
				if (propMapIDToObj.ContainsKey(key))
				{
					return (TracePointFormat)propMapIDToObj[key];
				}
				return null;
			}
		}

		public TracePointFormatCollection()
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

		internal int ReadResponseData(IntPtr pData, uint dataLen)
		{
			int dataOffset = 0;
			while (dataOffset < dataLen)
			{
				TracePointFormat tracePointFormat = new TracePointFormat(pData, dataLen, ref dataOffset);
				propMapIDToObj.Add(tracePointFormat.ID, tracePointFormat);
				propArrayList.Add(tracePointFormat);
			}
			return propArrayList.Count;
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

		~TracePointFormatCollection()
		{
			Dispose(disposing: false);
		}
	}
}
