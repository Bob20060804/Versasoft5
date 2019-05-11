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
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	public class OperationalValues
	{
		private int currentCpuModeField;

		private int operatingHoursField;

		[XmlAttribute]
		[DefaultValue(0)]
		public int CurrentCpuMode
		{
			get
			{
				return currentCpuModeField;
			}
			set
			{
				currentCpuModeField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(0)]
		public int OperatingHours
		{
			get
			{
				return operatingHoursField;
			}
			set
			{
				operatingHoursField = value;
			}
		}

		public OperationalValues()
		{
			currentCpuModeField = 0;
			operatingHoursField = 0;
		}
	}
}
