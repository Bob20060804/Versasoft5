using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class additionalDataType
	{
		[XmlElement("data")]
		public valueType[] data
		{
			get;
			set;
		}
	}
}
