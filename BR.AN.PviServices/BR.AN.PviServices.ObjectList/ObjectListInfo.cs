using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.ObjectList
{
	[Serializable]
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/ObjectList")]
	[DesignerCategory("code")]
	[XmlRoot("ObjectList", Namespace = "http://www.br-automation.com/ObjectList", IsNullable = false)]
	public class ObjectListInfo
	{
		private ModInfo[] modInfoField;

		private FileInfo[] fileInfoField;

		private DirInfo[] dirInfoField;

		private uint versionField;

		[XmlElement("ModInfo")]
		public ModInfo[] ModInfo
		{
			get
			{
				return modInfoField;
			}
			set
			{
				modInfoField = value;
			}
		}

		[XmlElement("FileInfo")]
		public FileInfo[] FileInfo
		{
			get
			{
				return fileInfoField;
			}
			set
			{
				fileInfoField = value;
			}
		}

		[XmlElement("DirInfo")]
		public DirInfo[] DirInfo
		{
			get
			{
				return dirInfoField;
			}
			set
			{
				dirInfoField = value;
			}
		}

		[DefaultValue(typeof(uint), "0")]
		[XmlAttribute]
		public uint Version
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

		public ObjectListInfo()
		{
			versionField = 0u;
		}
	}
}
