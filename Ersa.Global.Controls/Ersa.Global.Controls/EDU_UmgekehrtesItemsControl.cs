using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_UmgekehrtesItemsControl : ItemsControl
	{
		public static readonly DependencyProperty PRO_lstUmgekehrteItemsSourceProperty = DependencyProperty.Register("PRO_lstUmgekehrteItemsSource", typeof(IEnumerable), typeof(EDU_UmgekehrtesItemsControl), new UIPropertyMetadata(SUB_UmgekehrteItemsSourceGeaendert));

		public IEnumerable PRO_lstUmgekehrteItemsSource
		{
			get
			{
				return (IEnumerable)GetValue(PRO_lstUmgekehrteItemsSourceProperty);
			}
			set
			{
				SetValue(PRO_lstUmgekehrteItemsSourceProperty, value);
			}
		}

		private static void SUB_UmgekehrteItemsSourceGeaendert(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			(dependencyObject as EDU_UmgekehrtesItemsControl)?.SUB_ItemsSourceSetzen();
		}

		private void SUB_ItemsSourceSetzen()
		{
			INotifyCollectionChanged notifyCollectionChanged = PRO_lstUmgekehrteItemsSource as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged += delegate
				{
					SUB_ItemsSourceUmkehren();
				};
			}
			SUB_ItemsSourceUmkehren();
		}

		private void SUB_ItemsSourceUmkehren()
		{
			IList<object> list = new List<object>();
			if (PRO_lstUmgekehrteItemsSource != null)
			{
				foreach (object item in PRO_lstUmgekehrteItemsSource)
				{
					list.Insert(0, item);
				}
			}
			base.ItemsSource = list;
		}
	}
}
