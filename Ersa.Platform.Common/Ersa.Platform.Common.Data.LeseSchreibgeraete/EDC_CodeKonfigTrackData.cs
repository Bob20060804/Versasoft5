using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.LeseSchreibgeraete
{
	[EDC_TabellenInformation("CodeConfigurationTrackings", PRO_strTablespace = "ess5_production")]
	public class EDC_CodeKonfigTrackData
	{
		public const string gC_strTabellenName = "CodeConfigurationTrackings";

		private const string mC_strSpalteTrackId = "TrackId";

		private const string mC_strSpalteArrayIndex = "ArrayIndex";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteIstKonfiguriert = "IsConfigured";

		private const string mC_strSpalteWirdVerwendet = "IsActive";

		private const string mC_strSpalteOrt = "Location";

		private const string mC_strSpalteSpur = "Track";

		private const string mC_strSpalteFunktion = "CodeFunction";

		private const string mC_strSpalteAlb = "UseAlb";

		private const string mC_strSpalteElb = "UseElb";

		private const string mC_strSpalteTimeout = "Timeout";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		private const string mC_strSpalteAngelegtVon = "CreationUser";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TrackId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64TrackId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ArrayIndex", PRO_blnIsRequired = true)]
		public long PRO_i64ArrayIndex
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

		[EDC_SpaltenInformation("IsConfigured", PRO_blnIsRequired = true)]
		public bool PRO_blnIstKonfiguriert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsActive")]
		public bool PRO_blnWirdVerwendet
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Location")]
		public long PRO_i64Ort
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Track")]
		public long PRO_i64Spur
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeFunction")]
		public long PRO_i64Verwendung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UseAlb")]
		public bool PRO_blnAlbVerwenden
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UseElb")]
		public bool PRO_blnElbVerwenden
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Timeout")]
		public long PRO_i64Timeout
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

		public EDC_CodeKonfigTrackData()
		{
		}

		public EDC_CodeKonfigTrackData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenIdUndArrayIndexWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex);
		}

		public static string FUN_strMaschinenIdSelectCountStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodeConfigurationTrackings", FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId));
		}

		public static string FUN_strMaschinenIdUndArrayIndexSelectCountStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodeConfigurationTrackings", FUN_strMaschinenIdUndArrayIndexWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex));
		}
	}
}
