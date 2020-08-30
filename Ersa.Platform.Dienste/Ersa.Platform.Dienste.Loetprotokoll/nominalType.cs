using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class nominalType
	{
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

		[XmlAttribute]
		public DateTime starttime
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool starttimeSpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public DateTime endtime
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool endtimeSpecified
		{
			get;
			set;
		}
	}
}
