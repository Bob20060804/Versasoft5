using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class actionType
	{
		[XmlElement("expression")]
		public expressionType[] expression
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
	}
}
