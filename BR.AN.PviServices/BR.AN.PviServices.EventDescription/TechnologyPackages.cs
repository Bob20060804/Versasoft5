using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BR.AN.PviServices.EventDescription
{
	[Serializable]
	[XmlType(Namespace = "http://www.br-automation.com/EventLog")]
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	public class TechnologyPackages
	{
		private TechnologyPackage[] packageField;

		private string versionField;

		[XmlElement("Package")]
		public TechnologyPackage[] Package
		{
			get
			{
				return packageField;
			}
			set
			{
				packageField = value;
			}
		}

		[XmlAttribute]
		public string Version
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
