using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Ersa.Global.Common
{
	public class EDC_SmartObservableCollection<T> : ObservableCollection<T>
	{
		public EDC_SmartObservableCollection()
		{
		}

		public EDC_SmartObservableCollection(IList<T> i_lstCollection)
			: base((IEnumerable<T>)i_lstCollection)
		{
		}

		public void SUB_AddRange(IEnumerable<T> i_enuElemente)
		{
			foreach (T item in i_enuElemente)
			{
				base.Items.Add(item);
			}
			SUB_AenderungenSignalisieren();
		}

		public void SUB_Reset(IEnumerable<T> i_enuElemente)
		{
			base.Items.Clear();
			SUB_AddRange(i_enuElemente);
		}

		public void SUB_RemoveRange(IEnumerable<T> i_enuElemente)
		{
			foreach (T item in i_enuElemente)
			{
				base.Items.Remove(item);
			}
			SUB_AenderungenSignalisieren();
		}

		private void SUB_AenderungenSignalisieren()
		{
			OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
