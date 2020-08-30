using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subTestResultType
	{
		public channelType channel
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
		public DateTime time
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool timeSpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public long duration
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool durationSpecified
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
