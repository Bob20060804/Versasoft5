using Ersa.Global.DataProvider.Datenbanktypen;
using System;
using System.Collections.Specialized;
using System.IO;

namespace Ersa.Global.DataProvider.Helfer
{
	/// <summary>
	/// 数据库设置帮助类
	/// Database settings helper
	/// </summary>
	public static class EDC_DatenbankEinstellungenHelfer
	{
        /// <summary>
        /// Get Database Name
        /// </summary>
        /// <param name="i_fdcEinstellungen"></param>
        /// <returns></returns>
		public static string FUN_strHoleDatenbankName(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["Database"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			string path = i_fdcEinstellungen["DataDirectory"];
			string arg = i_fdcEinstellungen["DataSource"];
			string path2 = string.Format("{0}{1}", arg, ".sdf");
			string text2 = Path.Combine(path, path2);
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			return string.Empty;
		}

		/// <summary>
		/// 取消服务器名
		/// </summary>
		/// <param name="i_fdcEinstellungen"></param>
		/// <returns></returns>
		public static string FUN_strHoleServerName(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["Server"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static int FUN_i32HolePortNummer(NameValueCollection i_fdcEinstellungen)
		{
			string value = i_fdcEinstellungen["Port"];
			if (!string.IsNullOrEmpty(value))
			{
				return Convert.ToInt32(value);
			}
			return -1;
		}

		public static string FUN_strHoleDatenbankBenutzerName(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["UserId"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static string FUN_strHoleDatenbankKennwort(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["Password"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static string FUN_strHoleDatenbankSource(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["DataSource"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static string FUN_strHoleDatenbankVerzeichnis(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["DataDirectory"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static string FUN_strHoleDatenbankBinaerVerzeichnis(NameValueCollection i_fdcEinstellungen)
		{
			string text = i_fdcEinstellungen["BinaryDirectory"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static ENUM_DatenbankTyp FUN_enmHoleDatenbankTyp(NameValueCollection i_fdcEinstellungen)
		{
			return EDC_ProviderConverterHelfer.FUN_enmDatenbankTypErmitteln(i_fdcEinstellungen["ProviderName"]);
		}

		public static NameValueCollection FUN_fdcErstelleServerEinstellungen(NameValueCollection i_fdcDatenbankEinstellungen)
		{
			NameValueCollection nameValueCollection = new NameValueCollection(i_fdcDatenbankEinstellungen);
			nameValueCollection.Remove("Database");
			return nameValueCollection;
		}
	}
}
