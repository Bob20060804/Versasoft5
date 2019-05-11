using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointDataCollection : ICollection, IEnumerable, IDisposable
	{
		private bool disposed;

		private ArrayList propArrayList;

		public virtual object SyncRoot => propArrayList.SyncRoot;

		public virtual bool IsSynchronized => propArrayList.IsSynchronized;

		public virtual int Count => propArrayList.Count;

		public TracePointData this[int indexer]
		{
			get
			{
				if (indexer < propArrayList.Count)
				{
					return (TracePointData)propArrayList[indexer];
				}
				return null;
			}
		}

		public TracePointDataCollection()
		{
			propArrayList = new ArrayList();
		}

		public virtual IEnumerator GetEnumerator()
		{
			return propArrayList.GetEnumerator();
		}

		public virtual void CopyTo(Array array, int count)
		{
		}

		internal int Add(TracePointData tpData)
		{
			propArrayList.Add(tpData);
			return propArrayList.Count;
		}

		internal void ReadResponseData(IntPtr pData, uint dataLen, uint lenOfRecord, ref int dataOffset)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			for (num3 = 24u; num3 < lenOfRecord; num3 += num2)
			{
				num = PviMarshal.ReadUInt32(pData, ref dataOffset);
				num3 += 4;
				num2 = PviMarshal.ReadUInt32(pData, ref dataOffset);
				num3 += 4;
				Add(new TracePointData(num, num2, pData, dataLen, ref dataOffset));
			}
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
				}
				disposed = true;
			}
		}

		~TracePointDataCollection()
		{
			Dispose(disposing: false);
		}
	}
}
