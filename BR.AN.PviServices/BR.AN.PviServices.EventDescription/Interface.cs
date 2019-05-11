using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	public class Interface
	{
		private string ifField;

		private string ipField;

		[XmlAttribute]
		public string IF
		{
			get
			{
				return ifField;
			}
			set
			{
				ifField = value;
			}
		}

		[XmlAttribute]
		public string IP
		{
			get
			{
				return ipField;
			}
			set
			{
				ipField = value;
			}
		}
	}
}
