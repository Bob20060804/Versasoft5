using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointDescriptionCollection : ICollection, IEnumerable, IDisposable
	{
		private bool disposed;

		private ArrayList propArrayList;

		private Hashtable propMapIDToObj;

		private int propPVIDataSize;

		public virtual object SyncRoot => propArrayList.SyncRoot;

		public virtual bool IsSynchronized => propArrayList.IsSynchronized;

		public virtual int Count => propArrayList.Count;

		public virtual ICollection Keys => propMapIDToObj.Keys;

		internal int PVIDataSize => propPVIDataSize;

		public TracePointDescription this[int indexer]
		{
			get
			{
				if (indexer < propArrayList.Count)
				{
					return (TracePointDescription)propArrayList[indexer];
				}
				return null;
			}
		}

		[CLSCompliant(false)]
		public TracePointDescription this[uint key]
		{
			get
			{
				if (propMapIDToObj.ContainsKey(key))
				{
					return (TracePointDescription)propMapIDToObj[key];
				}
				return null;
			}
		}

		public TracePointDescriptionCollection()
		{
			propArrayList = new ArrayList();
			propMapIDToObj = new Hashtable();
			propPVIDataSize = 0;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return propArrayList.GetEnumerator();
		}

		public virtual void CopyTo(Array array, int count)
		{
		}

		[CLSCompliant(false)]
		public int Add(uint id, ulong offset, ArrayList listOfVariables)
		{
			int num = 0;
			uint num2 = 0u;
			int num3 = 0;
			if (propMapIDToObj.ContainsKey(id))
			{
				return -1;
			}
			for (num3 = 0; num3 < listOfVariables.Count; num3++)
			{
				num += listOfVariables[num3].ToString().Length;
				if (num3 != 0)
				{
					num++;
				}
			}
			num2 = (uint)(25 + num);
			TracePointDescription value = new TracePointDescription(id, offset, listOfVariables, num2);
			propMapIDToObj.Add(id, value);
			propArrayList.Add(value);
			propPVIDataSize += (int)num2;
			return propArrayList.Count;
		}

		[CLSCompliant(false)]
		public uint UpdateFormat(TracePointFormatCollection traceFormats)
		{
			for (int i = 0; i < traceFormats.Count; i++)
			{
				TracePointFormat tracePointFormat = traceFormats[i];
				TracePointDescription tracePointDescription = this[tracePointFormat.ID];
				if (tracePointDescription == null)
				{
					return tracePointFormat.ID;
				}
				tracePointDescription.UpdateFormat(tracePointFormat.VariableFormats, tracePointFormat.VariableLengths);
			}
			return uint.MaxValue;
		}

		[CLSCompliant(false)]
		public uint UpdateTracePointsData(TracePointFormatCollection traceFormats)
		{
			for (int i = 0; i < traceFormats.Count; i++)
			{
				TracePointFormat tracePointFormat = traceFormats[i];
				TracePointDescription tracePointDescription = this[tracePointFormat.ID];
				if (tracePointDescription == null)
				{
					return tracePointFormat.ID;
				}
				tracePointDescription.UpdateFormat(tracePointFormat.VariableFormats, tracePointFormat.VariableLengths);
			}
			return uint.MaxValue;
		}

		[CLSCompliant(false)]
		public uint UpdateTracePointsData(TracePointsDataCollection traceDataCol)
		{
			for (int i = 0; i < traceDataCol.Count; i++)
			{
				TracePointsData tracePointsData = traceDataCol[i];
				TracePointDescription tracePointDescription = this[tracePointsData.ID];
				if (tracePointDescription == null)
				{
					return tracePointsData.ID;
				}
				tracePointDescription.UpdateFormatNData(tracePointsData);
			}
			return uint.MaxValue;
		}

		public void Clear()
		{
			propArrayList.Clear();
			propMapIDToObj.Clear();
			propPVIDataSize = 0;
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

		~TracePointDescriptionCollection()
		{
			Dispose(disposing: false);
		}
	}
}
