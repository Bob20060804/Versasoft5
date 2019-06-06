using Ersa.Global.Controls.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_CommandTextBox : TextBox
	{
		public static readonly DependencyProperty PRO_cmdTextGeandertProperty = DependencyProperty.Register("PRO_cmdTextGeandert", typeof(ICommand), typeof(EDU_CommandTextBox));

		public ICommand PRO_cmdTextGeandert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdTextGeandertProperty);
			}
			set
			{
				SetValue(PRO_cmdTextGeandertProperty, value);
			}
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			if (PRO_cmdTextGeandert != null)
			{
				PRO_cmdTextGeandert.SUB_Execute(base.Text, this);
			}
		}
	}
}
