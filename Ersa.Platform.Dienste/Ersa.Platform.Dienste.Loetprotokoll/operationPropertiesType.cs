using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class operationPropertiesType
	{
		[XmlElement("operationProperty")]
		public propertyType[] operationProperty
		{
			get;
			set;
		}
	}
}
