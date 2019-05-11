using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.ObjectList
{
	[Serializable]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://www.br-automation.com/ObjectList")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	public class ModInfo : ObjectInfo
	{
		private int typeField;

		private int versionField;

		private int revisionField;

		private int memTypeField;

		private int taskClassField;

		private string installOrderField;

		private int taskStateField;

		[XmlAttribute]
		public int Type
		{
			get
			{
				return typeField;
			}
			set
			{
				typeField = value;
			}
		}

		[XmlAttribute]
		public int Version
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

		[XmlAttribute]
		public int Revision
		{
			get
			{
				return revisionField;
			}
			set
			{
				revisionField = value;
			}
		}

		[XmlAttribute]
		public int MemType
		{
			get
			{
				return memTypeField;
			}
			set
			{
				memTypeField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(-1)]
		public int TaskClass
		{
			get
			{
				return taskClassField;
			}
			set
			{
				taskClassField = value;
			}
		}

		[DefaultValue("")]
		[XmlAttribute]
		public string InstallOrder
		{
			get
			{
				return installOrderField;
			}
			set
			{
				installOrderField = value;
			}
		}

		[DefaultValue(-1)]
		[XmlAttribute]
		public int TaskState
		{
			get
			{
				return taskStateField;
			}
			set
			{
				taskStateField = value;
			}
		}

		public ModInfo()
		{
			taskClassField = -1;
			installOrderField = "";
			taskStateField = -1;
		}
	}
}
