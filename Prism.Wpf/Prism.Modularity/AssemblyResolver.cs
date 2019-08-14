using Prism.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Prism.Modularity
{
	public class AssemblyResolver : IAssemblyResolver, IDisposable
	{
		private class AssemblyInfo
		{
			public AssemblyName AssemblyName
			{
				get;
				set;
			}

			public Uri AssemblyUri
			{
				get;
				set;
			}

			public Assembly Assembly
			{
				get;
				set;
			}
		}

		private readonly List<AssemblyInfo> registeredAssemblies = new List<AssemblyInfo>();

		private bool handlesAssemblyResolve;

		public void LoadAssemblyFrom(string assemblyFilePath)
		{
			if (!handlesAssemblyResolve)
			{
				AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
				handlesAssemblyResolve = true;
			}
			Uri fileUri = GetFileUri(assemblyFilePath);
			if (fileUri == null)
			{
				throw new ArgumentException(Prism.Properties.Resources.InvalidArgumentAssemblyUri, "assemblyFilePath");
			}
			if (!File.Exists(fileUri.LocalPath))
			{
				throw new FileNotFoundException();
			}
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileUri.LocalPath);
			AssemblyInfo assemblyInfo = registeredAssemblies.FirstOrDefault((AssemblyInfo a) => assemblyName == a.AssemblyName);
			if (assemblyInfo == null)
			{
				assemblyInfo = new AssemblyInfo
				{
					AssemblyName = assemblyName,
					AssemblyUri = fileUri
				};
				registeredAssemblies.Add(assemblyInfo);
			}
		}

		private static Uri GetFileUri(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			if (!Uri.TryCreate(filePath, UriKind.Absolute, out Uri result))
			{
				return null;
			}
			if (!result.IsFile)
			{
				return null;
			}
			return result;
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			AssemblyName assemblyName = new AssemblyName(args.Name);
			AssemblyInfo assemblyInfo = registeredAssemblies.FirstOrDefault((AssemblyInfo a) => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));
			if (assemblyInfo != null)
			{
				if (assemblyInfo.Assembly == null)
				{
					assemblyInfo.Assembly = Assembly.LoadFrom(assemblyInfo.AssemblyUri.LocalPath);
				}
				return assemblyInfo.Assembly;
			}
			return null;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (handlesAssemblyResolve)
			{
				AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
				handlesAssemblyResolve = false;
			}
		}
	}
}
