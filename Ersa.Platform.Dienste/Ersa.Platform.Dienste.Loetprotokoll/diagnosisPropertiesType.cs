using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class diagnosisPropertiesType
	{
		[XmlElement("diagnosisProperty")]
		public propertyType[] diagnosisProperty
		{
			get;
			set;
		}
	}
}
