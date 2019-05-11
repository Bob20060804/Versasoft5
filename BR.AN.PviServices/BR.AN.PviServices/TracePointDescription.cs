using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointDescription : IDisposable
	{
		private bool disposed;

		private uint propRecordLen;

		private uint propID;

		private ulong propOffset;

		private ArrayList propListOfVars;

		private object propUserData;

		private TracePointDataCollection propTraceData;

		internal uint RecordLen => propRecordLen;

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

		public ArrayList ListOfVariables => propListOfVars;

		public object UserData
		{
			get
			{
				return propUserData;
			}
			set
			{
				propUserData = value;
			}
		}

		public TracePointDataCollection TraceData => propTraceData;

		public TracePointDescription()
		{
			propID = 0u;
			propOffset = 0uL;
			propListOfVars = null;
			propRecordLen = 0u;
			propUserData = null;
			propTraceData = null;
		}

		internal TracePointDescription(uint id, ulong offset, ArrayList listOfVariables, uint recordLen)
		{
			propID = id;
			propOffset = offset;
			propListOfVars = new ArrayList(listOfVariables);
			propRecordLen = recordLen;
			propUserData = null;
			propTraceData = null;
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

		internal void UpdateFormatNData(TracePointsData tpDat)
		{
			int num = 0;
			if (propTraceData == null)
			{
				propTraceData = new TracePointDataCollection();
			}
			for (num = 0; num < tpDat.TraceData.Count; num++)
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
				tracePointData.UpdateData(tpDat.TraceData[num].IECType, tpDat.TraceData[num].TypeLength, tpDat.TraceData[num].DataBytes);
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
					propListOfVars.Clear();
				}
				disposed = true;
			}
		}

		~TracePointDescription()
		{
			Dispose(disposing: false);
		}

		public override string ToString()
		{
			return "ID=\"" + propID.ToString() + "\" Offset=\"" + propOffset.ToString() + "\" RecordLen=\"" + propRecordLen.ToString() + "\" NumOfVars=\"" + propListOfVars.Count.ToString() + "\" NumOfData=\"" + propTraceData.Count.ToString() + "\"";
		}
	}
}
