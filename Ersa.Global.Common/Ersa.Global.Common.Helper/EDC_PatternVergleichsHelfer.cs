namespace Ersa.Global.Common.Helper
{
	public static class EDC_PatternVergleichsHelfer
	{
		private const char mC_chrSingleWildcard = '?';

		private const char mC_chrMultipleWildcard = '*';

		public static bool FUN_blnStimmtDerCodeUeberein(string i_strPatternCode, string i_strSuchCode)
		{
			if (string.IsNullOrEmpty(i_strSuchCode))
			{
				return false;
			}
			int[] array = new int[(i_strSuchCode.Length + 1) * (i_strPatternCode.Length + 1)];
			int[] array2 = new int[array.Length];
			int num = -1;
			bool[,] array3 = new bool[i_strSuchCode.Length + 1, i_strPatternCode.Length + 1];
			int num2 = 0;
			int num3 = 0;
			while (num2 < i_strSuchCode.Length && num3 < i_strPatternCode.Length && i_strPatternCode[num3] != '*' && (i_strSuchCode[num2] == i_strPatternCode[num3] || i_strPatternCode[num3] == '?'))
			{
				num2++;
				num3++;
			}
			if (num3 == i_strPatternCode.Length || i_strPatternCode[num3] == '*')
			{
				array3[num2, num3] = true;
				array[++num] = num2;
				array2[num] = num3;
			}
			bool flag = false;
			while (num >= 0 && !flag)
			{
				num2 = array[num];
				num3 = array2[num--];
				if (num2 == i_strSuchCode.Length && num3 == i_strPatternCode.Length)
				{
					flag = true;
					continue;
				}
				for (int i = num2; i < i_strSuchCode.Length; i++)
				{
					int num5 = i;
					int num6 = num3 + 1;
					if (num6 == i_strPatternCode.Length)
					{
						num5 = i_strSuchCode.Length;
					}
					else
					{
						while (num5 < i_strSuchCode.Length && num6 < i_strPatternCode.Length && i_strPatternCode[num6] != '*' && (i_strSuchCode[num5] == i_strPatternCode[num6] || i_strPatternCode[num6] == '?'))
						{
							num5++;
							num6++;
						}
					}
					if (((num6 == i_strPatternCode.Length && num5 == i_strSuchCode.Length) || (num6 < i_strPatternCode.Length && i_strPatternCode[num6] == '*')) && !array3[num5, num6])
					{
						array3[num5, num6] = true;
						array[++num] = num5;
						array2[num] = num6;
					}
				}
			}
			return flag;
		}
	}
}
