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
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	public class Host
	{
		private string host1Field;

		[XmlAttribute("Host")]
		public string Host1
		{
			get
			{
				return host1Field;
			}
			set
			{
				host1Field = value;
			}
		}
	}
}
