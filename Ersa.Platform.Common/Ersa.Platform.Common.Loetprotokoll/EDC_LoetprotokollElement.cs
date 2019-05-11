namespace Ersa.Platform.Common.Loetprotokoll
{
	public class EDC_LoetprotokollElement
	{
		public enum ENUM_DatenTyp
		{
			enmBool,
			enmInteger,
			enmFloat,
			enmString,
			enmZeit,
			enmTimeSpan
		}

		public string PRO_strIdentifier
		{
			get;
			set;
		}

		public string PRO_strName
		{
			get;
			set;
		}

		public string PRO_strEinheit
		{
			get;
			set;
		}

		public string PRO_strIstWert
		{
			get;
			set;
		}

		public string PRO_strSollwert
		{
			get;
			set;
		}

		public ENUM_DatenTyp PRO_enmDatenTyp
		{
			get;
			set;
		}

		public ENUM_ProtokollParameterTyp PRO_enmProtokollParameterTyp
		{
			get;
			set;
		}

		public string PRO_strToleranzPlus
		{
			get;
			set;
		}

		public string PRO_strToleranzMinus
		{
			get;
			set;
		}
	}
}
