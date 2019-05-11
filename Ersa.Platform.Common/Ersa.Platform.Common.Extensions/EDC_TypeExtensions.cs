using System;
using System.Reflection;

namespace Ersa.Platform.Common.Extensions
{
	public static class EDC_TypeExtensions
	{
		public static EDC_ElementVersion FUN_edcVersionErmitteln(this Type i_fdcTyp)
		{
			Assembly assembly = i_fdcTyp.Assembly;
			string i_strElementName = assembly.FUN_strAssemblyTitelErmitteln();
			Version i_fdcVersion = assembly.FUN_fdcAssemblyVersionErmitteln();
			return new EDC_ElementVersion(i_strElementName, i_fdcVersion);
		}

		public static string FUN_strAssemblyTitelErmitteln(this Assembly i_fdcAssembly)
		{
			return ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(i_fdcAssembly, typeof(AssemblyTitleAttribute))).Title;
		}

		public static Version FUN_fdcAssemblyVersionErmitteln(this Assembly i_fdcAssembly)
		{
			return new Version(((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(i_fdcAssembly, typeof(AssemblyFileVersionAttribute))).Version);
		}

		public static string FUN_fdcAssemblyProduktErmitteln(this Assembly i_fdcAssembly)
		{
			return ((AssemblyProductAttribute)Attribute.GetCustomAttribute(i_fdcAssembly, typeof(AssemblyProductAttribute))).Product;
		}

		public static string FUN_fdcAssemblyVersionStringErmitteln(this Assembly i_fdcAssembly)
		{
			return i_fdcAssembly.FUN_fdcAssemblyVersionErmitteln().ToString();
		}
	}
}
