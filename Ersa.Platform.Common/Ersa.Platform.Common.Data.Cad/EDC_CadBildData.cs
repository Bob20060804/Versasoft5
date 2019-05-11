using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadImages", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadBildData
	{
		public const string gC_strTabellenName = "CadImages";

		public const string gC_strSpalteProgrammId = "ProgramId";

		public const string gC_strSpalteBild = "Image";

		public const string gC_strSpalteTransmatrix00 = "Matrix00";

		public const string gC_strSpalteTransmatrix01 = "Matrix01";

		public const string gC_strSpalteTransmatrix10 = "Matrix10";

		public const string gC_strSpalteTransmatrix11 = "Matrix11";

		private const string mC_strSpalteVerwendung = "Purpose";

		private const string mC_strSpalteDateiname = "FileName";

		private const string mC_strSpalteBildbreite = "Width";

		private const string mC_strSpalteBildhoehe = "Height";

		private const string mC_strSpalteAnsichtsSeite = "ViewSide";

		private const string mC_strSpalteAufloesungx = "PixelPerMmX";

		private const string mC_strSpalteAufloesungy = "PixelPerMmY";

		private const string mC_strSpalteNullpunktx = "ZeroOffsetX";

		private const string mC_strSpalteNullpunkty = "ZeroOffsetY";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProgramId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Purpose", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32Verwendung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Image", PRO_blnIsRequired = true)]
		public byte[] PRO_bytBild
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("FileName", PRO_i32Length = 250)]
		public string PRO_strDateiname
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Width")]
		public float PRO_sngBildbreite
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Height")]
		public float PRO_sngBildhoehe
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ViewSide")]
		public int PRO_i32Ansichtsseite
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PixelPerMmX")]
		public float PRO_sngAufloesungX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PixelPerMmY")]
		public float PRO_sngAufloesungY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ZeroOffsetX")]
		public float PRO_sngNullpunktX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ZeroOffsetY")]
		public float PRO_sngNullpunktY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Matrix00")]
		public float PRO_sngMatrix00
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Matrix01")]
		public float PRO_sngMatrix01
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Matrix10")]
		public float PRO_sngMatrix10
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Matrix11")]
		public float PRO_sngMatrix11
		{
			get;
			set;
		}

		public ENUM_CadLeiterkartenAnsicht PRO_enmLeiterkartenAnsicht
		{
			get
			{
				return (ENUM_CadLeiterkartenAnsicht)PRO_i32Ansichtsseite;
			}
			set
			{
				PRO_i32Ansichtsseite = (int)value;
			}
		}

		public ENUM_CadBildVerwendung PRO_enmCadBildVerwendung
		{
			get
			{
				return (ENUM_CadBildVerwendung)PRO_i32Verwendung;
			}
			set
			{
				PRO_i32Verwendung = (int)value;
			}
		}

		public EDC_CadBildData()
		{
		}

		public EDC_CadBildData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strProgrammIdUndVerwendungWhereStatementErstellen(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung)
		{
			return string.Format("{0} AND {1} = {2}", FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId), "Purpose", (int)i_enmVerwendung);
		}

		public static string FUN_strLoescheBildStatementErstellen(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung)
		{
			return string.Format("DELETE FROM {0} {1}", "CadImages", FUN_strProgrammIdUndVerwendungWhereStatementErstellen(i_i64ProgrammId, i_enmVerwendung));
		}

		public static string FUN_strLoescheBilderStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("DELETE FROM {0} {1}", "CadImages", FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId));
		}
	}
}
