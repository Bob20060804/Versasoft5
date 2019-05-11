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
	public class Language
	{
		private Text[] textField;

		private string idField;

		[XmlElement("Text")]
		public Text[] Text
		{
			get
			{
				return textField;
			}
			set
			{
				textField = value;
			}
		}

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
	}
}
