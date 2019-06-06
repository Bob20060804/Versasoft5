using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls
{
	public class EDU_IconComboBox : ComboBox
	{
		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_strTextProperty;

		public static readonly DependencyProperty PRO_objPopupRichtungProperty;

		public string PRO_strIconUri
		{
			get
			{
				return (string)GetValue(PRO_strIconUriProperty);
			}
			set
			{
				SetValue(PRO_strIconUriProperty, value);
			}
		}

		public string PRO_strText
		{
			get
			{
				return (string)GetValue(PRO_strTextProperty);
			}
			set
			{
				SetValue(PRO_strTextProperty, value);
			}
		}

		public PlacementMode PRO_objPopupRichtung
		{
			get
			{
				return (PlacementMode)GetValue(PRO_objPopupRichtungProperty);
			}
			set
			{
				SetValue(PRO_objPopupRichtungProperty, value);
			}
		}

		static EDU_IconComboBox()
		{
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_IconComboBox));
			PRO_strTextProperty = DependencyProperty.Register("PRO_strText", typeof(string), typeof(EDU_IconComboBox));
			PRO_objPopupRichtungProperty = DependencyProperty.Register("PRO_objPopupRichtung", typeof(PlacementMode), typeof(EDU_IconComboBox), new PropertyMetadata(PlacementMode.Bottom));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_IconComboBox), new FrameworkPropertyMetadata(typeof(EDU_IconComboBox)));
		}

		public EDU_IconComboBox()
		{
			base.Loaded += SUB_ComboBoxGeladen;
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.NewItems != null)
			{
				foreach (object newItem in e.NewItems)
				{
					ButtonBase buttonBase = newItem as ButtonBase;
					if (buttonBase != null)
					{
						SUB_ButtonItemAnpassen(buttonBase);
					}
				}
			}
		}

		private void SUB_ComboBoxGeladen(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			foreach (object item in (IEnumerable)base.Items)
			{
				ButtonBase buttonBase = item as ButtonBase;
				if (buttonBase != null)
				{
					SUB_ButtonItemAnpassen(buttonBase);
				}
			}
		}

		private void SUB_ButtonItemAnpassen(ButtonBase i_btnButton)
		{
			i_btnButton.Click += delegate
			{
				base.IsDropDownOpen = false;
			};
		}
	}
}
