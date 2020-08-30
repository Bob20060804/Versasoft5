using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subTestType
	{
		public subPositionsType subPositions
		{
			get;
			set;
		}

		[XmlElement("subTestResult")]
		public subTestResultType[] subTestResult
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
		public string testPosition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string testPositionType
		{
			get;
			set;
		}

		[XmlAttribute]
		public string description
		{
			get;
			set;
		}
	}
}
