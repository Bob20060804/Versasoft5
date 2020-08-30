using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class operationPropertiesRequestType
	{
		[XmlElement("operationProperty")]
		public propertyRequestType[] operationProperty
		{
			get;
			set;
		}
	}
}
