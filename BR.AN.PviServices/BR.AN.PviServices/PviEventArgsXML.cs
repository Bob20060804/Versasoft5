using System;

namespace BR.AN.PviServices
{
	public class PviEventArgsXML : PviEventArgs
	{
		private string propXMLData;

		public string XMLData => propXMLData;

		public PviEventArgsXML(string name, string address, int error, string language, Action action, string xmlData)
			: base(name, address, error, language, action)
		{
			propXMLData = xmlData;
		}

		[CLSCompliant(false)]
		public PviEventArgsXML(string name, string address, int error, string language, Action action, IntPtr pData, uint dataLen)
			: base(name, address, error, language, action)
		{
			propXMLData = "";
			if (0 < dataLen && IntPtr.Zero != pData)
			{
				propXMLData = PviMarshal.PtrToStringAnsi(pData);
			}
		}
	}
}
