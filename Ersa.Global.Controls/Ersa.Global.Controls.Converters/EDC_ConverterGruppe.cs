using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Converters
{
	[ContentProperty("PRO_colConverters")]
	public class EDC_ConverterGruppe : IValueConverter
	{
		private readonly ObservableCollection<IValueConverter> m_colConverters = new ObservableCollection<IValueConverter>();

		private readonly Dictionary<IValueConverter, ValueConversionAttribute> m_dicCachedAttributes = new Dictionary<IValueConverter, ValueConversionAttribute>();

		public ObservableCollection<IValueConverter> PRO_colConverters => m_colConverters;

		public EDC_ConverterGruppe()
		{
			m_colConverters.CollectionChanged += SUB_OnConvertersCollectionChanged;
		}

		public object Convert(object i_objValue, Type fdcTargetType, object objParameter, CultureInfo fdcCulture)
		{
			object obj = i_objValue;
			for (int i = 0; i < PRO_colConverters.Count; i++)
			{
				IValueConverter valueConverter = PRO_colConverters[i];
				Type targetType = FUN_fdcTargetTypErmitteln(i, fdcTargetType, i_blnConvert: true);
				obj = valueConverter.Convert(obj, targetType, objParameter, fdcCulture);
				if (obj == Binding.DoNothing)
				{
					break;
				}
			}
			return obj;
		}

		public object ConvertBack(object i_objValue, Type fdcTargetType, object objParameter, CultureInfo fdcCulture)
		{
			object obj = i_objValue;
			for (int num = PRO_colConverters.Count - 1; num > -1; num--)
			{
				IValueConverter valueConverter = PRO_colConverters[num];
				Type targetType = FUN_fdcTargetTypErmitteln(num, fdcTargetType, i_blnConvert: false);
				obj = valueConverter.ConvertBack(obj, targetType, objParameter, fdcCulture);
				if (obj == Binding.DoNothing)
				{
					break;
				}
			}
			return obj;
		}

		private Type FUN_fdcTargetTypErmitteln(int i_i32converterIndex, Type i_fdcFinalTargetType, bool i_blnConvert)
		{
			IValueConverter valueConverter = null;
			if (i_blnConvert && i_i32converterIndex < PRO_colConverters.Count - 1)
			{
				valueConverter = PRO_colConverters[i_i32converterIndex + 1];
				if (valueConverter == null)
				{
					throw new InvalidOperationException("The Converters collection of the ValueConverterGroup contains a null reference at index: " + (i_i32converterIndex + 1));
				}
			}
			else if (i_i32converterIndex > 0)
			{
				valueConverter = PRO_colConverters[i_i32converterIndex - 1];
				if (valueConverter == null)
				{
					throw new InvalidOperationException("The Converters collection of the ValueConverterGroup contains a null reference at index: " + (i_i32converterIndex - 1));
				}
			}
			if (valueConverter == null)
			{
				return i_fdcFinalTargetType;
			}
			ValueConversionAttribute valueConversionAttribute = m_dicCachedAttributes[valueConverter];
			if (!i_blnConvert)
			{
				return valueConversionAttribute.TargetType;
			}
			return valueConversionAttribute.SourceType;
		}

		private void SUB_OnConvertersCollectionChanged(object i_objSender, NotifyCollectionChangedEventArgs i_fdcArgs)
		{
			IList list = null;
			switch (i_fdcArgs.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Replace:
				list = i_fdcArgs.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (IValueConverter oldItem in i_fdcArgs.OldItems)
				{
					m_dicCachedAttributes.Remove(oldItem);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				m_dicCachedAttributes.Clear();
				list = m_colConverters;
				break;
			}
			if (list != null && list.Count > 0)
			{
				foreach (IValueConverter item in list)
				{
					object[] customAttributes = item.GetType().GetCustomAttributes(typeof(ValueConversionAttribute), inherit: false);
					if (customAttributes.Length != 1)
					{
						throw new InvalidOperationException("All value converters added to a ValueConverterGroup must be decorated with the ValueConversionAttribute attribute exactly once.");
					}
					m_dicCachedAttributes.Add(item, customAttributes[0] as ValueConversionAttribute);
				}
			}
		}
	}
}
