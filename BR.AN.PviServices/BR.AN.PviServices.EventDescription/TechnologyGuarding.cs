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
	public class TechnologyGuarding
	{
		private int licenseOkField;

		private int reactionStatusField;

		[XmlAttribute]
		[DefaultValue(0)]
		public int LicenseOk
		{
			get
			{
				return licenseOkField;
			}
			set
			{
				licenseOkField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int ReactionStatus
		{
			get
			{
				return reactionStatusField;
			}
			set
			{
				reactionStatusField = value;
			}
		}

		public TechnologyGuarding()
		{
			licenseOkField = 0;
			reactionStatusField = 0;
		}
	}
}
