using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subPositionsType
	{
		[XmlElement("subPosition")]
		public subPositionType[] subPosition
		{
			get;
			set;
		}
	}
}
