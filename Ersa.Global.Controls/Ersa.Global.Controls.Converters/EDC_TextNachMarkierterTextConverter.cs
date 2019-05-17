using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_TextNachMarkierterTextConverter : IMultiValueConverter
	{
		public Style PRO_objMarkierterTextStyle
		{
			get;
			set;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 0)
			{
				string text = values[0] as string;
				if (values.Length > 1)
				{
					string text2 = values[1] as string;
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						IEnumerable<string> enumerable = FUN_lstTextZerteilen(text, text2);
						TextBlock textBlock = new TextBlock();
						{
							foreach (string item in enumerable)
							{
								textBlock.Inlines.Add(item.Equals(text2, StringComparison.OrdinalIgnoreCase) ? new Run(item)
								{
									Style = (PRO_objMarkierterTextStyle ?? new Style())
								} : new Run(item));
							}
							return textBlock;
						}
					}
				}
				return new TextBlock
				{
					Text = text
				};
			}
			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<string> FUN_lstTextZerteilen(string i_strText, string i_strSuchText)
		{
			IList<string> list = new List<string>();
			if (string.IsNullOrEmpty(i_strSuchText))
			{
				if (i_strText.Length > 0)
				{
					list.Add(i_strText);
				}
			}
			else
			{
				while (i_strText.Length > 0)
				{
					int num = i_strText.IndexOf(i_strSuchText, StringComparison.OrdinalIgnoreCase);
					if (num > -1)
					{
						if (num > 0)
						{
							string item = i_strText.Substring(0, num);
							list.Add(item);
						}
						list.Add(i_strText.Substring(num, i_strSuchText.Length));
						i_strText = i_strText.Remove(0, num + i_strSuchText.Length);
					}
					else
					{
						list.Add(i_strText);
						i_strText = string.Empty;
					}
				}
			}
			return list;
		}
	}
}
