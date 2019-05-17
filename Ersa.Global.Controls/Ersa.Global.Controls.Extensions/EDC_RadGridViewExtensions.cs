using Ersa.Global.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_RadGridViewExtensions
	{
		public static bool FUN_blnRechteckigeAuswahl(this RadGridView i_fdcGridView)
		{
			IEnumerable<GridViewColumn> source = (from i_fdcZellInfo in i_fdcGridView.SelectedCells
			select i_fdcZellInfo.Column).Distinct();
			IEnumerable<object> source2 = (from i_fdcZellInfo in i_fdcGridView.SelectedCells
			select i_fdcZellInfo.Item).Distinct();
			return source.Count() * source2.Count() == i_fdcGridView.SelectedCells.Count;
		}

		public static IDictionary<Tuple<int, int>, EDC_GetterSetterPaar> FUN_dicAuswahlTabelleErstellen(this RadGridView i_fdcGridView)
		{
			Dictionary<Tuple<int, int>, EDC_GetterSetterPaar> dictionary = new Dictionary<Tuple<int, int>, EDC_GetterSetterPaar>();
			GridViewColumnCollection columns = i_fdcGridView.Columns;
			DataItemCollection items = i_fdcGridView.Items;
			foreach (GridViewCellInfo selectedCell in i_fdcGridView.SelectedCells)
			{
				GridViewCellInfo fdcZellInfoKopie = selectedCell;
				GridViewCell fdcZelle = FUN_fdcZelleErmitteln(i_fdcGridView, fdcZellInfoKopie);
				Func<object> i_delGetter = () => fdcZelle.Value;
				Action<object> i_delSetter = delegate(object i_objWert)
				{
					SUB_ZellenwertSetzen(i_fdcGridView, fdcZellInfoKopie, i_objWert.ToString());
				};
				if (fdcZelle != null)
				{
					int item = columns.IndexOf(fdcZellInfoKopie.Column);
					int item2 = items.IndexOf(fdcZellInfoKopie.Item);
					dictionary.Add(new Tuple<int, int>(item, item2), new EDC_GetterSetterPaar(i_delGetter, i_delSetter));
				}
			}
			return dictionary;
		}

		private static GridViewCell FUN_fdcZelleErmitteln(RadGridView i_fdcGridView, GridViewCellInfo i_fdcZellInfo)
		{
			return (i_fdcGridView.ItemContainerGenerator.ContainerFromItem(i_fdcZellInfo.Item) as GridViewRow)?.Cells.Cast<GridViewCell>().FirstOrDefault((GridViewCell i_fdcZelle) => object.Equals(i_fdcZelle.Column, i_fdcZellInfo.Column));
		}

		private static void SUB_ZellenwertSetzen(RadGridView i_fdcGridView, GridViewCellInfo i_fdcZellInfo, string i_objWert)
		{
			GridViewBoundColumnBase gridViewBoundColumnBase = i_fdcZellInfo.Column as GridViewBoundColumnBase;
			if ((gridViewBoundColumnBase == null || gridViewBoundColumnBase.CanEdit(i_fdcZellInfo.Item)) && !i_fdcZellInfo.Column.IsReadOnly)
			{
				GridViewCellClipboardEventArgs gridViewCellClipboardEventArgs = new GridViewCellClipboardEventArgs(i_fdcZellInfo, i_objWert)
				{
					RoutedEvent = GridViewDataControl.PastingCellClipboardContentEvent
				};
				i_fdcGridView.RaiseEvent(gridViewCellClipboardEventArgs);
				if (!gridViewCellClipboardEventArgs.Cancel)
				{
					i_fdcZellInfo.Column.OnPastingCellClipboardContent(i_fdcZellInfo.Item, gridViewCellClipboardEventArgs.Value);
				}
			}
		}
	}
}
