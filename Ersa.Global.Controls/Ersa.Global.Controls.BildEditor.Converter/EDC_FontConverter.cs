using System;
using System.Windows;

namespace Ersa.Global.Controls.BildEditor.Converter
{
	public static class EDC_FontConverter
	{
		public static string FUN_strFontStyleToString(FontStyle i_fdcFontStyle)
		{
			string result = string.Empty;
			try
			{
				result = new FontStyleConverter().ConvertToString(i_fdcFontStyle);
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
		}

		public static FontStyle FUN_fdcFontStyleFromString(string i_strFontStyle)
		{
			FontStyle result = FontStyles.Normal;
			try
			{
				object obj = new FontStyleConverter().ConvertFromString(i_strFontStyle);
				if (obj == null)
				{
					return result;
				}
				result = (FontStyle)obj;
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
			catch (FormatException)
			{
				return result;
			}
		}

		public static string FUN_strFontWeightToString(FontWeight i_fdcFontWeight)
		{
			string result = string.Empty;
			try
			{
				result = new FontWeightConverter().ConvertToString(i_fdcFontWeight);
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
		}

		public static FontWeight FUN_fdcFontWeightFromString(string i_strFontWeight)
		{
			FontWeight result = FontWeights.Normal;
			try
			{
				object obj = new FontWeightConverter().ConvertFromString(i_strFontWeight);
				if (obj == null)
				{
					return result;
				}
				result = (FontWeight)obj;
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
			catch (FormatException)
			{
				return result;
			}
		}

		public static string FUN_strFontStretchToString(FontStretch i_fdcFontStretch)
		{
			string result = string.Empty;
			try
			{
				result = new FontStretchConverter().ConvertToString(i_fdcFontStretch);
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
		}

		public static FontStretch FUN_fdcFontStretchFromString(string i_strFontStretch)
		{
			FontStretch result = FontStretches.Normal;
			try
			{
				object obj = new FontStretchConverter().ConvertFromString(i_strFontStretch);
				if (obj == null)
				{
					return result;
				}
				result = (FontStretch)obj;
				return result;
			}
			catch (NotSupportedException)
			{
				return result;
			}
			catch (FormatException)
			{
				return result;
			}
		}
	}
}
