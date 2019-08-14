using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;

namespace Prism.Modularity
{
	public class DirectoryModuleCatalog : ModuleCatalog
	{
		private class InnerModuleInfoLoader : MarshalByRefObject
		{
			internal ModuleInfo[] GetModuleInfos(string path)
			{
				DirectoryInfo directory = new DirectoryInfo(path);
				ResolveEventHandler value = (object sender, ResolveEventArgs args) => OnReflectionOnlyResolve(args, directory);
				AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += value;
				ModuleInfo[] result = GetNotAllreadyLoadedModuleInfos(IModuleType: AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().First((Assembly asm) => asm.FullName == typeof(IModule).Assembly.FullName).GetType(typeof(IModule).FullName), directory: directory).ToArray();
				AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= value;
				return result;
			}

			private static IEnumerable<ModuleInfo> GetNotAllreadyLoadedModuleInfos(DirectoryInfo directory, Type IModuleType)
			{
				List<FileInfo> list = new List<FileInfo>();
				Assembly[] alreadyLoadedAssemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();
				foreach (FileInfo item in from file in directory.GetFiles("*.dll")
				where alreadyLoadedAssemblies.FirstOrDefault((Assembly assembly) => string.Compare(Path.GetFileName(assembly.Location), file.Name, StringComparison.OrdinalIgnoreCase) == 0) == null
				select file)
				{
					try
					{
						Assembly.ReflectionOnlyLoadFrom(item.FullName);
						list.Add(item);
					}
					catch (BadImageFormatException)
					{
					}
				}
				return list.SelectMany(delegate(FileInfo file)
				{
					Type[] exportedTypes = Assembly.ReflectionOnlyLoadFrom(file.FullName).GetExportedTypes();
					Type type2 = IModuleType;
					return from t in exportedTypes.Where(type2.IsAssignableFrom)
					where t != IModuleType
					where !t.IsAbstract
					select t into type
					select CreateModuleInfo(type);
				});
			}

			private static Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
			{
				Assembly assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault((Assembly asm) => string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));
				if (assembly != null)
				{
					return assembly;
				}
				AssemblyName assemblyName = new AssemblyName(args.Name);
				string text = Path.Combine(directory.FullName, assemblyName.Name + ".dll");
				if (File.Exists(text))
				{
					return Assembly.ReflectionOnlyLoadFrom(text);
				}
				return Assembly.ReflectionOnlyLoad(args.Name);
			}

			internal void LoadAssemblies(IEnumerable<string> assemblies)
			{
				foreach (string assembly in assemblies)
				{
					try
					{
						Assembly.ReflectionOnlyLoadFrom(assembly);
					}
					catch (FileNotFoundException)
					{
					}
				}
			}

			private static ModuleInfo CreateModuleInfo(Type type)
			{
				string name = type.Name;
				List<string> list = new List<string>();
				bool flag = false;
				CustomAttributeData customAttributeData = CustomAttributeData.GetCustomAttributes(type).FirstOrDefault((CustomAttributeData cad) => cad.Constructor.DeclaringType.FullName == typeof(ModuleAttribute).FullName);
				if (customAttributeData != null)
				{
					foreach (CustomAttributeNamedArgument namedArgument in customAttributeData.NamedArguments)
					{
						switch (namedArgument.MemberInfo.Name)
						{
						case "ModuleName":
							name = (string)namedArgument.TypedValue.Value;
							break;
						case "OnDemand":
							flag = (bool)namedArgument.TypedValue.Value;
							break;
						case "StartupLoaded":
							flag = !(bool)namedArgument.TypedValue.Value;
							break;
						}
					}
				}
				foreach (CustomAttributeData item in from cad in CustomAttributeData.GetCustomAttributes(type)
				where cad.Constructor.DeclaringType.FullName == typeof(ModuleDependencyAttribute).FullName
				select cad)
				{
					list.Add((string)item.ConstructorArguments[0].Value);
				}
				ModuleInfo obj = new ModuleInfo(name, type.AssemblyQualifiedName)
				{
					InitializationMode = (flag ? InitializationMode.OnDemand : InitializationMode.WhenAvailable),
					Ref = type.Assembly.EscapedCodeBase
				};
				obj.DependsOn.AddRange(list);
				return obj;
			}
		}

		public string ModulePath
		{
			get;
			set;
		}

		protected override void InnerLoad()
		{
			if (string.IsNullOrEmpty(ModulePath))
			{
				throw new InvalidOperationException(Prism.Properties.Resources.ModulePathCannotBeNullOrEmpty);
			}
			if (!Directory.Exists(ModulePath))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.DirectoryNotFound, new object[1]
				{
					ModulePath
				}));
			}
			AppDomain appDomain = BuildChildDomain(AppDomain.CurrentDomain);
			try
			{
				List<string> list = new List<string>();
				IEnumerable<string> collection = from assembly in AppDomain.CurrentDomain.GetAssemblies().Cast<Assembly>().Where(delegate(Assembly assembly)
				{
					if (!(assembly is AssemblyBuilder) && assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder")
					{
						return !string.IsNullOrEmpty(assembly.Location);
					}
					return false;
				})
				select assembly.Location;
				list.AddRange(collection);
				Type typeFromHandle = typeof(InnerModuleInfoLoader);
				if (typeFromHandle.Assembly != null)
				{
					InnerModuleInfoLoader innerModuleInfoLoader = (InnerModuleInfoLoader)appDomain.CreateInstanceFrom(typeFromHandle.Assembly.Location, typeFromHandle.FullName).Unwrap();
					innerModuleInfoLoader.LoadAssemblies(list);
					base.Items.AddRange(innerModuleInfoLoader.GetModuleInfos(ModulePath));
				}
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
		}

		protected virtual AppDomain BuildChildDomain(AppDomain parentDomain)
		{
			if (parentDomain == null)
			{
				throw new ArgumentNullException("parentDomain");
			}
			Evidence securityInfo = new Evidence(parentDomain.Evidence);
			AppDomainSetup setupInformation = parentDomain.SetupInformation;
			return AppDomain.CreateDomain("DiscoveryRegion", securityInfo, setupInformation);
		}
	}
}
