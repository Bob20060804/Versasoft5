using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DebuggerStepThrough]
	[XmlRoot(Namespace = "http://www.br-automation.com/EventLog", IsNullable = false)]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	public class LogBookPackage
	{
		private Language[] descriptionsField;

		private LogBookSnapshot[] logBookSnapshotField;

		private uint versionField;

		[XmlArrayItem(IsNullable = false)]
		public Language[] Descriptions
		{
			get
			{
				return descriptionsField;
			}
			set
			{
				descriptionsField = value;
			}
		}

		[XmlElement("LogBookSnapshot")]
		public LogBookSnapshot[] LogBookSnapshot
		{
			get
			{
				return logBookSnapshotField;
			}
			set
			{
				logBookSnapshotField = value;
			}
		}

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
	}
}
