using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class labelPropertiesType
	{
		[XmlElement("labelProperty")]
		public labelPropertiesTypeLabelProperty[] labelProperty
		{
			get;
			set;
		}
	}
}
