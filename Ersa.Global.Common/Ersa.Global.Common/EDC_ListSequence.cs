using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Common
{
	public class EDC_ListSequence<T>
	{
		public List<T> PRO_lstItems
		{
			get;
		}

		public EDC_ListSequence()
		{
			PRO_lstItems = new List<T>();
		}

		public EDC_ListSequence(int i_intAnzahlElemente)
		{
			PRO_lstItems = new List<T>(i_intAnzahlElemente);
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (T pRO_lstItem in PRO_lstItems)
			{
				num = 31 * num + pRO_lstItem.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object i_objObjekt)
		{
			EDC_ListSequence<T> eDC_ListSequence = i_objObjekt as EDC_ListSequence<T>;
			if (eDC_ListSequence != null)
			{
				return eDC_ListSequence.PRO_lstItems.SequenceEqual(PRO_lstItems);
			}
			return false;
		}
	}
}
