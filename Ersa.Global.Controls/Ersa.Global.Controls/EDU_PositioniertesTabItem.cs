using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_PositioniertesTabItem : TabItem
	{
		public static readonly DependencyProperty PRO_blnIsFirstProperty = DependencyProperty.Register("PRO_blnIsFirst", typeof(bool), typeof(EDU_PositioniertesTabItem));

		public static readonly DependencyProperty PRO_blnIsLastProperty = DependencyProperty.Register("PRO_blnIsLast", typeof(bool), typeof(EDU_PositioniertesTabItem));

		public static readonly DependencyProperty PRO_blnIstValideProperty = DependencyProperty.Register("PRO_blnIstValide", typeof(bool), typeof(EDU_PositioniertesTabItem), new PropertyMetadata(true));

		public bool PRO_blnIsFirst
		{
			get
			{
				return (bool)GetValue(PRO_blnIsFirstProperty);
			}
			set
			{
				SetValue(PRO_blnIsFirstProperty, value);
			}
		}

		public bool PRO_blnIsLast
		{
			get
			{
				return (bool)GetValue(PRO_blnIsLastProperty);
			}
			set
			{
				SetValue(PRO_blnIsLastProperty, value);
			}
		}

		public bool PRO_blnIstValide
		{
			get
			{
				return (bool)GetValue(PRO_blnIstValideProperty);
			}
			set
			{
				SetValue(PRO_blnIstValideProperty, value);
			}
		}
	}
}
