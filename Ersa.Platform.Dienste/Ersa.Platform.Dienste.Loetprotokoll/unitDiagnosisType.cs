using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class unitDiagnosisType
	{
		[XmlElement("subDiagnosis")]
		public subDiagnosisType[] subDiagnosis
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

		public diagnosisPropertiesType diagnosisProperties
		{
			get;
			set;
		}

		[XmlAttribute]
		public string referenceTestName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string referenceTestEquipment
		{
			get;
			set;
		}

		[XmlAttribute]
		public string diagnosisResultCode
		{
			get;
			set;
		}

		[XmlAttribute]
		public string diagnosisResultClass
		{
			get;
			set;
		}

		[XmlAttribute]
		public string dependence
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
