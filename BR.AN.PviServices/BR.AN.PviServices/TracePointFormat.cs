using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class TracePointFormat : IDisposable
	{
		private bool disposed;

		private uint propID;

		private ArrayList propPVFormats;

		private ArrayList propPVLengths;

		[CLSCompliant(false)]
		public uint ID
		{
			get
			{
				return propID;
			}
		}

		public ArrayList VariableFormats => propPVFormats;

		public ArrayList VariableLengths => propPVLengths;

		private void InitMembers()
		{
			propID = 0u;
			propPVFormats = new ArrayList();
			propPVLengths = new ArrayList();
		}

		public TracePointFormat()
		{
			InitMembers();
		}

		internal TracePointFormat(IntPtr pData, uint dataLen, ref int dataOffset)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			InitMembers();
			num = PviMarshal.ReadUInt32(pData, ref dataOffset);
			propID = PviMarshal.ReadUInt32(pData, ref dataOffset);
			for (num4 = 8u; num4 < num; num4 += 8)
			{
				num2 = PviMarshal.ReadUInt32(pData, ref dataOffset);
				num3 = PviMarshal.ReadUInt32(pData, ref dataOffset);
				propPVFormats.Add(num2);
				propPVLengths.Add(num3);
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
					propPVFormats.Clear();
					propPVLengths.Clear();
				}
				disposed = true;
			}
		}

		~TracePointFormat()
		{
			Dispose(disposing: false);
		}

		public override string ToString()
		{
			string text = "";
			string text2 = "";
			IECDataTypes iECDataTypes = IECDataTypes.UNDEFINED;
			for (int i = 0; i < propPVFormats.Count; i++)
			{
				iECDataTypes = (IECDataTypes)(uint)propPVFormats[i];
				if (0 < i)
				{
					text = text + "," + iECDataTypes.ToString();
					text2 = text2 + "," + propPVLengths[i].ToString();
				}
				else
				{
					text += iECDataTypes.ToString();
					text2 += propPVLengths[i].ToString();
				}
			}
			return "ID=\"" + propID.ToString() + "\" Formats=\"" + text + "\" Lengths=\"" + text2 + "\"";
		}
	}
}
