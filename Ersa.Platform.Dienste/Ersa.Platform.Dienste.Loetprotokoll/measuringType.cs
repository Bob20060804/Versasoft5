using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class measuringType
	{
		[XmlElement("channel")]
		public List<channelType> lstChannel
		{
			get;
			set;
		}

		[XmlAttribute]
		public string equipment
		{
			get;
			set;
		}
	}
}
