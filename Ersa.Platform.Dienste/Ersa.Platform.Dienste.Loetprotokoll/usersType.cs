using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class usersType
	{
		[XmlElement("user")]
		public userType[] user
		{
			get;
			set;
		}
	}
}
