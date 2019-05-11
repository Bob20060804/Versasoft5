using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class CpuRedInfo
	{
		private int switchoverLevelField;

		private int cpuInSyncField;

		private string projectNameField;

		[XmlAttribute]
		[DefaultValue(0)]
		public int SwitchoverLevel
		{
			get
			{
				return switchoverLevelField;
			}
			set
			{
				switchoverLevelField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int CpuInSync
		{
			get
			{
				return cpuInSyncField;
			}
			set
			{
				cpuInSyncField = value;
			}
		}

		[XmlAttribute]
		public string ProjectName
		{
			get
			{
				return projectNameField;
			}
			set
			{
				projectNameField = value;
			}
		}

		public CpuRedInfo()
		{
			switchoverLevelField = 0;
			cpuInSyncField = 0;
		}
	}
}
