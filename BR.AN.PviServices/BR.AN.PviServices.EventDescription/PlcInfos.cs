using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	public class PlcInfos
	{
		private CpuInfo cpuInfoField;

		private Hardware hardwareField;

		private CpuRedInfo cpuRedInfoField;

		private TOC tOCField;

		private TechnologyPackages technologyPackagesField;

		public CpuInfo CpuInfo
		{
			get
			{
				return cpuInfoField;
			}
			set
			{
				cpuInfoField = value;
			}
		}

		public Hardware Hardware
		{
			get
			{
				return hardwareField;
			}
			set
			{
				hardwareField = value;
			}
		}

		public CpuRedInfo CpuRedInfo
		{
			get
			{
				return cpuRedInfoField;
			}
			set
			{
				cpuRedInfoField = value;
			}
		}

		public TOC TOC
		{
			get
			{
				return tOCField;
			}
			set
			{
				tOCField = value;
			}
		}

		public TechnologyPackages TechnologyPackages
		{
			get
			{
				return technologyPackagesField;
			}
			set
			{
				technologyPackagesField = value;
			}
		}
	}
}
