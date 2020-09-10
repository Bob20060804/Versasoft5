using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class resourceType
	{
		[XmlAttribute]
		public string equipment
		{
			get;
			set;
		}

		[XmlAttribute]
		public string position
		{
			get;
			set;
		}

		[XmlAttribute]
		public string type
		{
			get;
			set;
		}

		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string state
		{
			get;
			set;
		}
	}
}
