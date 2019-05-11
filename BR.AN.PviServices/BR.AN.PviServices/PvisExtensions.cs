using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BR.AN.PviServices
{
	internal static class PvisExtensions
	{
		internal static string RemoveAsciiControlChars(this string instance)
		{
			if (instance == null)
			{
				return null;
			}
			instance = Regex.Replace(instance, "[^\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD\\u10000-\\u10FFFF]", "");
			char[] anyOf = new char[33]
			{
				'\0',
				'\u0001',
				'\u0002',
				'\u0003',
				'\u0004',
				'\u0005',
				'\u0006',
				'\a',
				'\b',
				'\t',
				'\n',
				'\v',
				'\f',
				'\r',
				'\u000e',
				'\u000f',
				'\u0010',
				'\u0011',
				'\u0012',
				'\u0013',
				'\u0014',
				'\u0015',
				'\u0016',
				'\u0017',
				'\u0018',
				'\u0019',
				'\u001a',
				'\u001b',
				'\u001c',
				'\u001d',
				'\u001e',
				'\u001f',
				'\\'
			};
			int num = instance.IndexOfAny(anyOf);
			while (-1 != num)
			{
				instance = instance.Remove(num, 1);
				num = instance.IndexOfAny(anyOf);
			}
			return instance;
		}

		internal static string RemoveAsciiControl1FChars(this string instance)
		{
			if (instance == null)
			{
				return null;
			}
			char[] anyOf = new char[33]
			{
				'\0',
				'\u0001',
				'\u0002',
				'\u0003',
				'\u0004',
				'\u0005',
				'\u0006',
				'\a',
				'\b',
				'\t',
				'\n',
				'\v',
				'\f',
				'\r',
				'\u000e',
				'\u000f',
				'\u0010',
				'\u0011',
				'\u0012',
				'\u0013',
				'\u0014',
				'\u0015',
				'\u0016',
				'\u0017',
				'\u0018',
				'\u0019',
				'\u001a',
				'\u001b',
				'\u001c',
				'\u001d',
				'\u001e',
				'\u001f',
				'\\'
			};
			int num = instance.IndexOfAny(anyOf);
			while (-1 != num)
			{
				instance = instance.Remove(num, 1);
				num = instance.IndexOfAny(anyOf);
			}
			return instance;
		}

		internal static string ConvertToAsciiCompatible(this string instance)
		{
			if (instance == null)
			{
				return null;
			}
			string text = "";
			for (int i = 0; i < instance.Length; i++)
			{
				if ('Ã' != instance[i] && 'Â' != instance[i])
				{
					text += instance[i];
					continue;
				}
				i++;
				if (i < instance.Length && ' ' != instance[i])
				{
					text += instance[i];
				}
			}
			instance = text;
			return instance;
		}

		internal static string Utf16ToUtf8(this string instance)
		{
			string text = string.Empty;
			byte[] bytes = Encoding.Unicode.GetBytes(instance);
			byte[] array = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
			for (int i = 0; i < array.Length; i++)
			{
				byte[] value = new byte[2]
				{
					array[i],
					0
				};
				text += BitConverter.ToChar(value, 0);
			}
			return text;
		}
	}
}
