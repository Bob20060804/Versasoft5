using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("Messages", PRO_strTablespace = "ess5_messages", PRO_strNonUniqueCombinedIndex = "MachineId,Occurred")]
	public class EDC_MeldungData
	{
		public const string mC_strTabellenName = "Messages";

		public const string mC_strSpalteBenutzerId = "UserId";

		public const string mC_strSpalteMeldungsId = "MessageId";

		public const string mC_strSpalteMachineId = "MachineId";

		public const string mC_strSpalteQuittiert = "Acknowledged";

		public const string mC_strSpalteAufgetreten = "Occurred";

		public const string mC_strSpalteBetriebsart = "OperationMode";

		public const string mC_strSpalteMeldungsTyp = "MessageType";

		public const string mC_strSpalteMeldungsSchluessel = "MessageKey";

		public const string mC_strSpalteOrt1Schluessel = "Facility1Key";

		public const string mC_strSpalteOrt2Schluessel = "Facility2Key";

		public const string mC_strSpalteOrt3Schluessel = "Facility3Key";

		public const string mC_strSpalteProduzent = "System";

		public const string mC_strSpalteCode = "Code";

		public const string mC_strSpalteZurueckgestellt = "MessageReset";

		public const string mC_strSpaltePossibleActions = "PossibleActions";

		public const string mC_strSpalteRequestedActions = "RequestedActions";

		private const string mC_strStartzeitpunktParameterName = "pStart";

		private const string mC_strTabellenIndex = "MachineId,Occurred";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strMeldungsId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Occurred", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public DateTime? PRO_fdcAufgetreten
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Acknowledged")]
		public DateTime? PRO_fdcQuittiert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UserId", PRO_blnIsRequired = true)]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OperationMode", PRO_blnIsRequired = true)]
		public int PRO_i32Betriebsart
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageType", PRO_blnIsRequired = true)]
		public int PRO_i32MeldungsTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageKey", PRO_blnIsRequired = true)]
		public int PRO_i32MeldungsNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility1Key", PRO_blnIsRequired = true)]
		public int PRO_i32MeldungsOrt1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility2Key", PRO_blnIsRequired = true)]
		public int PRO_i32MeldungsOrt2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility3Key", PRO_blnIsRequired = true)]
		public int PRO_i32MeldungsOrt3
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("System")]
		public int PRO_i32MeldungProduzent
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Code")]
		public int PRO_i32ProduzentenCode
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageReset")]
		public DateTime? PRO_fdcZurueckgestellt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PossibleActions", PRO_i32Length = 30)]
		public string PRO_strMoeglicheAktionen
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RequestedActions", PRO_i32Length = 30)]
		public string PRO_strProzessAktionen
		{
			get;
			set;
		}

		public ENUM_MeldungsTypen PRO_enmMeldungsTyp
		{
			get
			{
				return (ENUM_MeldungsTypen)PRO_i32MeldungsTyp;
			}
			set
			{
				PRO_i32MeldungsTyp = (int)value;
			}
		}

		public ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get
			{
				return (ENUM_MeldungProduzent)PRO_i32MeldungProduzent;
			}
			set
			{
				PRO_i32MeldungProduzent = (int)value;
			}
		}

		public IEnumerable<ENUM_ProzessAktionen> PRO_enuProzessAktionen
		{
			get
			{
				List<ENUM_ProzessAktionen> list = new List<ENUM_ProzessAktionen>();
				if (!string.IsNullOrEmpty(PRO_strProzessAktionen))
				{
					string[] array = PRO_strProzessAktionen.Split(new string[1]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in array)
					{
						list.Add((ENUM_ProzessAktionen)int.Parse(s));
					}
				}
				return list;
			}
			set
			{
				PRO_strProzessAktionen = string.Empty;
				if (value != null && value.Any())
				{
					PRO_strProzessAktionen = string.Join(",", (from i_enmWert in value
					select (int)i_enmWert).ToArray());
				}
			}
		}

		public IEnumerable<ENUM_MeldungAktionen> PRO_enuMoeglicheAktionen
		{
			get
			{
				List<ENUM_MeldungAktionen> list = new List<ENUM_MeldungAktionen>();
				if (!string.IsNullOrEmpty(PRO_strMoeglicheAktionen))
				{
					string[] array = PRO_strMoeglicheAktionen.Split(new string[1]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in array)
					{
						list.Add((ENUM_MeldungAktionen)int.Parse(s));
					}
				}
				return list;
			}
			set
			{
				PRO_strMoeglicheAktionen = string.Empty;
				if (value != null && value.Any())
				{
					PRO_strMoeglicheAktionen = string.Join(",", (from i_enmWert in value
					select (int)i_enmWert).ToArray());
				}
			}
		}

		public EDC_MeldungData()
		{
		}

		public EDC_MeldungData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMeldungsIdWhereStatementErstellen(string i_strMeldungsId)
		{
			return string.Format("Where {0} = '{1}'", "MessageId", i_strMeldungsId);
		}

		public static string FUN_strMeldeTypWhereStatementErstellen(int i_i32Meldetyp, long i_i64MachineId)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MessageType", i_i32Meldetyp, "MachineId", i_i64MachineId);
		}

		public static string FUN_strDatumQuittiertVorStartdatumWhereStatementErstellen(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			return string.Format("Where {0} < @{1} and {2} = {3} ", "Acknowledged", "pStart", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strDatumAufgetretenNachStartdatumWhereStatementErstellen(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			return string.Format("Where {0} > @{1} and {2} = {3} ", "Occurred", "pStart", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} ", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strLoescheMeldungenQuittiertVorDatumStatement(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			return string.Format("Delete from {0} {1} ", "Messages", FUN_strDatumQuittiertVorStartdatumWhereStatementErstellen(i_fdcStartzeitpunkt, i_i64MaschinenId, i_fdcDictionary));
		}

		public static string FUN_strErstelleSelectCountStatement(string i_strWhere)
		{
			return string.Format("Select Count(*) from {0} {1}", "Messages", i_strWhere);
		}

		public static string FUN_strErstelleSelectCountVorDatumStatement(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			return FUN_strErstelleSelectCountStatement(FUN_strDatumQuittiertVorStartdatumWhereStatementErstellen(i_fdcStartzeitpunkt, i_i64MaschinenId, i_fdcDictionary));
		}

		public static string FUN_strErstelleSelectMaxMeldungenStatement(long i_i64MaschinenId)
		{
			return string.Format("Select Max({0}) from {1} {2}", "Acknowledged", "Messages", FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId));
		}

		public static string FUN_strExterneMeldungenDefaultUpdateStatementErstellen()
		{
			return string.Format("Update {0} Set {1} = {2}, {3} = 0,  {4} = {5} Where {6} is null", "Messages", "System", 1, "Code", "PossibleActions", 1, "System");
		}
	}
}
