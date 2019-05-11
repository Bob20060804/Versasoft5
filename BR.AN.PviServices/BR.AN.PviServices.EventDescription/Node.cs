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
	public class Node
	{
		private string nameField;

		private string pathField;

		private string stateField;

		private string firmwareVersionField;

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
		public string Path
		{
			get
			{
				return pathField;
			}
			set
			{
				pathField = value;
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
	}
}
