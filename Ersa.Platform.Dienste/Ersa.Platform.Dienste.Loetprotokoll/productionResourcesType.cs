using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class productionResourcesType
	{
		[XmlElement("resource")]
		public List<resourceType> lstResource
		{
			get;
			set;
		}
	}
}
