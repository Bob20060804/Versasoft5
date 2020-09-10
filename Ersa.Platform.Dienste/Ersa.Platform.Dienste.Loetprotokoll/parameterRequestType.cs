using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class parameterRequestType
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
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string UnitOfMeasure
		{
			get;
			set;
		}

		[XmlAttribute]
		public string measureDataType
		{
			get;
			set;
		}
	}
}
