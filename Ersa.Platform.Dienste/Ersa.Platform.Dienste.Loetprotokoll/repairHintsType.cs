using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class repairHintsType
	{
		[XmlElement("repairHint")]
		public valueType[] repairHint
		{
			get;
			set;
		}
	}
}
