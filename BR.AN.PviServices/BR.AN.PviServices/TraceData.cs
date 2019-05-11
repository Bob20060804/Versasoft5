using System;

namespace BR.AN.PviServices
{
	public class TraceData : IDisposable
	{
		private IECDataTypes propDataType;

		private byte[] propData;

		public IECDataTypes DataType => propDataType;

		public byte[] Data => propData;

		internal TraceData(byte[] dataBytes, IECDataTypes dataType)
		{
			propDataType = dataType;
			propData = dataBytes;
		}

		public override string ToString()
		{
			int num = 0;
			string text = "";
			if (propData != null)
			{
				for (num = 0; num < propData.GetLength(0); num++)
				{
					text += $"{propData.GetValue(num):X2}";
				}
			}
			return "\" Type=\"" + propDataType.ToString() + "\" DataLength=\"" + ((propData != null) ? propData.GetLength(0).ToString() : "0") + "\"\" Data=\"" + text + "\"";
		}

		public void Dispose()
		{
			propDataType = IECDataTypes.UNDEFINED;
			if (propData != null)
			{
				propData = null;
			}
		}
	}
}
