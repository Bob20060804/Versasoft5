using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	[XmlInclude(typeof(EventEntry))]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[XmlInclude(typeof(ErrorEntry))]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	public abstract class LogEntry
	{
		private string aSCIIField;

		private byte[] binaryField;

		private Severity levelField;

		private DateTime timeField;

		private uint errorNumberField;

		private uint errorInfoField;

		private string taskField;

		private string loggerModuleField;

		private uint textIdField;

		public string ASCII
		{
			get
			{
				return aSCIIField;
			}
			set
			{
				aSCIIField = value;
			}
		}

		[XmlElement(DataType = "hexBinary")]
		public byte[] Binary
		{
			get
			{
				return binaryField;
			}
			set
			{
				binaryField = value;
			}
		}

		[XmlAttribute]
		public Severity Level
		{
			get
			{
				return levelField;
			}
			set
			{
				levelField = value;
			}
		}

		[XmlAttribute]
		public DateTime Time
		{
			get
			{
				return timeField;
			}
			set
			{
				timeField = value;
			}
		}

		[XmlAttribute]
		public uint ErrorNumber
		{
			get
			{
				return errorNumberField;
			}
			set
			{
				errorNumberField = value;
			}
		}

		[XmlAttribute]
		public uint ErrorInfo
		{
			get
			{
				return errorInfoField;
			}
			set
			{
				errorInfoField = value;
			}
		}

		[XmlAttribute]
		public string Task
		{
			get
			{
				return taskField;
			}
			set
			{
				taskField = value;
			}
		}

		[XmlAttribute]
		public string LoggerModule
		{
			get
			{
				return loggerModuleField;
			}
			set
			{
				loggerModuleField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(uint), "0")]
		public uint TextId
		{
			get
			{
				return textIdField;
			}
			set
			{
				textIdField = value;
			}
		}

		public LogEntry()
		{
			textIdField = 0u;
		}
	}
}
