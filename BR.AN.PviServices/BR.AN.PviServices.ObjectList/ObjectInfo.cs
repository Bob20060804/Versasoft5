using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.ObjectList
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/ObjectList")]
	[XmlInclude(typeof(FileInfo))]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlInclude(typeof(ModInfo))]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public abstract class ObjectInfo
	{
		private string nameField;

		private uint sizeField;

		private int projectDependentField;

		private string timeModifiedField;

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

		[XmlAttribute]
		public uint Size
		{
			get
			{
				return sizeField;
			}
			set
			{
				sizeField = value;
			}
		}

		[XmlAttribute]
		public int ProjectDependent
		{
			get
			{
				return projectDependentField;
			}
			set
			{
				projectDependentField = value;
			}
		}

		[XmlAttribute]
		public string TimeModified
		{
			get
			{
				return timeModifiedField;
			}
			set
			{
				timeModifiedField = value;
			}
		}
	}
}
