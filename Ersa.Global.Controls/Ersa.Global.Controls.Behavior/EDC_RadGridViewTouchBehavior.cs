using Ersa.Global.Controls.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_RadGridViewTouchBehavior : Behavior<RadGridView>
	{
		private Point m_fdcStartAuswahl;

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.ManipulationStarted += SUB_ManipulationStarted;
			base.AssociatedObject.ManipulationDelta += SUB_ManipulationDelta;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.ManipulationStarted -= SUB_ManipulationStarted;
			base.AssociatedObject.ManipulationDelta -= SUB_ManipulationDelta;
			base.OnDetaching();
		}

		private void SUB_ManipulationStarted(object i_objSender, ManipulationStartedEventArgs i_fdcArgs)
		{
			FrameworkElement frameworkElement = i_fdcArgs.Source as FrameworkElement;
			if (frameworkElement != null && Manipulation.IsManipulationActive(frameworkElement))
			{
				Manipulation.SetManipulationMode(frameworkElement, ManipulationModes.Translate);
			}
			m_fdcStartAuswahl = i_fdcArgs.ManipulationOrigin;
		}

		private void SUB_ManipulationDelta(object i_objSender, ManipulationDeltaEventArgs i_fdcArgs)
		{
			if (!i_fdcArgs.IsInertial)
			{
				UIElement uIElement = i_fdcArgs.ManipulationContainer as UIElement;
				if (uIElement != null)
				{
					double x = i_fdcArgs.CumulativeManipulation.Translation.X;
					double y = i_fdcArgs.CumulativeManipulation.Translation.Y;
					Rect rect = FUN_fdcRectErmitteln(m_fdcStartAuswahl, x, y);
					foreach (GridViewCell item2 in (from i_fdcZelle in base.AssociatedObject.FUN_lstKindElementeSuchen<GridViewCell>()
					where i_fdcZelle.IsSelected
					select i_fdcZelle).ToList())
					{
						item2.IsSelected = false;
					}
					base.AssociatedObject.SelectedCells.Clear();
					foreach (GridViewCell item3 in base.AssociatedObject.FUN_lstKindElementeSuchen<GridViewCell>().ToList())
					{
						Point location = item3.TransformToAncestor(uIElement).Transform(new Point(0.0, 0.0));
						Rect rect2 = new Rect(location, new Size(item3.ActualWidth, item3.ActualHeight));
						if (Rect.Intersect(rect, rect2) != Rect.Empty)
						{
							item3.IsSelected = true;
							GridViewCellInfo item = new GridViewCellInfo(item3);
							if (!base.AssociatedObject.SelectedCells.Contains(item))
							{
								base.AssociatedObject.SelectedCells.Add(item);
							}
						}
					}
				}
			}
		}

		private Rect FUN_fdcRectErmitteln(Point i_fdcStartPunkt, double i_dblBreite, double i_dblHoehe)
		{
			double num = i_fdcStartPunkt.X;
			double num2 = i_fdcStartPunkt.Y;
			if (i_dblBreite < 0.0)
			{
				num += i_dblBreite;
				i_dblBreite *= -1.0;
			}
			if (i_dblHoehe < 0.0)
			{
				num2 += i_dblHoehe;
				i_dblHoehe *= -1.0;
			}
			return new Rect(num, num2, i_dblBreite, i_dblHoehe);
		}
	}
}
