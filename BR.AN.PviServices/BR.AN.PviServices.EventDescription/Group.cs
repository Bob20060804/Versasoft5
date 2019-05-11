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
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DesignerCategory("code")]
	public class Group
	{
		private AprolSetting[] itemField;

		private Group[] group1Field;

		private string nameField;

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
		public Group[] Group1
		{
			get
			{
				return group1Field;
			}
			set
			{
				group1Field = value;
			}
		}

		[XmlAttribute]
		public string Name
		{
			get
			{
				return nameField;
			}
			set
			{
				nameField = value;
			}
		}
	}
}
