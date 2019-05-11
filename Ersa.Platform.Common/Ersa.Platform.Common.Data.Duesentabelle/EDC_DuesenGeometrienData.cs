using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("NozzleGeometries", PRO_strTablespace = "ess5_programs")]
	public class EDC_DuesenGeometrienData
	{
		public const long gC_i64SonderGeometrieId = 1000L;

		public const string gC_strTabellenName = "NozzleGeometries";

		protected const string mC_strSpalteGeometrieId = "GeomertyId";

		private const string mC_strSpalteInnenDurchmesser = "InnerDiameter";

		private const string mC_strSpalteAussenDurchmesser = "OuterDiameter";

		private const string mC_strSpalteHoehe = "Height";

		private const string mC_strSpalteSichtbar = "Visible";

		private const string mC_strSpalteSonder = "Special";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GeomertyId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64GeometrieId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("InnerDiameter", PRO_blnIsRequired = true)]
		public float PRO_sngInnenDurchmesser
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OuterDiameter", PRO_blnIsRequired = true)]
		public float PRO_sngAussenDurchmesser
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Height", PRO_blnIsRequired = true)]
		public float PRO_sngHoehe
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Visible", PRO_blnIsRequired = true)]
		public bool PRO_blnIstSichtbar
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Special", PRO_blnIsRequired = true)]
		public bool PRO_blnIstSonder
		{
			get;
			set;
		}

		public EDC_DuesenGeometrienData()
		{
		}

		public EDC_DuesenGeometrienData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strGeometrienSichtbarFilterWhereStatement()
		{
			return string.Format("Where {0} = '1' order by {1},{2},{3}", "Visible", "InnerDiameter", "OuterDiameter", "Height");
		}

		public static string FUN_strGeometrienSubselectFilterWhereStatement(string i_strSubselect)
		{
			return string.Format("Where {0} in ({1}) order by {2},{3},{4}", "GeomertyId", i_strSubselect, "InnerDiameter", "OuterDiameter", "Height");
		}

		public static string FUN_strErstelleLoescheTabellenInhaltAtatement()
		{
			return string.Format("Delete from {0}", "NozzleGeometries");
		}

		public static IEnumerable<string> FUN_enuHoleDefaultGeometrieDatenErstellungsStatementListe()
		{
			List<string> list = FUN_enuHoleStandardGeometrieDatenErstellungsStatementListe().ToList();
			IEnumerable<string> collection = FUN_enuHoleSonderGeometrieDatenErstellungsStatementListe();
			list.AddRange(collection);
			return list;
		}

		public static IEnumerable<string> FUN_enuHoleStandardGeometrieDatenErstellungsStatementListe()
		{
			return new List<string>
			{
				FUN_edcErstelleDefaultGeometrie(1L, 2.0, 4.5, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(2L, 3.0, 6.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(3L, 4.0, 8.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(4L, 6.0, 10.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(5L, 8.0, 12.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(6L, 10.0, 14.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(7L, 18.0, 20.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(8L, 21.0, 25.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(9L, 28.0, 32.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(10L, 31.0, 35.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(12L, 1.7999999523162842, 2.5, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(13L, 2.0, 3.5, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(14L, 2.0, 4.5, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(15L, 2.0, 4.5, 65.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(16L, 2.4000000953674316, 5.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(17L, 2.5, 4.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(18L, 2.5, 5.0, 57.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(19L, 2.5, 5.0, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(20L, 2.5999999046325684, 5.5, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(21L, 2.7000000476837158, 5.5, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(22L, 2.5999999046325684, 5.5999999046325684, 85.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(23L, 2.7999999523162842, 5.5999999046325684, 85.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(24L, 3.0, 5.5999999046325684, 85.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(25L, 3.5, 5.5999999046325684, 85.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(26L, 3.0, 4.5, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(27L, 3.0, 4.5, 59.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(28L, 3.0, 6.0, 40.5, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(29L, 3.0, 6.0, 52.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(30L, 3.0, 6.0, 57.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(31L, 3.0, 6.0, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(32L, 3.0, 6.0, 64.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(33L, 3.0, 6.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(34L, 3.0, 6.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(35L, 3.5, 6.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(36L, 3.5, 7.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(37L, 3.5, 7.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(38L, 3.5, 7.0, 85.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(39L, 3.5, 10.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(40L, 4.0, 6.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(41L, 4.0, 8.0, 40.5, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(42L, 4.0, 8.0, 56.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(43L, 4.0, 8.0, 57.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(44L, 4.0, 8.0, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(45L, 4.0, 8.0, 65.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(46L, 4.0, 8.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(47L, 4.0, 8.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(48L, 4.0, 8.0, 90.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(49L, 5.0, 8.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(50L, 5.0, 9.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(51L, 6.0, 8.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(52L, 6.0, 10.0, 57.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(53L, 6.0, 10.0, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(54L, 6.0, 10.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(55L, 6.0, 10.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(56L, 7.0, 11.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(57L, 8.0, 12.0, 57.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(58L, 8.0, 12.0, 62.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(59L, 8.0, 12.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(60L, 8.0, 12.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(61L, 9.0, 13.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(63L, 10.0, 14.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(64L, 10.0, 14.0, 80.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(65L, 12.0, 16.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(66L, 14.0, 18.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(67L, 16.0, 20.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(68L, 17.0, 19.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(69L, 18.0, 22.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(71L, 23.0, 25.0, 75.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(72L, 25.0, 29.0, 47.0, i_blnIstSichtbar: true),
				FUN_edcErstelleDefaultGeometrie(73L, 25.0, 29.0, 69.0, i_blnIstSichtbar: true)
			};
		}

		public static IEnumerable<string> FUN_enuHoleSonderGeometrieDatenErstellungsStatementListe()
		{
			return new List<string>
			{
				FUN_edcErstelleDefaultGeometrie(1000L, 0.0, 0.0, 0.0, i_blnIstSichtbar: true, i_blnIstSonder: true),
				FUN_edcErstelleDefaultGeometrie(1001L, 70.0, 90.0, 47.0, i_blnIstSichtbar: true, i_blnIstSonder: true)
			};
		}

		private static string FUN_edcErstelleDefaultGeometrie(long i_i64GeometrieId, double i_dblInnenDurchnesser, double i_dblAussenDurchmesser, double i_dblHoehe, bool i_blnIstSichtbar, bool i_blnIstSonder = false)
		{
			return string.Format("Insert into {0} ({1},{2},{3},{4},{5},{6}) values ({7},{8},{9},{10},{11},{12})", "NozzleGeometries", "GeomertyId", "InnerDiameter", "OuterDiameter", "Height", "Visible", "Special", i_i64GeometrieId, Convert.ToString(i_dblInnenDurchnesser, CultureInfo.GetCultureInfo("en")), Convert.ToString(i_dblAussenDurchmesser, CultureInfo.GetCultureInfo("en")), Convert.ToString(i_dblHoehe, CultureInfo.GetCultureInfo("en")), i_blnIstSichtbar ? "'1'" : "'0'", i_blnIstSonder ? "'1'" : "'0'");
		}
	}
}
