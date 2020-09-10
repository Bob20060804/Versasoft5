using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class userType
	{
		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string password
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
