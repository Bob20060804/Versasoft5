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
	public class CpuConfiguration
	{
		private string serialNumberField;

		private int nodeField;

		private string typeField;

		private int modeField;

		[XmlAttribute]
		public string SerialNumber
		{
			get
			{
				return serialNumberField;
			}
			set
			{
				serialNumberField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(0)]
		public int Node
		{
			get
			{
				return nodeField;
			}
			set
			{
				nodeField = value;
			}
		}

		[XmlAttribute]
		public string Type
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

		[DefaultValue(0)]
		[XmlAttribute]
		public int Mode
		{
			get
			{
				return modeField;
			}
			set
			{
				modeField = value;
			}
		}

		public CpuConfiguration()
		{
			nodeField = 0;
			modeField = 0;
		}
	}
}
