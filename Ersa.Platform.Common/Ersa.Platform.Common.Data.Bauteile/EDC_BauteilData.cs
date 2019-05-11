using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Bauteile
{
	[EDC_TabellenInformation("Packages", PRO_strTablespace = "ess5_components")]
	public class EDC_BauteilData
	{
		public const string gC_strTabellenName = "Packages";

		public const string gC_strSpalteBauteilId = "PackageId";

		public const string gC_strSpaltePackageName = "PackageName";

		public const string gC_strSpaltePitch = "Pitch";

		private const string mC_strSpalteLaenge = "Length";

		private const string mC_strSpalteBreite = "Width";

		private const string mC_strSpalteHoehe = "Height";

		private const string mC_strSpalteTextdatei = "TextFile";

		private const string mC_strSpalteBytearray = "ByteArray";

		private const string mC_strSpalteType = "Type";

		private const string mC_strSpalteAnzahlPins = "NumberOfPins";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PackageId", PRO_i32Length = 66, PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public string PRO_strBauteilId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PackageName", PRO_i32Length = 40, PRO_blnIsRequired = true)]
		public string PRO_strPackageName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Length")]
		public float PRO_sngLaenge
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Width")]
		public float PRO_sngBreite
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Height")]
		public float PRO_sngHoehe
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TextFile")]
		public string PRO_strSpalteTextdatei
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ByteArray")]
		public byte[] PRO_bytArray
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Type")]
		public int PRO_i32Type
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NumberOfPins")]
		public int PRO_i32AnzahlPins
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Pitch")]
		public float PRO_sngPitch
		{
			get;
			set;
		}

		public ENUM_BauteilTyp PRO_enmTyp
		{
			get
			{
				return (ENUM_BauteilTyp)PRO_i32Type;
			}
			set
			{
				PRO_i32Type = (int)value;
			}
		}

		public EDC_BauteilData()
		{
		}

		public EDC_BauteilData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBauteilIdWhereStatementErstellen(string i_strBauteilId)
		{
			return string.Format("Where {0} = '{1}'", "PackageId", i_strBauteilId);
		}

		public static string FUN_strPackageNameWhereStatementErstellen(string i_strPackageName)
		{
			return string.Format("Where {0} = '{1}'", "PackageName", i_strPackageName);
		}

		public static string FUN_strPackageNameWhereStatementErstellen(IEnumerable<string> i_enuPackageNamen)
		{
			return string.Format("Where {0} in ({1})", "PackageName", string.Join<string>(",", (IEnumerable<string>)(from i_strVariable in i_enuPackageNamen
			select $"'{i_strVariable}'").ToList()));
		}

		public static string FUN_strLikePackageNameWhereStatementErstellen(string i_strLikePackaName)
		{
			return string.Format("Where Lower({0}) Like Lower('%{1}%')", "PackageName", i_strLikePackaName);
		}
	}
}
