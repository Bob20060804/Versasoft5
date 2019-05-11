using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class NetworkDevices
	{
		private Host hostField;

		private Interface[] interfaceField;

		public Host Host
		{
			get
			{
				return hostField;
			}
			set
			{
				hostField = value;
			}
		}

		[XmlElement("Interface")]
		public Interface[] Interface
		{
			get
			{
				return interfaceField;
			}
			set
			{
				interfaceField = value;
			}
		}
	}
}
