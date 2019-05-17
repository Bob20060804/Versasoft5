using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls.NavigationsTabControl
{
	public class EDU_MultiContentTabControl : TabControl
	{
		private Panel m_fdcItemsHolder;

		public EDU_MultiContentTabControl()
		{
			base.ItemContainerGenerator.StatusChanged += SUB_OnItemContainerGeneratorStatusChanged;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			m_fdcItemsHolder = (GetTemplateChild("PART_SelectedContentHost") as Panel);
			SUB_AngezeigteElementeAktualisieren();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs i_fdcArgs)
		{
			base.OnItemsChanged(i_fdcArgs);
			if (m_fdcItemsHolder != null)
			{
				switch (i_fdcArgs.Action)
				{
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
					SUB_ItemsZuruecksetzen();
					break;
				case NotifyCollectionChangedAction.Remove:
					SUB_ItemsEntfernen(i_fdcArgs.OldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException("Replace not implemented");
				}
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs i_fdcArgs)
		{
			base.OnSelectionChanged(i_fdcArgs);
			SUB_AngezeigteElementeAktualisieren();
		}

		private void SUB_ItemsZuruecksetzen()
		{
			m_fdcItemsHolder.Children.Clear();
			if (base.Items.Count > 0)
			{
				base.SelectedItem = base.Items[0];
				SUB_AngezeigteElementeAktualisieren();
			}
		}

		private void SUB_ItemsEntfernen(IList i_lstElemente)
		{
			if (i_lstElemente != null)
			{
				foreach (object item in i_lstElemente)
				{
					if (item != null)
					{
						ContentPresenter contentPresenter = FUN_fdcChildContentPresenterErmitteln(item);
						if (contentPresenter != null)
						{
							m_fdcItemsHolder.Children.Remove(contentPresenter);
						}
					}
				}
			}
			SUB_AngezeigteElementeAktualisieren();
		}

		private void SUB_AngezeigteElementeAktualisieren()
		{
			if (m_fdcItemsHolder != null)
			{
				TabItem tabItem = FUN_fdcSelectedTabItem();
				if (tabItem != null)
				{
					SUB_ContentPresenterErzeugenWennNoetig(tabItem);
				}
				foreach (ContentPresenter child in m_fdcItemsHolder.Children)
				{
					TabItem tabItem2 = child.Tag as TabItem;
					child.Visibility = ((tabItem2 == null || !tabItem2.IsSelected) ? Visibility.Collapsed : Visibility.Visible);
				}
			}
		}

		private void SUB_ContentPresenterErzeugenWennNoetig(TabItem i_fdcTabItem)
		{
			ContentPresenter contentPresenter = FUN_fdcChildContentPresenterErmitteln(i_fdcTabItem);
			if (contentPresenter == null)
			{
				contentPresenter = new ContentPresenter
				{
					Content = i_fdcTabItem.Content,
					ContentTemplate = base.SelectedContentTemplate,
					ContentTemplateSelector = base.SelectedContentTemplateSelector,
					ContentStringFormat = base.SelectedContentStringFormat,
					Visibility = Visibility.Collapsed,
					Tag = i_fdcTabItem
				};
				m_fdcItemsHolder.Children.Add(contentPresenter);
			}
		}

		private ContentPresenter FUN_fdcChildContentPresenterErmitteln(object i_objData)
		{
			TabItem tabItem = i_objData as TabItem;
			if (tabItem != null)
			{
				i_objData = tabItem.Content;
			}
			if (i_objData == null || m_fdcItemsHolder == null)
			{
				return null;
			}
			return m_fdcItemsHolder.Children.OfType<ContentPresenter>().FirstOrDefault((ContentPresenter i_fdcContentPresenter) => i_fdcContentPresenter.Content == i_objData);
		}

		private void SUB_OnItemContainerGeneratorStatusChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.ItemContainerGenerator.StatusChanged -= SUB_OnItemContainerGeneratorStatusChanged;
				SUB_AngezeigteElementeAktualisieren();
			}
		}

		private TabItem FUN_fdcSelectedTabItem()
		{
			if (base.SelectedItem == null)
			{
				return null;
			}
			return (base.SelectedItem as TabItem) ?? (base.ItemContainerGenerator.ContainerFromIndex(base.SelectedIndex) as TabItem);
		}
	}
}
