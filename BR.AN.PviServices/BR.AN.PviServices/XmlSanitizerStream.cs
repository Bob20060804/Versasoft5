using System;
using System.IO;
using System.Text;

namespace BR.AN.PviServices
{
	public class XmlSanitizerStream : StreamReader
	{
		private const int EOF = -1;

		public XmlSanitizerStream(string path)
			: base(path)
		{
		}

		public override int Read()
		{
			int num;
			while ((num = base.Read()) != -1 && !IsLegalXmlChar(num))
			{
			}
			return num;
		}

		public override int Peek()
		{
			int num;
			do
			{
				num = base.Peek();
			}
			while (!IsLegalXmlChar(num) && (num = base.Read()) != -1);
			return num;
		}

		public static bool IsLegalXmlChar(int character)
		{
			if (character != 9 && character != 10 && character != 13 && (character < 32 || character > 55295) && (character < 57344 || character > 65533))
			{
				if (character >= 65536)
				{
					return character <= 1114111;
				}
				return false;
			}
			return true;
		}

		public override int Read(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException();
			}
			int num = 0;
			do
			{
				int num2 = Read();
				if (num2 == -1)
				{
					return num;
				}
				buffer[index + num++] = (char)num2;
			}
			while (num < count);
			return num;
		}

		public override int ReadBlock(char[] buffer, int index, int count)
		{
			int num = 0;
			int num2;
			do
			{
				num += (num2 = Read(buffer, index + num, count - num));
			}
			while (num2 > 0 && num < count);
			return num;
		}

		public override string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (true)
			{
				int num = Read();
				switch (num)
				{
				case -1:
					if (stringBuilder.Length > 0)
					{
						return stringBuilder.ToString();
					}
					return null;
				case 10:
				case 13:
					if (num == 13 && Peek() == 10)
					{
						Read();
					}
					return stringBuilder.ToString();
				}
				stringBuilder.Append((char)num);
			}
		}

		public override string ReadToEnd()
		{
			char[] array = new char[4096];
			StringBuilder stringBuilder = new StringBuilder(4096);
			int charCount;
			while ((charCount = Read(array, 0, array.Length)) != 0)
			{
				stringBuilder.Append(array, 0, charCount);
			}
			return stringBuilder.ToString();
		}
	}
}
