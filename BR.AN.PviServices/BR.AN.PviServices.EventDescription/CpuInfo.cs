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
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	public class CpuInfo
	{
		private SoftwareVers softwareVersField;

		private CpuConfiguration cpuConfigurationField;

		private OperationalValues operationalValuesField;

		private TechnologyGuarding technologyGuardingField;

		private NetworkDevices networkDevicesField;

		private int rIFModeSwitchField;

		private int cpuProcessCtrlStateField;

		private string projectNameField;

		private int offsetUtcField;

		private int dayLightSavingField;

		public SoftwareVers SoftwareVers
		{
			get
			{
				return softwareVersField;
			}
			set
			{
				softwareVersField = value;
			}
		}

		public CpuConfiguration CpuConfiguration
		{
			get
			{
				return cpuConfigurationField;
			}
			set
			{
				cpuConfigurationField = value;
			}
		}

		public OperationalValues OperationalValues
		{
			get
			{
				return operationalValuesField;
			}
			set
			{
				operationalValuesField = value;
			}
		}

		public TechnologyGuarding TechnologyGuarding
		{
			get
			{
				return technologyGuardingField;
			}
			set
			{
				technologyGuardingField = value;
			}
		}

		public NetworkDevices NetworkDevices
		{
			get
			{
				return networkDevicesField;
			}
			set
			{
				networkDevicesField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int RIFModeSwitch
		{
			get
			{
				return rIFModeSwitchField;
			}
			set
			{
				rIFModeSwitchField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int CpuProcessCtrlState
		{
			get
			{
				return cpuProcessCtrlStateField;
			}
			set
			{
				cpuProcessCtrlStateField = value;
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

		[XmlAttribute]
		[DefaultValue(0)]
		public int OffsetUtc
		{
			get
			{
				return offsetUtcField;
			}
			set
			{
				offsetUtcField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int DayLightSaving
		{
			get
			{
				return dayLightSavingField;
			}
			set
			{
				dayLightSavingField = value;
			}
		}

		public CpuInfo()
		{
			rIFModeSwitchField = 0;
			cpuProcessCtrlStateField = 0;
			offsetUtcField = 0;
			dayLightSavingField = 0;
		}
	}
}
