using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	public class Text
	{
		private int idField;

		private string valueField;

		[XmlAttribute]
		public int Id
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

		[XmlText]
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
