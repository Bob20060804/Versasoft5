using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Prozessschreiber
{
	[EDC_TabellenInformation("PRO_strTabellenname", PRO_blnNameIstProperty = true, PRO_strTablespace = "ess5_recorder")]
	public class EDC_SchreiberData
	{
		public const string mC_strTabellenName = "RecorderData";

		public const string mC_strSpalteAufgetreten = "Time";

		private const string mC_strStartzeitpunktParameterName = "pStart";

		private const string mC_strEndzeitpunktParameterName = "pEnde";

		private IEnumerable<EDC_DynamischeSpalte> m_lstDynamischeSpalten = new List<EDC_DynamischeSpalte>();

		public string PRO_strTabellenname => string.Format("{0}_MA{1}", "RecorderData", PRO_i64MaschinenId);

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

		[EDC_SpaltenInformation("Time", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public DateTime PRO_dtmTimeStamp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation(PRO_blnIsDynamischeSpalte = true)]
		public IEnumerable<EDC_DynamischeSpalte> PRO_lstDynamischeSpalten
		{
			get
			{
				return m_lstDynamischeSpalten;
			}
			set
			{
				m_lstDynamischeSpalten = value;
			}
		}

		public EDC_SchreiberData()
		{
		}

		public EDC_SchreiberData(long i_i64MaschinenId)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
		}

		public EDC_SchreiberData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strAufgetretenWhereStatementErstellenMitParameter(DateTime i_fdcStartzeitpunkt, DateTime i_fdcEndzeitpunkt, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			i_fdcDictionary.Add("pEnde", i_fdcEndzeitpunkt);
			return string.Format("Where {0} between @{1} and @{2}", "Time", "pStart", "pEnde");
		}

		public static string FUN_strAufgetretenVorWhereStatementErstellenMitParameter(DateTime i_fdcStartzeitpunkt, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			return string.Format("Where {0} < @{1} order by {0} desc", "Time", "pStart");
		}

		public static string FUN_strDatenVorStartdatumWhereStatementErstellen(DateTime i_fdcStartzeitpunkt, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			return string.Format("Where {0} < @{1}", "Time", "pStart");
		}

		public static string FUN_strLoescheSchreiberdatenVorDatumStatement(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			return $"Delete from {FUN_strBestimmeTabellenName(i_i64MaschinenId)} {FUN_strDatenVorStartdatumWhereStatementErstellen(i_fdcStartzeitpunkt, i_fdcDictionary)}";
		}

		public static string FUN_strSelectCountStatement(long i_i64MaschinenId)
		{
			return FUN_strSelectCountStatement(i_i64MaschinenId, string.Empty);
		}

		public static string FUN_strSelectCountVorStartdatumStatement(DateTime i_fdcStartzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			return FUN_strSelectCountStatement(i_i64MaschinenId, FUN_strDatenVorStartdatumWhereStatementErstellen(i_fdcStartzeitpunkt, i_fdcDictionary));
		}

		public static string FUN_strBestimmeTabellenName(long i_i64MaschinenId)
		{
			return string.Format("{0}_MA{1}", "RecorderData", i_i64MaschinenId);
		}

		private static string FUN_strSelectCountStatement(long i_i64MaschinenId, string i_strWhereStatement)
		{
			return $"Select count(*) from {FUN_strBestimmeTabellenName(i_i64MaschinenId)} {i_strWhereStatement}";
		}
	}
}
