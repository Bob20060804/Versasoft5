using System;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_TextBlockAutoShrink : TextBlock
	{
		public static readonly DependencyProperty PRO_dblDefaultMarginProperty;

		public double PRO_dblDefaultMargin
		{
			get
			{
				return (double)GetValue(PRO_dblDefaultMarginProperty);
			}
			set
			{
				SetValue(PRO_dblDefaultMarginProperty, value);
			}
		}

		static EDU_TextBlockAutoShrink()
		{
			PRO_dblDefaultMarginProperty = DependencyProperty.Register("PRO_dblDefaultMargin", typeof(double), typeof(EDU_TextBlockAutoShrink), new PropertyMetadata(0.0));
			TextBlock.TextProperty.OverrideMetadata(typeof(EDU_TextBlockAutoShrink), new FrameworkPropertyMetadata(SUB_OnTextPropertyChanged));
		}

		public EDU_TextBlockAutoShrink()
		{
			base.DataContextChanged += SUB_DataContextChanged;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo i_fdcSizeInfo)
		{
			SUB_AnGroesseAnpassen();
			base.OnRenderSizeChanged(i_fdcSizeInfo);
		}

		private static void SUB_OnTextPropertyChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_objSender as EDU_TextBlockAutoShrink)?.SUB_AnGroesseAnpassen();
		}

		private void SUB_DataContextChanged(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			SUB_AnGroesseAnpassen();
		}

		private void SUB_AnGroesseAnpassen()
		{
			FrameworkElement frameworkElement = base.Parent as FrameworkElement;
			if (frameworkElement != null)
			{
				double val = base.FontSize;
				double val2 = base.FontSize;
				double num = double.IsInfinity(base.MaxWidth) ? frameworkElement.ActualWidth : base.MaxWidth;
				double num2 = double.IsInfinity(base.MaxHeight) ? frameworkElement.ActualHeight : base.MaxHeight;
				if (base.ActualWidth > num)
				{
					val = base.FontSize * (num / (base.ActualWidth + PRO_dblDefaultMargin));
				}
				if (base.ActualHeight > num2)
				{
					double num3 = num2 / base.ActualHeight;
					num3 = ((1.0 - num3 > 0.04) ? Math.Sqrt(num3) : num3);
					val2 = base.FontSize * num3;
				}
				base.FontSize = Math.Min(val, val2);
			}
		}
	}
}
