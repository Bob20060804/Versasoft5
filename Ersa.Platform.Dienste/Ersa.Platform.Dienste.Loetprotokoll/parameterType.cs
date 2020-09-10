using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class parameterType
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
