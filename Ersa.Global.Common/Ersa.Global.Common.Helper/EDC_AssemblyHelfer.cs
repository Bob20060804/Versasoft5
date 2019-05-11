using System;
using System.IO;
using System.Reflection;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_AssemblyHelfer
	{
		public static string FUN_strHoleAktuellenAssemblyPfad()
		{
			return Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
		}
	}
}
