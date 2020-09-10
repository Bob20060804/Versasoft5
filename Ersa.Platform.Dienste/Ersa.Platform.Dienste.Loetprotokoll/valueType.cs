using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class valueType
	{
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
