using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("CyclicMessageLib", PRO_strTablespace = "ess5_messages")]
	public class EDC_ZyklischeMeldungVorlageGruppeData
	{
		public const string mC_strTabellenName = "CyclicMessageLib";

		public const string mC_strSpalteVorlageGruppeId = "CyclicLibId";

		public const string mC_strSpalteName = "Name";

		public const string mC_strSpalteErstellungsDatum = "CreationDate";

		public const string mC_strSpalteErstellungsBenutzer = "CreationUser";

		public const string mC_strSpalteAenderungsDatum = "ChangeDate";

		public const string mC_strSpalteAenderungsBenutzer = "ChangeUser";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CyclicLibId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64VorlageGruppeId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Name", PRO_i32Length = 100, PRO_blnIsRequired = true)]
		public string PRO_strName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_fdcErstellungsDatum
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationUser")]
		public long PRO_i64ErstellungsBenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeDate")]
		public DateTime PRO_fdcAenderungsDatum
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeUser")]
		public long PRO_i64AenderungsBenutzerId
		{
			get;
			set;
		}

		public EDC_ZyklischeMeldungVorlageGruppeData()
		{
		}

		public EDC_ZyklischeMeldungVorlageGruppeData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVorlageGruppenIdWhereStatementErstellen(int i_i32MeldungsId)
		{
			return string.Format("Where {0} = {1}", "CyclicLibId", i_i32MeldungsId);
		}
	}
}
