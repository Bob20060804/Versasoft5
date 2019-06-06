using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_ScrollViewerExtensions
	{
		public static IList<FrameworkElement> FUN_lstInhaltErmitteln(this ScrollViewer i_fdcScrollViewer, Func<FrameworkElement, bool> i_delBedingung = null)
		{
			IList<FrameworkElement> list = new List<FrameworkElement>();
			FrameworkElement frameworkElement = i_fdcScrollViewer.FUN_objBenanntesKindElementSuchen("PART_ScrollContentPresenter").FirstOrDefault();
			if (frameworkElement != null)
			{
				StackPanel stackPanel = frameworkElement.FUN_lstKindElementeSuchen<StackPanel>().FirstOrDefault();
				if (stackPanel != null)
				{
					foreach (FrameworkElement item in stackPanel.Children.OfType<FrameworkElement>())
					{
						if (i_delBedingung == null || i_delBedingung(item))
						{
							list.Add(item);
						}
					}
					return list;
				}
			}
			return list;
		}

		public static bool FUN_blnIstElementInSichtbereich(this ScrollViewer i_fdcScrollViewer, FrameworkElement i_fdcFrameworkElement)
		{
			Rect rect = i_fdcFrameworkElement.TransformToAncestor(i_fdcScrollViewer).TransformBounds(new Rect(new Point(0.0, 0.0), i_fdcFrameworkElement.RenderSize));
			return Rect.Intersect(new Rect(new Point(0.0, 0.0), i_fdcScrollViewer.RenderSize), rect) != Rect.Empty;
		}
	}
}
