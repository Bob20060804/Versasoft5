#define TRACE
using Ersa.Global.Common.Extensions;
using System;
using System.Diagnostics;

namespace Ersa.Global.Common
{
	public static class EDC_Utility
	{
		private const int mC_i32JahrMin = 1970;

		private const int mC_i32JahrMax = 2100;

		private const int mC_i32MonatMin = 1;

		private const int mC_i32MonatMax = 12;

		private const int mC_i32TagMin = 1;

		private const int mC_i32TagMax = 31;

		private const int mC_i32TageMin = 0;

		private const int mC_i32TageMax = 36500;

		private const int mC_i32StundeMin = 0;

		private const int mC_i32StundeMax = 23;

		private const int mC_i32MinuteMin = 0;

		private const int mC_i32MinuteMax = 59;

		private const int mC_i32SekundeMin = 0;

		private const int mC_i32SekundeMax = 59;

		public static bool FUN_blnKannValidesDateTimeErstellen(int i_i32Jahr, int i_i32Monat, int i_i32Tag, int i_i32Stunde, int i_i32Minute, int i_i32Sekunde)
		{
			if (FUN_blnKannValidesDateTimeErstellen(i_i32Jahr, i_i32Monat, i_i32Tag) && i_i32Stunde.FUN_blnLiegtZwischen(0, 23) && i_i32Minute.FUN_blnLiegtZwischen(0, 59))
			{
				return i_i32Sekunde.FUN_blnLiegtZwischen(0, 59);
			}
			return false;
		}

		public static bool FUN_blnKannValidesDateTimeErstellen(int i_i32Jahr, int i_i32Monat, int i_i32Tag)
		{
			if (i_i32Jahr.FUN_blnLiegtZwischen(1970, 2100) && i_i32Monat.FUN_blnLiegtZwischen(1, 12))
			{
				return i_i32Tag.FUN_blnLiegtZwischen(1, 31);
			}
			return false;
		}

		public static bool FUN_blnKannValidesTimeSpanErstellen(int i_i32Stunde, int i_i32Minute, int i_i32Sekunde)
		{
			if (i_i32Stunde.FUN_blnLiegtZwischen(0, 23) && i_i32Minute.FUN_blnLiegtZwischen(0, 59))
			{
				return i_i32Sekunde.FUN_blnLiegtZwischen(0, 59);
			}
			return false;
		}

		public static bool FUN_blnKannValidesTimeSpanErstellen(int i_i32Tage, int i_i32Stunde, int i_i32Minute, int i_i32Sekunde)
		{
			if (i_i32Tage.FUN_blnLiegtZwischen(0, 36500))
			{
				return FUN_blnKannValidesTimeSpanErstellen(i_i32Stunde, i_i32Minute, i_i32Sekunde);
			}
			return false;
		}

		public static DateTime FUN_sttTaeglichenUhrSyncZeitpunktErstellen(int i_i32Stunden, int i_i32Minuten)
		{
			DateTime now = DateTime.Now;
			DateTime date = now.Date;
			int num = 0;
			if (now.Hour > i_i32Stunden)
			{
				num = 1;
			}
			if (now.Hour == i_i32Stunden && now.Minute >= i_i32Minuten)
			{
				num = 1;
			}
			return date.AddHours(i_i32Stunden).AddMinutes(i_i32Minuten).AddDays(num);
		}

		public static string FUN_strZeitMitMillisek(DateTime i_fdcDateTime)
		{
			return $"{i_fdcDateTime:T}.{i_fdcDateTime.Millisecond:D3}";
		}

		public static void SUB_ConsoleLog(string i_strInfo, string i_strValue1 = "", string i_strValue2 = "")
		{
			string text = "#" + FUN_strZeitMitMillisek(DateTime.Now) + "# " + i_strInfo + " Val1 = " + i_strValue1;
			if (!string.IsNullOrWhiteSpace(i_strValue2))
			{
				text = text + " Val2 = " + i_strValue2;
			}
			Trace.WriteLine(text);
		}
	}
}
