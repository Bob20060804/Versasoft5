using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	public enum Severity
	{
		Success,
		Information,
		Warning,
		Error,
		Debug
	}
}
