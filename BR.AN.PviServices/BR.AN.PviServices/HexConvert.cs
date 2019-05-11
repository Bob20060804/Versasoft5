using System;

namespace BR.AN.PviServices
{
	public class HexConvert
	{
		private static char[] hexDigits = new char[16]
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
			'F'
		};

		public static byte[] ToBytesNoSwap(string hex)
		{
			byte b = 0;
			byte b2 = 0;
			char c = '0';
			char c2 = '0';
			byte[] array = null;
			if (0 < hex.Length)
			{
				string text = hex.ToUpper();
				int num = text.Length / 2;
				if (text.Length % 2 == 1)
				{
					num++;
					text.Insert(0, "0");
				}
				array = new byte[num];
				int num2 = 0;
				for (int i = 0; i < text.Length; i++)
				{
					b = 0;
					b2 = 0;
					c = Convert.ToChar(text.Substring(i, 1));
					if (PviMarshal.HexCharToByte(c, ref b))
					{
						c2 = '0';
						if (text.Length > i + 1)
						{
							c2 = Convert.ToChar(text.Substring(i + 1, 1));
						}
						PviMarshal.HexCharToByte(c2, ref b2);
						array[num2] = (byte)(b * 16 + b2);
						num2++;
						i++;
					}
					else
					{
						b = 0;
					}
				}
			}
			return array;
		}

		public static byte[] ToBytes(string hex)
		{
			string text = hex.ToUpper();
			int num = text.Length / 2;
			byte b = 0;
			byte b2 = 0;
			char c = '0';
			char c2 = '0';
			if (text.Length % 2 == 1)
			{
				num++;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				b = 0;
				b2 = 0;
				c = Convert.ToChar(text.Substring(text.Length - 1 - i, 1));
				if (PviMarshal.HexCharToByte(c, ref b))
				{
					c2 = '0';
					if (text.Length - 2 - i >= 0)
					{
						c2 = Convert.ToChar(text.Substring(text.Length - 2 - i, 1));
					}
					while (!PviMarshal.HexCharToByte(c2, ref b2))
					{
						i++;
						b2 = 0;
						if (text.Length - 2 - i >= 0)
						{
							c2 = Convert.ToChar(text.Substring(text.Length - 2 - i, 1));
						}
					}
					array[num2] = (byte)(b + b2 * 16);
					num2++;
					i++;
				}
				else
				{
					b = 0;
				}
			}
			byte[] array2 = new byte[num2];
			int num3 = 0;
			for (num3 = 0; num3 < array2.Length / 2; num3++)
			{
				byte b3 = array[num3];
				array2[num3] = array[array2.Length - num3 - 1];
				array2[array2.Length - num3 - 1] = b3;
			}
			if (array2.Length % 2 == 1)
			{
				array2[num3] = array[num3];
			}
			array = null;
			return array2;
		}

		public static string ToHexString(byte[] bytes)
		{
			char[] array = new char[bytes.Length * 2];
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = bytes[i];
				array[i * 2] = hexDigits[num >> 4];
				array[i * 2 + 1] = hexDigits[num & 0xF];
			}
			return new string(array);
		}
	}
}
