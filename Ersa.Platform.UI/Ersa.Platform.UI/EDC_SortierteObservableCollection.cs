using System;
using System.Collections.ObjectModel;

namespace Ersa.Platform.UI
{
	public class EDC_SortierteObservableCollection<T> : ObservableCollection<T> where T : IComparable
	{
		protected override void InsertItem(int i_i32Index, T i_objItem)
		{
			for (int i = 0; i < base.Count; i++)
			{
				switch (Math.Sign(base[i].CompareTo(i_objItem)))
				{
				case 0:
				case 1:
					base.InsertItem(i, i_objItem);
					return;
				}
			}
			base.InsertItem(base.Count, i_objItem);
		}
	}
}
