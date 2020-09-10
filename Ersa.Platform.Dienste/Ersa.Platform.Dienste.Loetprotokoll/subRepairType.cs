using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subRepairType
	{
		public subPositionsType subPositions
		{
			get;
			set;
		}

		public additionalResultCodesType additionalResultCodes
		{
			get;
			set;
		}

		public additionalDataType additionalData
		{
			get;
			set;
		}

		public repairHintsType repairHints
		{
			get;
			set;
		}

		public repairPropertiesType repairProperties
		{
			get;
			set;
		}

		[XmlArrayItem("material", typeof(materialType), IsNullable = false)]
		[XmlArrayItem("materialLot", typeof(materialLotType), IsNullable = false)]
		public materialAttributesType[] replacement
		{
			get;
			set;
		}

		[XmlAttribute]
		public string referenceSubTestName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string referenceSubTestPosition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string repairPosition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string repairPositionType
		{
			get;
			set;
		}

		[XmlAttribute]
		public string repairResultCode
		{
			get;
			set;
		}

		[XmlAttribute]
		public string repairResultClass
		{
			get;
			set;
		}

		[XmlAttribute]
		public string dependence
		{
			get;
			set;
		}

		[XmlAttribute]
		public string description
		{
			get;
			set;
		}
	}
}
