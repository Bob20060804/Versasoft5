using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class channelType
	{
		[XmlElement("sample")]
		public List<sampleType> lstSample
		{
			get;
			set;
		}

		public limitType limit_hh
		{
			get;
			set;
		}

		public limitType limit_h
		{
			get;
			set;
		}

		public nominalType nominalValue
		{
			get;
			set;
		}

		public limitType limit_l
		{
			get;
			set;
		}

		public limitType limit_ll
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

		[XmlAttribute]
		public string state
		{
			get;
			set;
		}
	}
}
