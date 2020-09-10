using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class productionResourcesRequestType
	{
		[XmlElement("resource")]
		public resourceRequestType[] resource
		{
			get;
			set;
		}
	}
}
