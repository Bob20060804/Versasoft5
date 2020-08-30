using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class sampleType
	{
		public failedType failed
		{
			get;
			set;
		}

		[XmlAttribute]
		public DateTime time
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool timeSpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public long duration
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool durationSpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public string value
		{
			[CompilerGenerated]
			get
			{
				return value;
			}
			[CompilerGenerated]
			set
			{
				this.value = value;
			}
		}
	}
}
