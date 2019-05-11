using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Common.Extensions
{
	public static class EDC_CollectionExtensions
	{
		public static void AddRange<T>(this ICollection<T> i_objCollection, IEnumerable<T> i_objListToAdd)
		{
			if (i_objCollection == null)
			{
				throw new NullReferenceException("Collection ist nicht initialisiert!");
			}
			foreach (T item in i_objListToAdd)
			{
				i_objCollection.Add(item);
			}
		}

		public static void AddRange<T>(this ICollection<T> i_objCollection, params T[] ia_objListToAdd)
		{
			i_objCollection.AddRange(ia_objListToAdd.ToList());
		}

		public static void RemoveRange<T>(this ICollection<T> i_objCollection, IEnumerable<T> i_objListToAdd)
		{
			if (i_objCollection == null)
			{
				throw new NullReferenceException("Collection ist nicht initialisiert!");
			}
			foreach (T item in i_objListToAdd)
			{
				i_objCollection.Remove(item);
			}
		}

		public static void MoveItem<T>(this IList<T> i_lstListe, int i_i32AlterIndex, int i_i32NeuerIndex)
		{
			T item = i_lstListe[i_i32AlterIndex];
			i_lstListe.Remove(item);
			i_lstListe.Insert(i_i32NeuerIndex, item);
		}

		public static TSource FUN_objElementMitMaxWertErmitteln<TSource, TKey>(this IEnumerable<TSource> i_enuQuelle, Func<TSource, TKey> i_delSelector)
		{
			return i_enuQuelle.FUN_objElementMitMaxWertErmitteln(i_delSelector, Comparer<TKey>.Default);
		}

		public static TSource FUN_objElementMitMaxWertErmitteln<TSource, TKey>(this IEnumerable<TSource> i_enuQuelle, Func<TSource, TKey> i_delSelector, IComparer<TKey> i_fdcComparer)
		{
			if (i_enuQuelle == null)
			{
				throw new ArgumentNullException("i_enuQuelle");
			}
			if (i_delSelector == null)
			{
				throw new ArgumentNullException("i_delSelector");
			}
			if (i_fdcComparer == null)
			{
				throw new ArgumentNullException("i_fdcComparer");
			}
			using (IEnumerator<TSource> enumerator = i_enuQuelle.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					return default(TSource);
				}
				TSource val = enumerator.Current;
				TKey y = i_delSelector(val);
				while (enumerator.MoveNext())
				{
					TSource current = enumerator.Current;
					TKey val2 = i_delSelector(current);
					if (i_fdcComparer.Compare(val2, y) > 0)
					{
						val = current;
						y = val2;
					}
				}
				return val;
			}
		}

		public static bool FUN_blnIdentisch<T>(this ICollection<T> i_enuListe1, ICollection<T> i_enuListe2)
		{
			if (i_enuListe1.All(i_enuListe2.Contains))
			{
				return i_enuListe2.All(i_enuListe1.Contains);
			}
			return false;
		}

		public static IEnumerable<T> FUN_enuUnion<T>(this T i_objFirst, params T[] ia_objSecond)
		{
			return new T[1]
			{
				i_objFirst
			}.Union(ia_objSecond);
		}

		public static IEnumerable<T> FUN_enuUnion<T>(this T i_objFirst, IEnumerable<T> i_enuSecond)
		{
			return new T[1]
			{
				i_objFirst
			}.Union(i_enuSecond);
		}

		public static IEnumerable<T> FUN_enuUnion<T>(this IEnumerable<T> i_enuFirst, params T[] ia_objItems)
		{
			return i_enuFirst.Union(ia_objItems);
		}
	}
}
