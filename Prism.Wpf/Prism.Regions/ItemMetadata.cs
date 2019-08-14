using System;
using System.Windows;

namespace Prism.Regions
{
	public class ItemMetadata : DependencyObject
	{
		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ItemMetadata), null);

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ItemMetadata), new PropertyMetadata(DependencyPropertyChanged));

		public object Item
		{
			get;
			private set;
		}

		public string Name
		{
			get
			{
				return (string)GetValue(NameProperty);
			}
			set
			{
				SetValue(NameProperty, value);
			}
		}

		public bool IsActive
		{
			get
			{
				return (bool)GetValue(IsActiveProperty);
			}
			set
			{
				SetValue(IsActiveProperty, value);
			}
		}

		public event EventHandler MetadataChanged;

		public ItemMetadata(object item)
		{
			Item = item;
		}

		public void InvokeMetadataChanged()
		{
			this.MetadataChanged?.Invoke(this, EventArgs.Empty);
		}

		private static void DependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			(dependencyObject as ItemMetadata)?.InvokeMetadataChanged();
		}
	}
}
