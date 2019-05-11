using System;
using System.IO;
using System.Reflection;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_BuildDateHelfer
	{
		private const int mC_i32PeHeaderOffset = 60;

		private const int mC_i32LinkerTimestampOffset = 8;

		public static DateTime FUN_dtmGetLinkerTime(this Assembly i_fdcAssembly, TimeZoneInfo i_fdcTarget = null)
		{
			string location = i_fdcAssembly.Location;
			byte[] array = new byte[2048];
			using (FileStream fileStream = new FileStream(location, FileMode.Open, FileAccess.Read))
			{
				fileStream.Read(array, 0, 2048);
			}
			int num = BitConverter.ToInt32(array, 60);
			int num2 = BitConverter.ToInt32(array, num + 8);
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(num2);
			TimeZoneInfo destinationTimeZone = i_fdcTarget ?? TimeZoneInfo.Local;
			return TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
		}
	}
}
