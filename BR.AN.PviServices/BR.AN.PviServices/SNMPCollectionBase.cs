using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class SNMPCollectionBase : SNMPBase, ICollection, IEnumerable
	{
		internal Hashtable propItems;

		protected int propRequestCount;

		protected bool propRequesting;

		public int Count => propItems.Count;

		public ICollection Values => propItems.Values;

		public ICollection Keys => propItems.Keys;

		public virtual bool IsSynchronized => propItems.IsSynchronized;

		public virtual object SyncRoot => propItems.SyncRoot;

		public event CollectionErrorEventHandler Changed;

		internal SNMPCollectionBase(string name, SNMPBase parentObj)
			: base(name, parentObj)
		{
			propItems = new Hashtable();
			propRequestCount = 0;
			propRequesting = false;
		}

		public override void Cleanup()
		{
			propItems.Clear();
			base.Cleanup();
		}

		public bool ContainsKey(string key)
		{
			return propItems.ContainsKey(key);
		}

		internal void Add(string key, object value)
		{
			propItems.Add(key, value);
		}

		internal void Remove(string key)
		{
			propItems.Remove(key);
		}

		protected virtual void OnChanged(CollectionErrorEventArgs e)
		{
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			return propItems.GetEnumerator();
		}

		public virtual void CopyTo(Array array, int arrayIndex)
		{
			propItems.CopyTo(array, arrayIndex);
		}
	}
}
