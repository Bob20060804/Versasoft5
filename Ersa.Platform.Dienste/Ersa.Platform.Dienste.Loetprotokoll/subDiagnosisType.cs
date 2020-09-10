using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class subDiagnosisType
	{
		public subPositionsType subPositions
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
		public string referenceSubTestName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string referenceSubTestPosition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string diagnosisPosition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string diagnosisPositionType
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
