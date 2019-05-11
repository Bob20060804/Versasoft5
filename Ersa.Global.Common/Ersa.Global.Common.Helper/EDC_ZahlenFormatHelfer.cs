using System;
using System.Globalization;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_ZahlenFormatHelfer
	{
		public static double FUN_dblNachkommaWert(string i_strWert)
		{
			if (string.IsNullOrWhiteSpace(i_strWert))
			{
				return 0.0;
			}
			if (i_strWert == "-")
			{
				return 0.0;
			}
			if (i_strWert == ".")
			{
				return 0.0;
			}
			if (!FUN_blnIstZahl(i_strWert))
			{
				return 0.0;
			}
			return double.Parse(i_strWert.Replace(',', '.'), CultureInfo.InvariantCulture);
		}

		public static string FUN_strWert(double i_dblWert, int i_i32AnzahlNachkommaZahlen)
		{
			if (i_i32AnzahlNachkommaZahlen < 1)
			{
				if (i_dblWert >= 2147483647.0)
				{
					return int.MaxValue.ToString(CultureInfo.InvariantCulture);
				}
				if (i_dblWert <= -2147483648.0)
				{
					return int.MinValue.ToString(CultureInfo.InvariantCulture);
				}
				return ((int)Math.Round(i_dblWert)).ToString(CultureInfo.InvariantCulture);
			}
			string format = "F" + i_i32AnzahlNachkommaZahlen.ToString(CultureInfo.InvariantCulture);
			return i_dblWert.ToString(format, CultureInfo.InvariantCulture);
		}

		public static bool FUN_blnUngleich(float i_sngA, float i_sngB, int i_i32Nachkomma)
		{
			decimal d = Math.Round((decimal)i_sngA, i_i32Nachkomma);
			decimal d2 = Math.Round((decimal)i_sngB, i_i32Nachkomma);
			return d != d2;
		}

		public static bool FUN_blnUngleich(float? i_sngA, float i_sngB, int i_i32Nachkomma)
		{
			if (!i_sngA.HasValue)
			{
				return true;
			}
			decimal d = Math.Round((decimal)i_sngA.Value, i_i32Nachkomma);
			decimal d2 = Math.Round((decimal)i_sngB, i_i32Nachkomma);
			return d != d2;
		}

		public static bool FUN_blnHatDezimaltrenner(string i_strWert)
		{
			if (string.IsNullOrWhiteSpace(i_strWert))
			{
				return false;
			}
			if (i_strWert.Contains("."))
			{
				return true;
			}
			if (i_strWert.Contains(","))
			{
				return true;
			}
			return false;
		}

		public static string FUN_strZeitAusSekunden(int i_i32Sekunden)
		{
			if (i_i32Sekunden != 0)
			{
				return $"{i_i32Sekunden / 60:D2}:{i_i32Sekunden % 60:D2}";
			}
			return "00:00";
		}

		public static bool FUN_blnIstZahl(string i_strZahl)
		{
			if (string.IsNullOrWhiteSpace(i_strZahl))
			{
				return false;
			}
			double result;
			return double.TryParse(i_strZahl.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}
	}
}
