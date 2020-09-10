using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subUnitDataType
	{
		public productionResourcesType productionResources
		{
			get;
			set;
		}

		public processingParametersType processingParameters
		{
			get;
			set;
		}

		public propertiesType properties
		{
			get;
			set;
		}

		[XmlArrayItem("material", typeof(materialType), IsNullable = false)]
		[XmlArrayItem("materialLot", typeof(materialLotType), IsNullable = false)]
		public materialAttributesType[] assembly
		{
			get;
			set;
		}

		[XmlArrayItem("material", typeof(materialType), IsNullable = false)]
		[XmlArrayItem("materialLot", typeof(materialLotType), IsNullable = false)]
		public materialAttributesType[] disassembly
		{
			get;
			set;
		}

		[XmlElement("measuring")]
		public measuringType[] measuring
		{
			get;
			set;
		}

		[XmlElement("diagnosis", typeof(unitDiagnosisType))]
		[XmlElement("repair", typeof(unitRepairType))]
		[XmlElement("test", typeof(unitTestType))]
		public object[] Items
		{
			get;
			set;
		}

		[XmlElement("subUnitData")]
		public subUnitDataType[] subUnitData
		{
			get;
			set;
		}

		[XmlElement("additionalId")]
		public additionalIdType[] additionalId
		{
			get;
			set;
		}

		[XmlAttribute]
		public string subUnit
		{
			get;
			set;
		}

		[XmlAttribute]
		public string subUnitType
		{
			get;
			set;
		}

		[XmlAttribute]
		public string subUnitSide
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
		public string positionType
		{
			get;
			set;
		}

		[XmlAttribute]
		public string material
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialVersion
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialVariant
		{
			get;
			set;
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

		[XmlAttribute]
		public string description
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

		[XmlAttribute]
		public string processingState
		{
			get;
			set;
		}
	}
}
