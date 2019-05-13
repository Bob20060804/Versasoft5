using System.Windows;

namespace Ersa.Platform.UI.Common
{
	public class EDC_StringFormatProxy : FrameworkElement
	{
		public static readonly DependencyProperty PRO_fdcStringFormatProperty = DependencyProperty.Register("PRO_strStringFormat", typeof(string), typeof(EDC_StringFormatProxy), new PropertyMetadata("{0}", SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcValueProperty1 = DependencyProperty.Register("PRO_objWert1", typeof(object), typeof(EDC_StringFormatProxy), new PropertyMetadata(null, SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcValueProperty2 = DependencyProperty.Register("PRO_objWert2", typeof(object), typeof(EDC_StringFormatProxy), new PropertyMetadata(null, SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcValueProperty3 = DependencyProperty.Register("PRO_objWert3", typeof(object), typeof(EDC_StringFormatProxy), new PropertyMetadata(null, SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcValueProperty4 = DependencyProperty.Register("PRO_objWert4", typeof(object), typeof(EDC_StringFormatProxy), new PropertyMetadata(null, SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcValueProperty5 = DependencyProperty.Register("PRO_objWert5", typeof(object), typeof(EDC_StringFormatProxy), new PropertyMetadata(null, SUB_OnDataChanged));

		public static readonly DependencyProperty PRO_fdcResultProperty = DependencyProperty.Register("PRO_strResult", typeof(string), typeof(EDC_StringFormatProxy), new PropertyMetadata(null));

		public string PRO_strStringFormat
		{
			get
			{
				return (string)GetValue(PRO_fdcStringFormatProperty);
			}
			set
			{
				SetValue(PRO_fdcStringFormatProperty, value);
			}
		}

		public object PRO_objWert1
		{
			get
			{
				return GetValue(PRO_fdcValueProperty1);
			}
			set
			{
				SetValue(PRO_fdcValueProperty1, value);
			}
		}

		public object PRO_objWert2
		{
			get
			{
				return GetValue(PRO_fdcValueProperty2);
			}
			set
			{
				SetValue(PRO_fdcValueProperty2, value);
			}
		}

		public object PRO_objWert3
		{
			get
			{
				return GetValue(PRO_fdcValueProperty3);
			}
			set
			{
				SetValue(PRO_fdcValueProperty3, value);
			}
		}

		public object PRO_objWert4
		{
			get
			{
				return GetValue(PRO_fdcValueProperty4);
			}
			set
			{
				SetValue(PRO_fdcValueProperty4, value);
			}
		}

		public object PRO_objWert5
		{
			get
			{
				return GetValue(PRO_fdcValueProperty5);
			}
			set
			{
				SetValue(PRO_fdcValueProperty5, value);
			}
		}

		public string PRO_strResult
		{
			get
			{
				return (string)GetValue(PRO_fdcResultProperty);
			}
			set
			{
				SetValue(PRO_fdcResultProperty, value);
			}
		}

		private static void SUB_OnDataChanged(DependencyObject i_fdcSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDC_StringFormatProxy eDC_StringFormatProxy = i_fdcSender as EDC_StringFormatProxy;
			if (eDC_StringFormatProxy != null)
			{
				eDC_StringFormatProxy.PRO_strResult = string.Format(eDC_StringFormatProxy.PRO_strStringFormat, eDC_StringFormatProxy.PRO_objWert1, eDC_StringFormatProxy.PRO_objWert2, eDC_StringFormatProxy.PRO_objWert3, eDC_StringFormatProxy.PRO_objWert4, eDC_StringFormatProxy.PRO_objWert5);
			}
		}
	}
}
