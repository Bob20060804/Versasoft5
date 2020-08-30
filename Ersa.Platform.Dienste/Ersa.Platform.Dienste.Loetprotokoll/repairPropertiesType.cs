using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class repairPropertiesType
	{
		[XmlElement("repairProperty")]
		public propertyType[] repairProperty
		{
			get;
			set;
		}
	}
}
