using Prism.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Prism.Modularity
{
	public class ModuleInfoGroup : IModuleCatalogItem, IList<ModuleInfo>, ICollection<ModuleInfo>, IEnumerable<ModuleInfo>, IEnumerable, IList, ICollection
	{
		private readonly Collection<ModuleInfo> modules = new Collection<ModuleInfo>();

		public InitializationMode InitializationMode
		{
			get;
			set;
		}

		public string Ref
		{
			get;
			set;
		}

		public int Count => modules.Count;

		public bool IsReadOnly => false;

		public bool IsFixedSize => false;

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (ModuleInfo)value;
			}
		}

		public bool IsSynchronized => ((ICollection)modules).IsSynchronized;

		public object SyncRoot => ((ICollection)modules).SyncRoot;

		public ModuleInfo this[int index]
		{
			get
			{
				return modules[index];
			}
			set
			{
				modules[index] = value;
			}
		}

		public void Add(ModuleInfo item)
		{
			ForwardValues(item);
			modules.Add(item);
		}

		protected void ForwardValues(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			if (moduleInfo.Ref == null)
			{
				moduleInfo.Ref = Ref;
			}
			if (moduleInfo.InitializationMode == InitializationMode.WhenAvailable && InitializationMode != 0)
			{
				moduleInfo.InitializationMode = InitializationMode;
			}
		}

		public void Clear()
		{
			modules.Clear();
		}

		public bool Contains(ModuleInfo item)
		{
			return modules.Contains(item);
		}

		public void CopyTo(ModuleInfo[] array, int arrayIndex)
		{
			modules.CopyTo(array, arrayIndex);
		}

		public bool Remove(ModuleInfo item)
		{
			return modules.Remove(item);
		}

		public IEnumerator<ModuleInfo> GetEnumerator()
		{
			return modules.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		int IList.Add(object value)
		{
			Add((ModuleInfo)value);
			return 1;
		}

		bool IList.Contains(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ModuleInfo moduleInfo = value as ModuleInfo;
			if (moduleInfo == null)
			{
				throw new ArgumentException(Prism.Properties.Resources.ValueMustBeOfTypeModuleInfo, "value");
			}
			return Contains(moduleInfo);
		}

		public int IndexOf(object value)
		{
			return modules.IndexOf((ModuleInfo)value);
		}

		public void Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ModuleInfo moduleInfo = value as ModuleInfo;
			if (moduleInfo == null)
			{
				throw new ArgumentException(Prism.Properties.Resources.ValueMustBeOfTypeModuleInfo, "value");
			}
			modules.Insert(index, moduleInfo);
		}

		void IList.Remove(object value)
		{
			Remove((ModuleInfo)value);
		}

		public void RemoveAt(int index)
		{
			modules.RemoveAt(index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)modules).CopyTo(array, index);
		}

		public int IndexOf(ModuleInfo item)
		{
			return modules.IndexOf(item);
		}

		public void Insert(int index, ModuleInfo item)
		{
			modules.Insert(index, item);
		}
	}
}
