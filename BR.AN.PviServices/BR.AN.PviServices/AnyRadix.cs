using System;

namespace BR.AN.PviServices
{
	public class AnyRadix : ICustomFormatter, IFormatProvider
	{
		private const string radixCode = "Ra";

		private static char[] rDigits = new char[36]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z'
		};

		public object GetFormat(Type argType)
		{
			if (argType == typeof(ICustomFormatter))
			{
				return this;
			}
			return null;
		}

		public string Format(string formatString, object argToBeFormatted, IFormatProvider provider)
		{
			if (formatString == null || !formatString.Trim().StartsWith("Ra"))
			{
				if (argToBeFormatted is IFormattable)
				{
					return ((IFormattable)argToBeFormatted).ToString(formatString, provider);
				}
				return argToBeFormatted.ToString();
			}
			int num = 0;
			char[] array = new char[63];
			formatString = formatString.Replace("Ra", "");
			long num2;
			try
			{
				num2 = Convert.ToInt64(formatString);
			}
			catch (System.Exception innerException)
			{
				throw new ArgumentException($"The radix \"{formatString}\" is invalid.", innerException);
			}
			if (num2 < 2 || num2 > 36)
			{
				throw new ArgumentException($"The radix \"{formatString}\" is not in the range 2..36.");
			}
			long num3;
			try
			{
				num3 = (long)argToBeFormatted;
			}
			catch (System.Exception innerException2)
			{
				throw new ArgumentException($"The argument \"{argToBeFormatted}\" cannot be converted to an integer value.", innerException2);
			}
			long num4 = Math.Abs(num3);
			for (num = 0; num <= 64; num++)
			{
				if (num4 == 0)
				{
					break;
				}
				array[array.Length - num - 1] = rDigits[num4 % num2];
				num4 /= num2;
			}
			if (num3 < 0)
			{
				array[array.Length - num++ - 1] = '-';
			}
			return new string(array, array.Length - num, num);
		}
	}
}
