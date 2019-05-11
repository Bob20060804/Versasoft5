using System;

namespace BR.AN.PviServices
{
	internal class PviParse
	{
		public static bool TryParseInt32(string s, out int result)
		{
			try
			{
				result = int.Parse(s);
				return true;
			}
			catch
			{
				result = 0;
				return false;
			}
		}

		public static bool TryParseInt16(string s, out short result)
		{
			try
			{
				result = short.Parse(s);
				return true;
			}
			catch
			{
				result = 0;
				return false;
			}
		}

		public static bool TryParseUInt16(string s, out ushort result)
		{
			try
			{
				result = ushort.Parse(s);
				return true;
			}
			catch
			{
				result = 0;
				return false;
			}
		}

		public static bool TryParseUInt32(string s, out uint result)
		{
			try
			{
				result = uint.Parse(s);
				return true;
			}
			catch
			{
				result = 0u;
				return false;
			}
		}

		public static bool TryParseDateTime(string s, out DateTime result)
		{
			try
			{
				result = DateTime.Parse(s);
				return true;
			}
			catch
			{
				result = DateTime.MinValue;
				return false;
			}
		}

		public static bool TryParseDouble(string s, out double result)
		{
			try
			{
				result = double.Parse(s);
				return true;
			}
			catch
			{
				result = 0.0;
				return false;
			}
		}

		public static bool TryParseByte(string s, out byte result)
		{
			try
			{
				result = byte.Parse(s);
				return true;
			}
			catch
			{
				result = 0;
				return false;
			}
		}
	}
}
