using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[XmlType("MappingEntry")]
	public class EDC_ProtokollParameterNamenMapperItem
	{
		[XmlAttribute]
		public string key;

		[XmlAttribute]
		public string value;
	}
}
