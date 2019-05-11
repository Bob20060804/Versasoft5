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
	public class LogBookSnapshot
	{
		private PlcInfos plcInfosField;

		private Aprol aprolField;

		private Language[] descriptionsField;

		private LogBook[] itemsField;

		private string nameField;

		private string fallBackLanguageField;

		public PlcInfos PlcInfos
		{
			get
			{
				return plcInfosField;
			}
			set
			{
				plcInfosField = value;
			}
		}

		public Aprol Aprol
		{
			get
			{
				return aprolField;
			}
			set
			{
				aprolField = value;
			}
		}

		[XmlArrayItem(IsNullable = false)]
		public Language[] Descriptions
		{
			get
			{
				return descriptionsField;
			}
			set
			{
				descriptionsField = value;
			}
		}

		[XmlElement("EventLogBook", typeof(EventLogBook))]
		[XmlElement("ErrorLogBook", typeof(ErrorLogBook))]
		public LogBook[] Items
		{
			get
			{
				return itemsField;
			}
			set
			{
				itemsField = value;
			}
		}

		[XmlAttribute]
		public string Name
		{
			get
			{
				return nameField;
			}
			set
			{
				nameField = value;
			}
		}

		[XmlAttribute]
		public string FallBackLanguage
		{
			get
			{
				return fallBackLanguageField;
			}
			set
			{
				fallBackLanguageField = value;
			}
		}
	}
}
