using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	public class EventLogBook : LogBook
	{
		private EventEntry[] eventEntryField;

		[XmlElement("EventEntry")]
		public EventEntry[] EventEntry
		{
			get
			{
				return eventEntryField;
			}
			set
			{
				eventEntryField = value;
			}
		}
	}
}
