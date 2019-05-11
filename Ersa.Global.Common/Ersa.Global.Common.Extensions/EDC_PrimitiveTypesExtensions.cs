using System;

namespace Ersa.Global.Common.Extensions
{
	public static class EDC_PrimitiveTypesExtensions
	{
		public static bool FUN_blnLiegtZwischen(this int i_i32Wert, int i_i32MinWert, int i_i32MaxWert)
		{
			if (i_i32MinWert <= i_i32Wert)
			{
				return i_i32Wert <= i_i32MaxWert;
			}
			return false;
		}

		public static DateTime FUN_sttMapToDateTimeOrDefault(this string i_strDateTimeString)
		{
			if (!DateTime.TryParse(i_strDateTimeString, out DateTime result))
			{
				return new DateTime(1970, 1, 1, 0, 0, 0);
			}
			return result;
		}

		public static string FUN_strLeft(this string i_strOriginal, int i_i32Laenge)
		{
			return i_strOriginal.Substring(0, Math.Min(i_i32Laenge, i_strOriginal.Length));
		}

		public static string FUN_strToDatumStringFuerDefaultDateiNamen(this DateTime i_dtmZeitpunkt)
		{
			return $"{i_dtmZeitpunkt:yyyyMMdd}";
		}

		public static string FUN_strToDatumUndZeitStringFuerDefaultDateiNamen(this DateTime i_dtmZeitpunkt)
		{
			return $"{i_dtmZeitpunkt:yyyyMMddHHmmss}";
		}

		public static string FUN_strZeitMitTicks(this DateTime i_dtmZeitpunkt)
		{
			return $"{i_dtmZeitpunkt:HH:mm:ss}.{i_dtmZeitpunkt.Millisecond}.{i_dtmZeitpunkt.Ticks}";
		}
	}
}
