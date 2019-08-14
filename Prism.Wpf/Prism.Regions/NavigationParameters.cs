using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Prism.Regions
{
	public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		private readonly List<KeyValuePair<string, object>> entries = new List<KeyValuePair<string, object>>();

		public object this[string key]
		{
			get
			{
				foreach (KeyValuePair<string, object> entry in entries)
				{
					if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
					{
						return entry.Value;
					}
				}
				return null;
			}
		}

		public NavigationParameters()
		{
		}

		public NavigationParameters(string query)
		{
			if (query == null)
			{
				return;
			}
			int length = query.Length;
			for (int i = (query.Length > 0 && query[0] == '?') ? 1 : 0; i < length; i++)
			{
				int num = i;
				int num2 = -1;
				for (; i < length; i++)
				{
					switch (query[i])
					{
					case '=':
						if (num2 < 0)
						{
							num2 = i;
						}
						continue;
					default:
						continue;
					case '&':
						break;
					}
					break;
				}
				string text = null;
				string text2 = null;
				if (num2 >= 0)
				{
					text = query.Substring(num, num2 - num);
					text2 = query.Substring(num2 + 1, i - num2 - 1);
				}
				else
				{
					text2 = query.Substring(num, i - num);
				}
				Add((text != null) ? Uri.UnescapeDataString(text) : null, Uri.UnescapeDataString(text2));
				if (i == length - 1 && query[i] == '&')
				{
					Add(null, "");
				}
			}
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(string key, object value)
		{
			entries.Add(new KeyValuePair<string, object>(key, value));
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entries.Count > 0)
			{
				stringBuilder.Append('?');
				bool flag = true;
				foreach (KeyValuePair<string, object> entry in entries)
				{
					if (!flag)
					{
						stringBuilder.Append('&');
					}
					else
					{
						flag = false;
					}
					stringBuilder.Append(Uri.EscapeDataString(entry.Key));
					stringBuilder.Append('=');
					stringBuilder.Append(Uri.EscapeDataString(entry.Value.ToString()));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
