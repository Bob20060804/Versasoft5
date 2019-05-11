using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	public class SoftwareVers
	{
		private string automationStudioField;

		private string automationRuntimeField;

		private string visualComponentsField;

		private string acp10MotionField;

		private string aRNC0Field;

		[XmlAttribute]
		public string AutomationStudio
		{
			get
			{
				return automationStudioField;
			}
			set
			{
				automationStudioField = value;
			}
		}

		[XmlAttribute]
		public string AutomationRuntime
		{
			get
			{
				return automationRuntimeField;
			}
			set
			{
				automationRuntimeField = value;
			}
		}

		[XmlAttribute]
		public string VisualComponents
		{
			get
			{
				return visualComponentsField;
			}
			set
			{
				visualComponentsField = value;
			}
		}

		[XmlAttribute]
		public string Acp10Motion
		{
			get
			{
				return acp10MotionField;
			}
			set
			{
				acp10MotionField = value;
			}
		}

		[XmlAttribute]
		public string ARNC0
		{
			get
			{
				return aRNC0Field;
			}
			set
			{
				aRNC0Field = value;
			}
		}
	}
}
