using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class unitTestType
	{
		[XmlElement("subTest")]
		public subTestType[] subTest
		{
			get;
			set;
		}

		public additionalResultCodesType additionalResultCodes
		{
			get;
			set;
		}

		public additionalDataType additionalData
		{
			get;
			set;
		}

		public repairHintsType repairHints
		{
			get;
			set;
		}

		public testPropertiesType testProperties
		{
			get;
			set;
		}

		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string testResultCode
		{
			get;
			set;
		}

		[XmlAttribute]
		public string testResultClass
		{
			get;
			set;
		}

		[XmlAttribute]
		public string description
		{
			get;
			set;
		}
	}
}
