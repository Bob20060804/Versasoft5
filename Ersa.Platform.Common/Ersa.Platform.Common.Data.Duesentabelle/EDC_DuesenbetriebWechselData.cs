using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.Selektiv;
using System;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("NozzleOperatingChanges", PRO_strTablespace = "ess5_production")]
	public class EDC_DuesenbetriebWechselData
	{
		public const string gC_strTabellenName = "NozzleOperatingChanges";

		private const string mC_strSpalteDuesenGuid = "NozzleGuid";

		private const string mC_strSpalteWechselDatum = "ChangeDate";

		private const string mC_strSpalteBenutzerId = "UserId";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteTiegel = "SolderPot";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NozzleGuid", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strDuesenGuid
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeDate", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public DateTime PRO_dtmWechselDatum
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UserId")]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId")]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SolderPot")]
		public long PRO_i64Tiegel
		{
			get;
			set;
		}

		public ENUM_SelektivTiegel PRO_enmTiegel
		{
			get
			{
				return (ENUM_SelektivTiegel)PRO_i64Tiegel;
			}
			set
			{
				PRO_i64Tiegel = (long)value;
			}
		}

		public EDC_DuesenbetriebWechselData()
		{
		}

		public EDC_DuesenbetriebWechselData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleWechselFuerMaschineWhereStatement(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strLoescheBenutzerIdStatementErstellen(long i_i64BenutzerId)
		{
			return string.Format("DELETE FROM {0} Where {1} = {2} ", "NozzleOperatingChanges", "UserId", i_i64BenutzerId);
		}
	}
}
