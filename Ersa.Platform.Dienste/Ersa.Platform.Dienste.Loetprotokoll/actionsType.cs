using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class actionsType
	{
		[XmlElement("action")]
		public actionType[] action
		{
			get;
			set;
		}
	}
}
