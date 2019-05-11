using System;
using System.Security.Cryptography;
using System.Text;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_HashHelfer
	{
		private static readonly Random ms_fdcRandom = new Random();

		public static string FUN_strHashErzeugen(string i_strKey, string i_strSalt)
		{
			if (i_strKey == null)
			{
				throw new ArgumentNullException("i_strKey");
			}
			if (i_strSalt == null)
			{
				throw new ArgumentNullException("i_strSalt");
			}
			return FUN_strHashAusKeyUndSaltErzeugen(i_strKey, i_strSalt);
		}

		public static bool FUN_blnIstHashGueltig(string i_strKey, string i_strSalt, string i_strHash)
		{
			if (i_strKey == null)
			{
				throw new ArgumentNullException("i_strKey");
			}
			if (i_strSalt == null)
			{
				throw new ArgumentNullException("i_strSalt");
			}
			if (i_strHash == null)
			{
				throw new ArgumentNullException("i_strHash");
			}
			return FUN_strHashAusKeyUndSaltErzeugen(i_strKey, i_strSalt) == i_strHash;
		}

		public static string FUN_strZufaelligenSaltErzeugen()
		{
			return ms_fdcRandom.Next().ToString();
		}

		private static string FUN_strHashAusKeyUndSaltErzeugen(string i_strKey, string i_strSalt)
		{
			string s = i_strKey + i_strSalt;
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			return Convert.ToBase64String(new SHA1Managed().ComputeHash(bytes));
		}
	}
}
