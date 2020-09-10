using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	public class labelPropertiesTypeLabelProperty : propertyType
	{
		[XmlAttribute]
		public string labelField
		{
			get;
			set;
		}
	}
}
