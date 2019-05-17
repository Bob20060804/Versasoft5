using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_RadGridViewHelfer
	{
		public static readonly DependencyProperty ms_fdcSelektierteZeilenProperty = DependencyProperty.RegisterAttached("PRO_lstSelektierteZeilen", typeof(ObservableCollection<object>), typeof(EDC_RadGridViewHelfer), new PropertyMetadata(SUB_SelektierteZeilenCollectionGeaendert));

		public static ObservableCollection<object> GetPRO_lstSelektierteZeilen(DependencyObject i_fdcDependencyObject)
		{
			return (ObservableCollection<object>)i_fdcDependencyObject.GetValue(ms_fdcSelektierteZeilenProperty);
		}

		public static void SetPRO_lstSelektierteZeilen(DependencyObject i_fdcDependencyObject, ObservableCollection<object> i_lstZeilen)
		{
			i_fdcDependencyObject.SetValue(ms_fdcSelektierteZeilenProperty, i_lstZeilen);
		}

		private static void SUB_SelektierteZeilenCollectionGeaendert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			RadGridView fdcGridView = i_fdcDependencyObject as RadGridView;
			if (fdcGridView != null)
			{
				fdcGridView.SelectedCellsChanged += delegate
				{
					ObservableCollection<object> pRO_lstSelektierteZeilen = GetPRO_lstSelektierteZeilen(fdcGridView);
					pRO_lstSelektierteZeilen.Clear();
					foreach (object item in (from i_fdcZellInfo in fdcGridView.SelectedCells
					select i_fdcZellInfo.Item).Distinct())
					{
						pRO_lstSelektierteZeilen.Add(item);
					}
				};
			}
		}
	}
}
