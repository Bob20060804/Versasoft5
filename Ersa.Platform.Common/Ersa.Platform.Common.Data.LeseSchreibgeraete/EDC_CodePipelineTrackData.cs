using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.LeseSchreibgeraete
{
	[EDC_TabellenInformation("CodePipelineTrackings", PRO_strTablespace = "ess5_production")]
	public class EDC_CodePipelineTrackData
	{
		public const string gC_strTabellenName = "CodePipelineTrackings";

		private const string mC_strSpalteTrackId = "TrackId";

		private const string mC_strSpalteArrayIndex = "ArrayIndex";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteZweig = "BranchNr";

		private const string mC_strSpalteElement = "PipeElement";

		private const string mC_strSpalteInhalt = "Content";

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

		[EDC_SpaltenInformation("MachineId")]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BranchNr")]
		public long PRO_i64Zweig
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PipeElement")]
		public long PRO_i64PipelineElement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Content")]
		public string PRO_strInhalt
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

		public EDC_CodePipelineTrackData()
		{
		}

		public EDC_CodePipelineTrackData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = {5}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex, "BranchNr", i_i64Zweig);
		}

		public static string FUN_strPipelineSelectCountStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodePipelineTrackings", FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex, i_i64Zweig));
		}
	}
}
