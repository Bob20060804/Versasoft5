using System.Collections;

namespace BR.AN.PviServices
{
	public class HashtableArray
	{
		internal ArrayList propArrayList;

		internal Hashtable propHashTable;

		public virtual int Count => propArrayList.Count;

		public object this[object key]
		{
			get
			{
				return propArrayList[(int)propHashTable[key]];
			}
		}

		public object this[int index]
		{
			get
			{
				return propArrayList[index];
			}
		}

		public ICollection Keys => propHashTable.Keys;

		public HashtableArray()
		{
			propArrayList = new ArrayList(1);
			propHashTable = new Hashtable();
		}

		public void Remove(object key)
		{
			if (ContainsKey(key))
			{
				propArrayList.RemoveAt((int)propHashTable[key]);
				propHashTable.Remove(key);
			}
		}

		public int Add(object key, object value)
		{
			int num = -1;
			if (!propHashTable.ContainsKey(key))
			{
				num = propArrayList.Add(value);
				propHashTable.Add(key, num);
			}
			return num;
		}

		public void Clear()
		{
			propArrayList.Clear();
			propHashTable.Clear();
		}

		public virtual IEnumerator GetEnumerator()
		{
			return propArrayList.GetEnumerator();
		}

		public bool ContainsKey(object key)
		{
			return propHashTable.ContainsKey(key);
		}

		public virtual object Clone()
		{
			HashtableArray hashtableArray = new HashtableArray();
			hashtableArray.propHashTable = (Hashtable)propHashTable.Clone();
			hashtableArray.propArrayList = (ArrayList)propArrayList.Clone();
			return hashtableArray;
		}
	}
}
