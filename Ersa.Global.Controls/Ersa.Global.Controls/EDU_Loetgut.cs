using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_Loetgut : Control
	{
		public static readonly DependencyProperty PRO_i32KennzahlProperty;

		public static readonly DependencyProperty PRO_blnFehlerProperty;

		public static readonly DependencyProperty PRO_blnSelektiertProperty;

		public int PRO_i32Kennzahl
		{
			get
			{
				return (int)GetValue(PRO_i32KennzahlProperty);
			}
			set
			{
				SetValue(PRO_i32KennzahlProperty, value);
			}
		}

		public bool PRO_blnFehler
		{
			get
			{
				return (bool)GetValue(PRO_blnFehlerProperty);
			}
			set
			{
				SetValue(PRO_blnFehlerProperty, value);
			}
		}

		public bool PRO_blnSelektiert
		{
			get
			{
				return (bool)GetValue(PRO_blnSelektiertProperty);
			}
			set
			{
				SetValue(PRO_blnSelektiertProperty, value);
			}
		}

		static EDU_Loetgut()
		{
			PRO_i32KennzahlProperty = DependencyProperty.Register("PRO_i32Kennzahl", typeof(int), typeof(EDU_Loetgut));
			PRO_blnFehlerProperty = DependencyProperty.Register("PRO_blnFehler", typeof(bool), typeof(EDU_Loetgut));
			PRO_blnSelektiertProperty = DependencyProperty.Register("PRO_blnSelektiert", typeof(bool), typeof(EDU_Loetgut));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_Loetgut), new FrameworkPropertyMetadata(typeof(EDU_Loetgut)));
		}
	}
}
