using System;

namespace BR.AN.PviServices
{
	public class TracePointVariable : IDisposable
	{
		private TracePoint propParentTracePoint;

		private string propName;

		private IECDataTypes propDataType;

		private byte[] propData;

		public string Name => propName;

		public IECDataTypes DataType => propDataType;

		public byte[] Data => propData;

		internal TracePointVariable(TracePoint tracePoint, string name)
		{
			propName = name;
			propParentTracePoint = tracePoint;
			propData = null;
			propDataType = IECDataTypes.UNDEFINED;
		}

		internal void SetDataBytes(int dataType, byte[] newValue)
		{
			propDataType = (IECDataTypes)dataType;
			propData = null;
			propData = newValue;
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
			return "Name=\"" + propName + "\" Type=\"" + propDataType.ToString() + "\" DataLength=\"" + ((propData != null) ? propData.GetLength(0).ToString() : "0") + "\"\" Data=\"" + text + "\"";
		}

		public void Dispose()
		{
			propName = null;
			propParentTracePoint = null;
			propDataType = IECDataTypes.UNDEFINED;
			if (propData != null)
			{
				propData = null;
			}
		}
	}
}
