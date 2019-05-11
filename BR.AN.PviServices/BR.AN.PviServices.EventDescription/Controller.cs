using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	public class Controller
	{
		private string nameField;

		private string biosVersionField;

		private string firmwareVersionField;

		private string stateField;

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
		public string BiosVersion
		{
			get
			{
				return biosVersionField;
			}
			set
			{
				biosVersionField = value;
			}
		}

		[XmlAttribute]
		public string FirmwareVersion
		{
			get
			{
				return firmwareVersionField;
			}
			set
			{
				firmwareVersionField = value;
			}
		}

		[XmlAttribute]
		public string State
		{
			get
			{
				return stateField;
			}
			set
			{
				stateField = value;
			}
		}
	}
}
