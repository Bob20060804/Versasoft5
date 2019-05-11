using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointsData : IDisposable
	{
		private bool disposed;

		private uint propID;

		private ulong propOffset;

		private TracePointDataCollection propTraceData;

		[CLSCompliant(false)]
		public uint ID
		{
			get
			{
				return propID;
			}
		}

		[CLSCompliant(false)]
		public ulong Offset
		{
			get
			{
				return propOffset;
			}
		}

		public TracePointDataCollection TraceData => propTraceData;

		private void InitMembers()
		{
			propID = 0u;
			propOffset = 0uL;
			propTraceData = null;
		}

		public TracePointsData()
		{
			InitMembers();
		}

		internal TracePointsData(IntPtr pData, uint dataLen, ref int dataOffset)
		{
			uint num = 0u;
			InitMembers();
			propTraceData = new TracePointDataCollection();
			num = PviMarshal.ReadUInt32(pData, ref dataOffset);
			propID = PviMarshal.ReadUInt32(pData, ref dataOffset);
			PviMarshal.ReadUInt32(pData, ref dataOffset);
			PviMarshal.ReadUInt32(pData, ref dataOffset);
			propOffset = PviMarshal.ReadUInt64(pData, ref dataOffset);
			propTraceData.ReadResponseData(pData, dataLen, num, ref dataOffset);
		}

		internal void UpdateFormat(ArrayList formatTypes, ArrayList typeLenghts)
		{
			int num = 0;
			if (propTraceData == null)
			{
				propTraceData = new TracePointDataCollection();
			}
			for (num = 0; num < formatTypes.Count; num++)
			{
				TracePointData tracePointData;
				if (num < propTraceData.Count)
				{
					tracePointData = propTraceData[num];
				}
				else
				{
					tracePointData = new TracePointData();
					propTraceData.Add(tracePointData);
				}
				tracePointData.UpdateFormat((uint)formatTypes[num], (uint)typeLenghts[num]);
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
					propTraceData.Dispose();
				}
				disposed = true;
			}
		}

		~TracePointsData()
		{
			Dispose(disposing: false);
		}

		public override string ToString()
		{
			return "ID=\"" + propID.ToString() + "\" Offset=\"" + propOffset.ToString() + "\"  Records=\"" + propTraceData.Count.ToString() + "\"";
		}
	}
}
