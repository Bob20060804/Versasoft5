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
	public class ErrorLogBook : LogBook
	{
		private ErrorEntry[] errorEntryField;

		[XmlElement("ErrorEntry")]
		public ErrorEntry[] ErrorEntry
		{
			get
			{
				return errorEntryField;
			}
			set
			{
				errorEntryField = value;
			}
		}
	}
}
