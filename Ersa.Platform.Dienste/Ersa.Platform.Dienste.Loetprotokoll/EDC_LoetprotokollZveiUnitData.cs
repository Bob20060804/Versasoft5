using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	[XmlRoot("unitData", Namespace = "", IsNullable = false)]
	public class EDC_LoetprotokollZveiUnitData
	{
		[XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Schema = "unitData-1.0.xsd";

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
		public List<measuringType> lstMeasuring
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

		public additionalDataType additionalData
		{
			get;
			set;
		}

		public actionsType actions
		{
			get;
			set;
		}

		[XmlAttribute]
		public string unit
		{
			get;
			set;
		}

		[XmlAttribute]
		public string messageId
		{
			get;
			set;
		}

		[XmlAttribute]
		public string unitType
		{
			get;
			set;
		}

		[XmlAttribute]
		public string unitSide
		{
			get;
			set;
		}

		[XmlAttribute]
		public string plant
		{
			get;
			set;
		}

		[XmlAttribute]
		public string equipment
		{
			get;
			set;
		}

		[XmlAttribute]
		public string equipmentClass
		{
			get;
			set;
		}

		[XmlAttribute]
		public string operation
		{
			get;
			set;
		}

		[XmlAttribute]
		public string order
		{
			get;
			set;
		}

		[XmlAttribute]
		public string orderLot
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
		public string @operator
		{
			get;
			set;
		}

		[XmlAttribute]
		public string starttime
		{
			get;
			set;
		}

		[XmlAttribute]
		public string endtime
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
		public DateTime arrivaltime
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool arrivaltimeSpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public DateTime departuretime
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool departuretimeSpecified
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

		[XmlAttribute]
		public string locale
		{
			get;
			set;
		}

		[XmlAttribute]
		public string senderID
		{
			get;
			set;
		}
	}
}
