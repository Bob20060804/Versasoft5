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
	public class TOC
	{
		private ITEM[] iTEMField;

		private string versionField;

		[XmlElement("ITEM")]
		public ITEM[] ITEM
		{
			get
			{
				return iTEMField;
			}
			set
			{
				iTEMField = value;
			}
		}

		[DefaultValue("0")]
		[XmlAttribute]
		public string Version
		{
			get
			{
				return versionField;
			}
			set
			{
				versionField = value;
			}
		}

		public TOC()
		{
			versionField = "0";
		}
	}
}
