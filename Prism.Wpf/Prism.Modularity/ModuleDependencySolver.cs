using Prism.Common;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prism.Modularity
{
	public class ModuleDependencySolver
	{
		private readonly ListDictionary<string, string> dependencyMatrix = new ListDictionary<string, string>();

		private readonly List<string> knownModules = new List<string>();

		public int ModuleCount => dependencyMatrix.Count;

		public void AddModule(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StringCannotBeNullOrEmpty, new object[1]
				{
					"name"
				}));
			}
			AddToDependencyMatrix(name);
			AddToKnownModules(name);
		}

		public void AddDependency(string dependingModule, string dependentModule)
		{
			if (string.IsNullOrEmpty(dependingModule))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StringCannotBeNullOrEmpty, new object[1]
				{
					"dependingModule"
				}));
			}
			if (string.IsNullOrEmpty(dependentModule))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StringCannotBeNullOrEmpty, new object[1]
				{
					"dependentModule"
				}));
			}
			if (!knownModules.Contains(dependingModule))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.DependencyForUnknownModule, new object[1]
				{
					dependingModule
				}));
			}
			AddToDependencyMatrix(dependentModule);
			dependencyMatrix.Add(dependentModule, dependingModule);
		}

		private void AddToDependencyMatrix(string module)
		{
			if (!dependencyMatrix.ContainsKey(module))
			{
				dependencyMatrix.Add(module);
			}
		}

		private void AddToKnownModules(string module)
		{
			if (!knownModules.Contains(module))
			{
				knownModules.Add(module);
			}
		}

		public string[] Solve()
		{
			List<string> list = new List<string>();
			while (list.Count < dependencyMatrix.Count)
			{
				List<string> list2 = FindLeaves(list);
				if (list2.Count == 0 && list.Count < dependencyMatrix.Count)
				{
					throw new CyclicDependencyFoundException(Prism.Properties.Resources.CyclicDependencyFound);
				}
				list.AddRange(list2);
			}
			list.Reverse();
			if (list.Count > knownModules.Count)
			{
				string text = FindMissingModules(list);
				throw new ModularityException(text, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.DependencyOnMissingModule, new object[1]
				{
					text
				}));
			}
			return list.ToArray();
		}

		private string FindMissingModules(List<string> skip)
		{
			string text = "";
			foreach (string item in skip)
			{
				if (!knownModules.Contains(item))
				{
					text += ", ";
					text += item;
				}
			}
			return text.Substring(2);
		}

		private List<string> FindLeaves(List<string> skip)
		{
			List<string> list = new List<string>();
			foreach (string key in dependencyMatrix.Keys)
			{
				if (!skip.Contains(key))
				{
					int num = 0;
					foreach (string item in dependencyMatrix[key])
					{
						if (!skip.Contains(item))
						{
							num++;
						}
					}
					if (num == 0)
					{
						list.Add(key);
					}
				}
			}
			return list;
		}
	}
}
