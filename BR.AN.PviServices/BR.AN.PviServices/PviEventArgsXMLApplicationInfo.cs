using System;

namespace BR.AN.PviServices
{
	public class PviEventArgsXMLApplicationInfo : PviEventArgsXML
	{
		public AppInfo ApplicationInfo
		{
			get;
			private set;
		}

		[CLSCompliant(false)]
		public PviEventArgsXMLApplicationInfo(string name, string address, int error, string language, Action action, IntPtr pData, uint dataLen)
			: base(name, address, error, language, action, pData, dataLen)
		{
			if (0 < dataLen && IntPtr.Zero != pData)
			{
				string xmlData = PviMarshal.PtrToStringUTF8(pData, dataLen);
				ApplicationInfo = new AppInfo(xmlData);
			}
		}
	}
}
