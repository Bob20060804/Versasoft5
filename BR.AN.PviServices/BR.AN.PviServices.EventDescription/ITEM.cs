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
	public class ITEM
	{
		private string idField;

		private string nAMEField;

		private int tYPEField;

		private string mEMField;

		private string mODIFIEDField;

		[XmlAttribute]
		public string ID
		{
			get
			{
				return idField;
			}
			set
			{
				idField = value;
			}
		}

		[XmlAttribute]
		public string NAME
		{
			get
			{
				return nAMEField;
			}
			set
			{
				nAMEField = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int TYPE
		{
			get
			{
				return tYPEField;
			}
			set
			{
				tYPEField = value;
			}
		}

		[XmlAttribute]
		public string MEM
		{
			get
			{
				return mEMField;
			}
			set
			{
				mEMField = value;
			}
		}

		[XmlAttribute]
		public string MODIFIED
		{
			get
			{
				return mODIFIEDField;
			}
			set
			{
				mODIFIEDField = value;
			}
		}

		public ITEM()
		{
			tYPEField = 0;
		}
	}
}
