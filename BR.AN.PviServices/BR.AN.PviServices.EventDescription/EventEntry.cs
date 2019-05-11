using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	public class EventEntry : LogEntry
	{
		private int eventIdField;

		private uint recordIdField;

		private int additionalDataFormatField;

		private uint facilityCodeField;

		private uint originRecordIdField;

		private uint customerCodeField;

		[XmlAttribute]
		public int EventId
		{
			get
			{
				return eventIdField;
			}
			set
			{
				eventIdField = value;
			}
		}

		[XmlAttribute]
		public uint RecordId
		{
			get
			{
				return recordIdField;
			}
			set
			{
				recordIdField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int AdditionalDataFormat
		{
			get
			{
				return additionalDataFormatField;
			}
			set
			{
				additionalDataFormatField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(uint), "0")]
		public uint FacilityCode
		{
			get
			{
				return facilityCodeField;
			}
			set
			{
				facilityCodeField = value;
			}
		}

		[DefaultValue(typeof(uint), "0")]
		[XmlAttribute]
		public uint OriginRecordId
		{
			get
			{
				return originRecordIdField;
			}
			set
			{
				originRecordIdField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(uint), "0")]
		public uint CustomerCode
		{
			get
			{
				return customerCodeField;
			}
			set
			{
				customerCodeField = value;
			}
		}

		public EventEntry()
		{
			additionalDataFormatField = 0;
			facilityCodeField = 0u;
			originRecordIdField = 0u;
			customerCodeField = 0u;
		}
	}
}
