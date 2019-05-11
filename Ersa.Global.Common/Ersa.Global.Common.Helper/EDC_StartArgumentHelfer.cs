using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_StartArgumentHelfer
	{
		public static IDictionary<string, string> FUN_dicInDictionaryUmwandeln(StartupEventArgs i_fdcArgs, string i_strOptionsKennzeichnung = "-")
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string text = null;
			string text2 = null;
			IEnumerable<string> enumerable = i_fdcArgs?.Args;
			foreach (string item in enumerable ?? Enumerable.Empty<string>())
			{
				if (item.StartsWith(i_strOptionsKennzeichnung))
				{
					if (!string.IsNullOrEmpty(text))
					{
						dictionary.Add(text, text2);
					}
					text = item.Substring(1);
					text2 = null;
				}
				else
				{
					text2 = (text2 ?? item);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				dictionary.Add(text, text2);
			}
			return dictionary;
		}

		public static NameValueCollection FUN_fdcInNameValueCollectionUmwandeln(string i_strOptionswerte, string i_strTrennzeichen = ";", string i_strZuweisungszeichen = "=")
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			string[] array = i_strOptionswerte.Split(new string[1]
			{
				i_strTrennzeichen
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new string[1]
				{
					i_strZuweisungszeichen
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 2)
				{
					nameValueCollection.Add(array2[0], array2[1]);
				}
			}
			return nameValueCollection;
		}

		public static bool FUN_blnWertErmitteln(this IDictionary<string, string> i_dicStartArgumente, string i_strKey, out long i_i64Wert)
		{
			i_i64Wert = 0L;
			if (!i_dicStartArgumente.TryGetValue(i_strKey, out string value))
			{
				return false;
			}
			return long.TryParse(value, out i_i64Wert);
		}
	}
}
