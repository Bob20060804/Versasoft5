using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.ObjectList
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/ObjectList")]
	[DesignerCategory("code")]
	[XmlInclude(typeof(DirInfo))]
	[DebuggerStepThrough]
	public abstract class FileSystemInfo
	{
		private string dirField;

		private string classField;

		private string groupField;

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
	}
}
