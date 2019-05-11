using System;
using System.Linq;
using System.Security.Cryptography;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_TagesPasswortHelfer
	{
		public static bool FUN_blnIstTagesPasswortGueltig(string i_strPasswort, DateTime i_dtmDateTime, byte[] ia_bytUniversalSchluessel)
		{
			string value = i_strPasswort.ToUpper();
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime, ia_bytUniversalSchluessel, 2).Equals(value))
			{
				return true;
			}
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime.AddDays(-1.0), ia_bytUniversalSchluessel, 2).Equals(value))
			{
				return true;
			}
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime.AddDays(1.0), ia_bytUniversalSchluessel, 2).Equals(value))
			{
				return true;
			}
			return false;
		}

		public static bool FUN_blnIstLangesTagesPasswortGueltig(string i_strPasswort, DateTime i_dtmDateTime, byte[] ia_bytUniversalSchluessel)
		{
			string value = i_strPasswort.ToUpper();
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime, ia_bytUniversalSchluessel, 3).Equals(value))
			{
				return true;
			}
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime.AddDays(-1.0), ia_bytUniversalSchluessel, 3).Equals(value))
			{
				return true;
			}
			if (FUN_strHoleTagesSchluessel(i_dtmDateTime.AddDays(1.0), ia_bytUniversalSchluessel, 3).Equals(value))
			{
				return true;
			}
			return false;
		}

		private static byte[] FUNsa_bytHoleMonatsSchluessel(DateTime i_fdcDateTime, byte[] ia_bytUniversalSchluessel)
		{
			byte[] array = new byte[ia_bytUniversalSchluessel.Length];
			array = ia_bytUniversalSchluessel.ToArray();
			array[ia_bytUniversalSchluessel.Length - 4] = (byte)(i_fdcDateTime.Month * 3);
			array[ia_bytUniversalSchluessel.Length - 3] = (byte)(i_fdcDateTime.Year / 256);
			array[ia_bytUniversalSchluessel.Length - 2] = (byte)(i_fdcDateTime.Year % 256);
			array[ia_bytUniversalSchluessel.Length - 1] = (byte)i_fdcDateTime.Month;
			return SHA256.Create().ComputeHash(array, 0, ia_bytUniversalSchluessel.Length);
		}

		private static string FUN_strHoleTagesSchluessel(DateTime i_dtmDateTime, byte[] ia_bytUniversalSchluessel, int i_i32Laenge)
		{
			byte[] array = FUNsa_bytHoleMonatsSchluessel(i_dtmDateTime, ia_bytUniversalSchluessel);
			array[15] = (byte)(i_dtmDateTime.Day * 7);
			return BitConverter.ToString(SHA256.Create().ComputeHash(array, 0, 16), 16 - i_i32Laenge, i_i32Laenge).Replace("-", string.Empty);
		}
	}
}
