using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.ObjectList
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/ObjectList")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class FileInfo : ObjectInfo
	{
		private string dirField;

		private string classField;

		private string groupField;

		private string hashValueField;

		[XmlAttribute]
		public string Dir
		{
			get
			{
				return dirField;
			}
			set
			{
				dirField = value;
			}
		}

		[XmlAttribute]
		public string Class
		{
			get
			{
				return classField;
			}
			set
			{
				classField = value;
			}
		}

		[XmlAttribute]
		public string Group
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

		[XmlAttribute]
		public string HashValue
		{
			get
			{
				return hashValueField;
			}
			set
			{
				hashValueField = value;
			}
		}
	}
}
