using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class materialPropertiesRequestType
	{
		[XmlElement("materialProperty")]
		public propertyRequestType[] materialProperty
		{
			get;
			set;
		}
	}
}
