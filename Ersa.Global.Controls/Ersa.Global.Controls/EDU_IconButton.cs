using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_IconButton : Button
	{
		public const int mC_i32SperrzeitKurz = 200;

		public const int mC_i32SperrzeitMittel = 500;

		public const int mC_i32SperrzeitLang = 700;

		public const int mC_i32SperrzeitSehrLang = 1000;

		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_fdcEckRadienProperty;

		public static readonly DependencyProperty PRO_i32SperrzeitProperty;

		public int PRO_i32Sperrzeit
		{
			get
			{
				return (int)GetValue(PRO_i32SperrzeitProperty);
			}
			set
			{
				SetValue(PRO_i32SperrzeitProperty, value);
			}
		}

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

		public CornerRadius PRO_fdcEckRadien
		{
			get
			{
				return (CornerRadius)GetValue(PRO_fdcEckRadienProperty);
			}
			set
			{
				SetValue(PRO_fdcEckRadienProperty, value);
			}
		}

		static EDU_IconButton()
		{
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_IconButton));
			PRO_fdcEckRadienProperty = DependencyProperty.Register("PRO_fdcEckRadien", typeof(CornerRadius), typeof(EDU_IconButton));
			PRO_i32SperrzeitProperty = DependencyProperty.Register("PRO_i32Sperrzeit", typeof(int), typeof(EDU_IconButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_IconButton), new FrameworkPropertyMetadata(typeof(EDU_IconButton)));
		}

		protected override void OnClick()
		{
			base.OnClick();
			if (PRO_i32Sperrzeit > 0)
			{
				base.IsHitTestVisible = false;
				bool blnHatFocus = base.IsFocused;
				Binding fdcBinding = BindingOperations.GetBinding(this, UIElement.IsEnabledProperty);
				if (fdcBinding != null)
				{
					BindingOperations.ClearBinding(this, UIElement.IsEnabledProperty);
				}
				base.Dispatcher.BeginInvoke((Action)delegate
				{
					SetValue(UIElement.IsEnabledProperty, false);
				});
				Keyboard.ClearFocus();
				base.Dispatcher.BeginInvoke((Action)async delegate
				{
					await Task.Delay(PRO_i32Sperrzeit);
					await base.Dispatcher.BeginInvoke((Action)delegate
					{
						SetValue(UIElement.IsEnabledProperty, true);
					});
					if (fdcBinding != null)
					{
						SetBinding(UIElement.IsEnabledProperty, fdcBinding);
					}
					base.IsHitTestVisible = true;
					if (blnHatFocus)
					{
						Focus();
					}
				});
			}
		}
	}
}
