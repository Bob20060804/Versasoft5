using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Benutzer
{
	[EDC_TabellenInformation("UserTrackings")]
	public class EDC_BenutzerTrackData
	{
		public const string gC_strTabellenName = "UserTrackings";

		public const string gC_strSpalteTrackingId = "TrackId";

		public const string gC_strSpalteMachineId = "MachineId";

		private const string mC_strSpalteBenutzerId = "UserId";

		private const string mC_strSpalteZeitpunkt = "Time";

		private const string mC_strSpalteAktivitaet = "Activity";

		public const string gC_strSpalteParameter = "Parameter";

		public const string gC_strSpalteAlterWert = "OldValue";

		public const string gC_strSpalteNeuerWert = "NewValue";

		private const string mC_strStartzeitpunktParameterName = "pStart";

		private const string mC_strEndzeitpunktParameterName = "pEnde";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TrackId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64TrackingId
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

		[EDC_SpaltenInformation("Time", PRO_blnIsRequired = true)]
		public DateTime PRO_dtmZeitpunkt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Activity", PRO_i32Length = 8)]
		public string PRO_strAktivitaet
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter", PRO_i32Length = 200)]
		public string PRO_strParameter
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OldValue", PRO_i32Length = 200)]
		public string PRO_strAlterWert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NewValue", PRO_i32Length = 200)]
		public string PRO_strNeuerWert
		{
			get;
			set;
		}

		public EDC_BenutzerTrackData()
		{
		}

		public EDC_BenutzerTrackData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strErstellIntervallWhereStatementErstellen(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, DateTime i_fdcEndzeitpunkt, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			i_fdcDictionary.Add("pEnde", i_fdcEndzeitpunkt);
			return string.Format("{0} and {1} between @{2} and @{3}", FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId), "Time", "pStart", "pEnde");
		}
	}
}
