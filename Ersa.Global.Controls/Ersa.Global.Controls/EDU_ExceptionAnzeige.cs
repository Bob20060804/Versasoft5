using System;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_ExceptionAnzeige : Control
	{
		public static readonly DependencyProperty PRO_fdcExceptionProperty;

		public Exception PRO_fdcException
		{
			get
			{
				return (Exception)GetValue(PRO_fdcExceptionProperty);
			}
			set
			{
				SetValue(PRO_fdcExceptionProperty, value);
			}
		}

		static EDU_ExceptionAnzeige()
		{
			PRO_fdcExceptionProperty = DependencyProperty.Register("PRO_fdcException", typeof(Exception), typeof(EDU_ExceptionAnzeige));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ExceptionAnzeige), new FrameworkPropertyMetadata(typeof(EDU_ExceptionAnzeige)));
		}
	}
}
