using Ersa.Global.Controls.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls.NavigationsTabControl
{
	public class EDU_NavigationsTabControl : EDU_MultiContentTabControl
	{
		public static readonly DependencyProperty PRO_cmdTabItemGeaendertCommandProperty = DependencyProperty.Register("PRO_cmdTabItemGeaendertCommand", typeof(ICommand), typeof(EDU_NavigationsTabControl));

		private EDU_PositioniertesTabItem m_edcLetztesEingefuegtesTabItem;

		public ICommand PRO_cmdTabItemGeaendertCommand
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdTabItemGeaendertCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdTabItemGeaendertCommandProperty, value);
			}
		}

		public EDU_NavigationsTabControl()
		{
			base.Loaded += delegate
			{
				object selectedItem = base.SelectedItem;
				if (selectedItem != null)
				{
					EDC_NavigationsElementAenderungsDaten i_objCommandParameter = new EDC_NavigationsElementAenderungsDaten
					{
						PRO_objElement = selectedItem
					};
					PRO_cmdTabItemGeaendertCommand.SUB_Execute(i_objCommandParameter, this);
				}
			};
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			EDU_PositioniertesTabItem eDU_PositioniertesTabItem = new EDU_PositioniertesTabItem
			{
				PRO_blnIsLast = true
			};
			if (m_edcLetztesEingefuegtesTabItem == null)
			{
				eDU_PositioniertesTabItem.PRO_blnIsFirst = true;
			}
			else
			{
				m_edcLetztesEingefuegtesTabItem.PRO_blnIsLast = false;
			}
			m_edcLetztesEingefuegtesTabItem = eDU_PositioniertesTabItem;
			return eDU_PositioniertesTabItem;
		}

		protected override void OnPreviewKeyDown(KeyEventArgs i_fdcArgs)
		{
			if (i_fdcArgs.OriginalSource.GetType() == typeof(TabItem) && base.Items.Count > 1)
			{
				object i_objNeuesElement = null;
				switch (i_fdcArgs.Key)
				{
				case Key.Left:
					i_objNeuesElement = ((base.SelectedIndex == 0) ? base.Items[base.Items.Count - 1] : base.Items[base.SelectedIndex - 1]);
					break;
				case Key.Right:
					i_objNeuesElement = ((base.SelectedIndex == base.Items.Count - 1) ? base.Items[0] : base.Items[base.SelectedIndex + 1]);
					break;
				}
				i_fdcArgs.Handled = FUN_blnAuswahlAenderungBehandeln(base.SelectedItem, i_objNeuesElement);
			}
			if (!i_fdcArgs.Handled)
			{
				base.OnPreviewKeyDown(i_fdcArgs);
			}
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs i_fdcArgs)
		{
			DependencyObject dependencyObject = i_fdcArgs.OriginalSource as DependencyObject;
			object obj = null;
			if (dependencyObject != null)
			{
				TabItem tabItem = dependencyObject.FUN_objElternElementErmitteln<TabItem>();
				if (tabItem != null)
				{
					obj = tabItem.Content;
				}
				i_fdcArgs.Handled = FUN_blnAuswahlAenderungBehandeln(base.SelectedItem, obj);
			}
			if (!i_fdcArgs.Handled)
			{
				base.OnPreviewMouseDown(i_fdcArgs);
				if (obj != null)
				{
					base.SelectedItem = obj;
				}
			}
		}

		private bool FUN_blnAuswahlAenderungBehandeln(object i_objAltesElement, object i_objNeuesElement)
		{
			bool result = false;
			if (PRO_cmdTabItemGeaendertCommand != null && i_objAltesElement != null && i_objNeuesElement != null && !object.Equals(i_objAltesElement, i_objNeuesElement) && base.Items.Contains(i_objAltesElement) && base.Items.Contains(i_objNeuesElement))
			{
				EDC_NavigationsElementAenderungsDaten eDC_NavigationsElementAenderungsDaten = new EDC_NavigationsElementAenderungsDaten
				{
					PRO_objElement = i_objNeuesElement
				};
				PRO_cmdTabItemGeaendertCommand.SUB_Execute(eDC_NavigationsElementAenderungsDaten, this);
				result = eDC_NavigationsElementAenderungsDaten.PRO_blnIstAenderungAbgebrochen;
			}
			return result;
		}
	}
}
