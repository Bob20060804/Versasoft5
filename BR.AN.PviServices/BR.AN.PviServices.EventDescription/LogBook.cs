using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[XmlInclude(typeof(EventLogBook))]
	[XmlInclude(typeof(ErrorLogBook))]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public abstract class LogBook
	{
		private string nameField;

		private uint versionField;

		private uint activeIndexField;

		private uint referenceIndexField;

		private uint attributesField;

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
		public uint Version
		{
			get
			{
				return versionField;
			}
			set
			{
				versionField = value;
			}
		}

		[DefaultValue(typeof(uint), "0")]
		[XmlAttribute]
		public uint ActiveIndex
		{
			get
			{
				return activeIndexField;
			}
			set
			{
				activeIndexField = value;
			}
		}

		[DefaultValue(typeof(uint), "0")]
		[XmlAttribute]
		public uint ReferenceIndex
		{
			get
			{
				return referenceIndexField;
			}
			set
			{
				referenceIndexField = value;
			}
		}

		[XmlAttribute]
		public uint Attributes
		{
			get
			{
				return attributesField;
			}
			set
			{
				attributesField = value;
			}
		}

		public LogBook()
		{
			activeIndexField = 0u;
			referenceIndexField = 0u;
		}
	}
}
