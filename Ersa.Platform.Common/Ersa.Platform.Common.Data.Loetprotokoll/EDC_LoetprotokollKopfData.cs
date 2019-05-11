using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	[EDC_TabellenInformation("PRO_strTabellenname", PRO_blnNameIstProperty = true, PRO_strTablespace = "ess5_protocol")]
	public class EDC_LoetprotokollKopfData
	{
		public const string gC_strTabellenName = "ProtocolHeads";

		public const string gC_strSpalteProtokollId = "ProtocolId";

		public const string gC_strSpalteLoetprogrammVersionsId = "HistoryId";

		public const string gC_strSpalteArbeitsversion = "Draft";

		public const string gC_strSpalteEingangszeit = "EntryTime";

		public const string gC_strSpalteAusganzszeit = "OutletTime";

		public const string gC_strSpalteBenutzerId = "UserId";

		public const string gC_strSpalteLoetgutSchlecht = "Faulty";

		public const string gC_strSpalteLaufendeNummer = "SerialBoardNumber";

		public const string gC_strSpalteModus = "Mode";

		private const string mC_strStartzeitpunktParameterName = "pStart";

		private const string mC_strEndzeitpunktParameterName = "pEnde";

		public static string PRO_strTabellennameMitMaschinenPrefix => string.Format("{0}_ma", "ProtocolHeads".ToLower());

		public string PRO_strTabellenname => string.Format("{0}_MA{1}", "ProtocolHeads", PRO_i64MaschinenId);

		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProtocolId", PRO_blnIsRequired = true, PRO_blnIsPrimary = true)]
		public long PRO_i64ProtokollId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("HistoryId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64LoetprogrammVersionsId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Draft")]
		public bool PRO_blnArbeitsversion
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EntryTime", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public DateTime PRO_dtmEingangszeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OutletTime", PRO_blnIsNonUniqueIndex = true)]
		public DateTime PRO_dtmAusgangszeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UserId", PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Faulty")]
		public bool PRO_blnLoetgutSchlecht
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SerialBoardNumber")]
		public long PRO_i64LaufendeNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Mode")]
		public long PRO_i64Modus
		{
			get;
			set;
		}

		public ENUM_LoetprogrammModus PRO_enmModus
		{
			get
			{
				return (ENUM_LoetprogrammModus)PRO_i64Modus;
			}
			set
			{
				PRO_i64Modus = (long)value;
			}
		}

		public EDC_LoetprotokollKopfData()
		{
		}

		public EDC_LoetprotokollKopfData(long i_i64MaschinenId)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
		}

		public EDC_LoetprotokollKopfData(long i_i64MaschinenId, string i_strWhereStatement)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strLoetprogrammVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strErfasstZwischenWhereStatementErstellen(DateTime i_fdcStartzeitpunkt, DateTime i_fdcEndzeitpunkt, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			i_fdcDictionary.Add("pEnde", i_fdcEndzeitpunkt);
			return string.Format("Where {0} between @{1} and @{2}", "OutletTime", "pStart", "pEnde");
		}

		public static string FUN_strProtokollIdHistoryIdWhereStatementErstellen(long i_i64ProtokollId, long i_i64LoetprogrammVersionsId)
		{
			return string.Format("Where {0} = {1} And {2} = {3}", "ProtocolId", i_i64ProtokollId, "HistoryId", i_i64LoetprogrammVersionsId);
		}

		public override bool Equals(object i_edcO)
		{
			EDC_LoetprotokollKopfData eDC_LoetprotokollKopfData = i_edcO as EDC_LoetprotokollKopfData;
			if (eDC_LoetprotokollKopfData == null)
			{
				return false;
			}
			if (eDC_LoetprotokollKopfData.PRO_i64ProtokollId == PRO_i64ProtokollId)
			{
				return eDC_LoetprotokollKopfData.PRO_i64LoetprogrammVersionsId == PRO_i64LoetprogrammVersionsId;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return PRO_i64ProtokollId.GetHashCode() * PRO_i64LoetprogrammVersionsId.GetHashCode();
		}
	}
}
