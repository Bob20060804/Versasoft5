using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	public class Hardware
	{
		private Controller controllerField;

		private Node[] nodeField;

		public Controller Controller
		{
			get
			{
				return controllerField;
			}
			set
			{
				controllerField = value;
			}
		}

		[XmlElement("Node")]
		public Node[] Node
		{
			get
			{
				return nodeField;
			}
			set
			{
				nodeField = value;
			}
		}
	}
}
