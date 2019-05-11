using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Common
{
	public class EDC_Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IEnumerable<TElement>, IEnumerable
	{
		private readonly IEnumerable<TElement> m_enuElemente;

		public TKey Key
		{
			get;
			private set;
		}

		public EDC_Grouping(TKey i_objKey, IEnumerable<TElement> i_enuElemente)
		{
			if (i_enuElemente == null)
			{
				throw new ArgumentNullException("i_enuElemente");
			}
			Key = i_objKey;
			m_enuElemente = i_enuElemente;
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			return m_enuElemente.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
