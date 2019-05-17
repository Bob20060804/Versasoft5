using System.Windows;

namespace Ersa.Global.Controls
{
	public class EDC_BindingProxy : Freezable
	{
		public static readonly DependencyProperty PRO_objDataProperty = DependencyProperty.Register("PRO_objData", typeof(object), typeof(EDC_BindingProxy), new UIPropertyMetadata(null));

		public object PRO_objData
		{
			get
			{
				return GetValue(PRO_objDataProperty);
			}
			set
			{
				SetValue(PRO_objDataProperty, value);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new EDC_BindingProxy();
		}
	}
}
