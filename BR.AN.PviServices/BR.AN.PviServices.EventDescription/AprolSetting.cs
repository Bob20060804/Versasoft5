using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class AprolSetting
	{
		private string idField;

		private string valueField;

		[XmlAttribute]
		public string Id
		{
			get
			{
				return idField;
			}
			set
			{
				idField = value;
			}
		}

		[XmlAttribute]
		public string Value
		{
			get
			{
				return valueField;
			}
			set
			{
				valueField = value;
			}
		}
	}
}
