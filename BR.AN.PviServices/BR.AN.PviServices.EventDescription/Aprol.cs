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
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	public class Aprol
	{
		private AprolSetting[] itemField;

		private Group[] groupField;

		[XmlElement("Item")]
		public AprolSetting[] Item
		{
			get
			{
				return itemField;
			}
			set
			{
				itemField = value;
			}
		}

		[XmlElement("Group")]
		public Group[] Group
		{
			get
			{
				return groupField;
			}
			set
			{
				groupField = value;
			}
		}
	}
}
