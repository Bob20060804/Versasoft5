using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_GroupExpandedStatusBehavior : Behavior<Expander>
	{
		public static readonly DependencyProperty PRO_objGroupNameProperty = DependencyProperty.Register("PRO_objGroupName", typeof(object), typeof(EDC_GroupExpandedStatusBehavior), new PropertyMetadata((object)null));

		private const string mC_strGroupNameIfNull = "C_strGroupNameIfNull";

		private static readonly DependencyProperty PRO_dicExpandedStateStoreProperty = DependencyProperty.RegisterAttached("PRO_dicExpandedStateStore", typeof(IDictionary<object, bool>), typeof(EDC_GroupExpandedStatusBehavior), new PropertyMetadata((object)null));

		public object PRO_objGroupName
		{
			get
			{
				return GetValue(PRO_objGroupNameProperty);
			}
			set
			{
				SetValue(PRO_objGroupNameProperty, value);
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			bool? flag = FUN_blnZustandErmitteln();
			if (flag.HasValue)
			{
				base.AssociatedObject.IsExpanded = flag.Value;
			}
			base.AssociatedObject.Expanded += SUB_OnAusgeklappt;
			base.AssociatedObject.Collapsed += SUB_OnEingeklappt;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.Expanded -= SUB_OnAusgeklappt;
			base.AssociatedObject.Collapsed -= SUB_OnEingeklappt;
			base.OnDetaching();
		}

		private ItemsControl FUN_fdcItemsControlErmitteln()
		{
			DependencyObject dependencyObject = base.AssociatedObject;
			while (dependencyObject != null && !(dependencyObject is ItemsControl))
			{
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			return dependencyObject as ItemsControl;
		}

		private bool? FUN_blnZustandErmitteln()
		{
			IDictionary<object, bool> dictionary = FUN_dicZustaendeErmitteln();
			object key = PRO_objGroupName ?? "C_strGroupNameIfNull";
			if (!dictionary.ContainsKey(key))
			{
				return null;
			}
			return dictionary[key];
		}

		private IDictionary<object, bool> FUN_dicZustaendeErmitteln()
		{
			ItemsControl itemsControl = FUN_fdcItemsControlErmitteln();
			if (itemsControl == null)
			{
				return new Dictionary<object, bool>();
			}
			IDictionary<object, bool> dictionary = itemsControl.GetValue(PRO_dicExpandedStateStoreProperty) as IDictionary<object, bool>;
			if (dictionary == null)
			{
				dictionary = new Dictionary<object, bool>();
				itemsControl.SetValue(PRO_dicExpandedStateStoreProperty, dictionary);
			}
			return dictionary;
		}

		private void SUB_OnEingeklappt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			SUB_ZustandSetzen(i_blnZustand: false);
		}

		private void SUB_OnAusgeklappt(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			SUB_ZustandSetzen(i_blnZustand: true);
		}

		private void SUB_ZustandSetzen(bool i_blnZustand)
		{
			IDictionary<object, bool> dictionary = FUN_dicZustaendeErmitteln();
			object key = PRO_objGroupName ?? "C_strGroupNameIfNull";
			dictionary[key] = i_blnZustand;
		}
	}
}
