using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Codetabelle
{
	[EDC_TabellenInformation("CodeTables", PRO_strTablespace = "ess5_programs")]
	public class EDC_CodetabelleData
	{
		public const string gC_strTabellenName = "CodeTables";

		public const string gC_strSpalteCodetabellenId = "CodeTableId";

		public const string gC_strSpalteGruppenId = "GroupId";

		private const string mC_strSpalteName = "Name";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		private const string mC_strSpalteAngelegtVon = "CreationUser";

		private const string mC_strSpalteBearbeitetAm = "ChangeDate";

		private const string mC_strSpalteBearbeitetVon = "ChangeUser";

		private const string mC_strBearbeitetAmParameterName = "pBearbeitetAm";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeTableId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64CodetabellenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GroupId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64GruppenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Name", PRO_i32Length = 100)]
		public string PRO_strName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationUser")]
		public long PRO_i64AngelegtVon
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeDate")]
		public DateTime PRO_dtmBearbeitetAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeUser")]
		public long PRO_i64BearbeitetVon
		{
			get;
			set;
		}

		public EDC_CodetabelleData()
		{
		}

		public EDC_CodetabelleData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strCodetabellenIdWhereStatementErstellen(long i_i64CodetabellenId)
		{
			return string.Format("Where {0} = {1}", "CodeTableId", i_i64CodetabellenId);
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} in (Select {1} from {2} where {3} = {4})", "GroupId", "GroupId", "MachineGroupMembers", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strNameMaschinenIdWhereStatementErstellen(string i_strName, long i_i64MaschinenId)
		{
			return string.Format("Where {0} = '{1}' AND  {2} in (Select {3} from {4} where {5} = {6})", "Name", i_strName, "GroupId", "GroupId", "MachineGroupMembers", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strUpdateStatementErstellen(long i_i64CodeTabellenId, string i_strNeuerName, long i_i64BenutzerId, DateTime i_dtmAenderungsZeitpunkt, Dictionary<string, object> i_dicParameterDictionary)
		{
			i_dicParameterDictionary.Add("pBearbeitetAm", i_dtmAenderungsZeitpunkt);
			return string.Format("Update {0} Set {1} = '{2}', {3} = {4}, {5} = @{6} where {7} = {8}", "CodeTables", "Name", i_strNeuerName, "ChangeUser", i_i64BenutzerId, "ChangeDate", "pBearbeitetAm", "CodeTableId", i_i64CodeTabellenId);
		}
	}
}
