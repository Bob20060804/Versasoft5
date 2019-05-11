using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.Selektiv;
using System;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("NozzleOperatingActualValues", PRO_strTablespace = "ess5_production")]
	public class EDC_DuesenbetriebWerteData
	{
		public const string gC_strTabellenName = "NozzleOperatingActualValues";

		private const string mC_strSpalteDuesenGuid = "NozzleGuid";

		private const string mC_strSpalteGeometrieId = "GeomertyId";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteTiegel = "SolderPot";

		private const string mC_strSpalteDuesenStartDatum = "UseStartDate";

		private const string mC_strSpalteDuesenEndeDatum = "UseEndDate";

		private const string mC_strEinschaltDauer = "WaveOnTime";

		private const string mC_strAussschaltDauer = "WaveOffTime";

		private const string mC_strGesamtDauer = "TotalTime";

		private const string mC_strAktivierungsanzahl = "DispenceNumber";

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

		[EDC_SpaltenInformation("GeomertyId")]
		public long PRO_i64GeometrieId
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

		[EDC_SpaltenInformation("UseStartDate")]
		public DateTime? PRO_dtmDuesenStartDatum
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UseEndDate")]
		public DateTime? PRO_dtmDuesenEndeDatum
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("WaveOnTime")]
		public long PRO_i64WelleEinZeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("WaveOffTime")]
		public long PRO_i64WelleAusZeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TotalTime")]
		public long PRO_i64GesamtZeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DispenceNumber")]
		public long PRO_i64AnzahlAktivierungen
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

		public EDC_DuesenbetriebWerteData()
		{
		}

		public EDC_DuesenbetriebWerteData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleAktiveDusenFuerMaschineWhereStatement(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} and {2} is null order by {3},{4}", "MachineId", i_i64MaschinenId, "UseEndDate", "SolderPot", "GeomertyId");
		}

		public static string FUN_strHoleAktiveDusenWhereStatement(long i_i64MaschinenId, long i_i64GeomertieId, ENUM_SelektivTiegel i_enmTiegel)
		{
			return string.Format("Where {0} = {1} and {2} = {3}  and {4} = {5} and {6} is null", "MachineId", i_i64MaschinenId, "GeomertyId", i_i64GeomertieId, "SolderPot", (long)i_enmTiegel, "UseEndDate");
		}

		public static string FUN_strLoescheGeomertieIdStatementErstellen(long i_i64GeomertieId)
		{
			return string.Format("DELETE FROM {0} Where {1} = {2} ", "NozzleOperatingActualValues", "GeomertyId", i_i64GeomertieId);
		}
	}
}
