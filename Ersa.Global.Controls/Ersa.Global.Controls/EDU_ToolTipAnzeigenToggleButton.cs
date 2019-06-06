using Ersa.Global.Controls.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls
{
	public class EDU_ToolTipAnzeigenToggleButton : ToggleButton
	{
		private readonly IList<Popup> m_lstToolTipPopups;

		private DependencyObject m_objStammElement;

		public EDU_ToolTipAnzeigenToggleButton()
		{
			m_lstToolTipPopups = new List<Popup>();
			base.Loaded += SUB_ToggleButtonGeladen;
			base.Checked += SUB_ToggleButtonAktiviert;
			base.Unchecked += SUB_ToggleButtonDeaktiviert;
		}

		private void SUB_ToggleButtonGeladen(object sender, RoutedEventArgs e)
		{
			m_objStammElement = this.FUN_objStammElementErmitteln();
		}

		private void SUB_ToggleButtonAktiviert(object sender, RoutedEventArgs routedEventArgs)
		{
			SUB_ToolTipsAktivieren(m_objStammElement);
		}

		private void SUB_ToggleButtonDeaktiviert(object sender, RoutedEventArgs routedEventArgs)
		{
			SUB_ToolTipsDeaktivieren();
		}

		private void SUB_ToolTipsAktivieren(DependencyObject i_objAktuellesObjekt)
		{
			if (i_objAktuellesObjekt == null)
			{
				return;
			}
			FrameworkElement frameworkElement = i_objAktuellesObjekt as FrameworkElement;
			if (frameworkElement != null)
			{
				string text = frameworkElement.ToolTip as string;
				if (text != null)
				{
					Popup popup = new Popup
					{
						AllowsTransparency = true,
						StaysOpen = true,
						PlacementTarget = frameworkElement,
						Placement = PlacementMode.Center,
						Child = new EDU_ToolTipPopupContent
						{
							Content = text
						}
					};
					m_lstToolTipPopups.Add(popup);
					popup.IsOpen = true;
				}
			}
			foreach (DependencyObject item in i_objAktuellesObjekt.FUN_lstKindElementeErmitteln())
			{
				SUB_ToolTipsAktivieren(item);
			}
		}

		private void SUB_ToolTipsDeaktivieren()
		{
			foreach (Popup lstToolTipPopup in m_lstToolTipPopups)
			{
				lstToolTipPopup.IsOpen = false;
			}
			m_lstToolTipPopups.Clear();
		}
	}
}
