using System;
using System.Collections;
using System.Collections.Generic;

namespace Prism.Common
{
	public sealed class ListDictionary<TKey, TValue> : IDictionary<TKey, IList<TValue>>, ICollection<KeyValuePair<TKey, IList<TValue>>>, IEnumerable<KeyValuePair<TKey, IList<TValue>>>, IEnumerable
	{
		private Dictionary<TKey, IList<TValue>> innerValues = new Dictionary<TKey, IList<TValue>>();

		public IList<TValue> Values
		{
			get
			{
				List<TValue> list = new List<TValue>();
				foreach (IList<TValue> value in innerValues.Values)
				{
					list.AddRange(value);
				}
				return list;
			}
		}

		public ICollection<TKey> Keys => innerValues.Keys;

		public IList<TValue> this[TKey key]
		{
			get
			{
				if (!innerValues.ContainsKey(key))
				{
					innerValues.Add(key, new List<TValue>());
				}
				return innerValues[key];
			}
			set
			{
				innerValues[key] = value;
			}
		}

		public int Count => innerValues.Count;

		ICollection<IList<TValue>> IDictionary<TKey, IList<TValue>>.Values
		{
			get
			{
				return innerValues.Values;
			}
		}

		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).IsReadOnly;
			}
		}

		public void Add(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			CreateNewList(key);
		}

		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (innerValues.ContainsKey(key))
			{
				innerValues[key].Add(value);
			}
			else
			{
				CreateNewList(key).Add(value);
			}
		}

		private List<TValue> CreateNewList(TKey key)
		{
			List<TValue> list = new List<TValue>();
			innerValues.Add(key, list);
			return list;
		}

		public void Clear()
		{
			innerValues.Clear();
		}

		public bool ContainsValue(TValue value)
		{
			foreach (KeyValuePair<TKey, IList<TValue>> innerValue in innerValues)
			{
				if (innerValue.Value.Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return innerValues.ContainsKey(key);
		}

		public IEnumerable<TValue> FindAllValuesByKey(Predicate<TKey> keyFilter)
		{
			foreach (KeyValuePair<TKey, IList<TValue>> item in (IEnumerable<KeyValuePair<TKey, IList<TValue>>>)this)
			{
				if (keyFilter(item.Key))
				{
					foreach (TValue item2 in item.Value)
					{
						yield return item2;
					}
				}
			}
		}

		public IEnumerable<TValue> FindAllValues(Predicate<TValue> valueFilter)
		{
			foreach (KeyValuePair<TKey, IList<TValue>> item in (IEnumerable<KeyValuePair<TKey, IList<TValue>>>)this)
			{
				foreach (TValue item2 in item.Value)
				{
					if (valueFilter(item2))
					{
						yield return item2;
					}
				}
			}
		}

		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return innerValues.Remove(key);
		}

		public void Remove(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (innerValues.ContainsKey(key))
			{
				((List<TValue>)innerValues[key]).RemoveAll((TValue item) => value.Equals(item));
			}
		}

		public void Remove(TValue value)
		{
			foreach (KeyValuePair<TKey, IList<TValue>> innerValue in innerValues)
			{
				Remove(innerValue.Key, value);
			}
		}

		void IDictionary<TKey, IList<TValue>>.Add(TKey key, IList<TValue> value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			innerValues.Add(key, value);
		}

		bool IDictionary<TKey, IList<TValue>>.TryGetValue(TKey key, out IList<TValue> value)
		{
			value = this[key];
			return true;
		}

		void ICollection<KeyValuePair<TKey, IList<TValue>>>.Add(KeyValuePair<TKey, IList<TValue>> item)
		{
			((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Add(item);
		}

		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Contains(KeyValuePair<TKey, IList<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, IList<TValue>>>.CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Remove(KeyValuePair<TKey, IList<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Remove(item);
		}

		IEnumerator<KeyValuePair<TKey, IList<TValue>>> IEnumerable<KeyValuePair<TKey, IList<TValue>>>.GetEnumerator()
		{
			return innerValues.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return innerValues.GetEnumerator();
		}
	}
}
