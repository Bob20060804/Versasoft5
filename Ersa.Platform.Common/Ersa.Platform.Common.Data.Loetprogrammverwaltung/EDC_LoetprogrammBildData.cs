using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramImages", PRO_strTablespace = "ess5_images")]
	public class EDC_LoetprogrammBildData
	{
		public const string gC_strTabellenName = "ProgramImages";

		public const string gC_strSpalteProgrammId = "ProgramId";

		public const string gC_strSpalteBild = "Image";

		public const string gC_strSpalteDateiname = "FileName";

		public const string gC_strSpalteZusatzinfo1 = "Metainfo1";

		public const string gC_strSpalteZusatzinfo2 = "Metainfo2";

		public const string gC_strSpalteZusatzinfo3 = "Metainfo3";

		public const string gC_strSpalteZusatzinfo4 = "Metainfo4";

		public const string gC_strSpalteZusatzinfo5 = "Metainfo5";

		public const string gC_strSpalteZusatzinfo6 = "Metainfo6";

		private const string mC_strSpalteVerwendung = "Purpose";

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

		[EDC_SpaltenInformation("Metainfo1", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Metainfo2", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Metainfo3", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo3
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Metainfo4", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo4
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Metainfo5", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo5
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Metainfo6", PRO_i32Length = 50)]
		public string PRO_strZusatzinfo6
		{
			get;
			set;
		}

		public ENUM_BildVerwendung PRO_enmVerwendung
		{
			get
			{
				return (ENUM_BildVerwendung)PRO_i32Verwendung;
			}
			set
			{
				PRO_i32Verwendung = (int)value;
			}
		}

		public EDC_LoetprogrammBildData()
		{
		}

		public EDC_LoetprogrammBildData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdMitAllenVerwendungenWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strProgrammIdMitVerwendungenWhereStatementErstellen(long i_i64ProgrammId, IEnumerable<int> i_lstVerwendungen)
		{
			string text = string.Empty;
			foreach (int item in i_lstVerwendungen)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += item;
			}
			return string.Format("Where {0} = {1} and {2} in ({3})", "ProgramId", i_i64ProgrammId, "Purpose", text);
		}

		public static string FUN_strProgrammIdUndVerwendungWhereStatementErstellen(long i_i64ProgrammId, int i_i32Verwendung)
		{
			return string.Format("Where {0} = {1} AND {2} = {3}", "ProgramId", i_i64ProgrammId, "Purpose", i_i32Verwendung);
		}
	}
}
