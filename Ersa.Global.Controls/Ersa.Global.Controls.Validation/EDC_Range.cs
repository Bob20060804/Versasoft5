using System.Windows;

namespace Ersa.Global.Controls.Validation
{
	public class EDC_Range : Freezable
	{
		public static readonly DependencyProperty PRO_dblMaxProperty = DependencyProperty.Register("PRO_dblMax", typeof(double), typeof(EDC_Range), new PropertyMetadata(1.7976931348623157E+308));

		public static readonly DependencyProperty PRO_dblMinProperty = DependencyProperty.Register("PRO_dblMin", typeof(double), typeof(EDC_Range), new PropertyMetadata(-1.7976931348623157E+308));

		public double PRO_dblMax
		{
			get
			{
				return (double)GetValue(PRO_dblMaxProperty);
			}
			set
			{
				SetValue(PRO_dblMaxProperty, value);
			}
		}

		public double PRO_dblMin
		{
			get
			{
				return (double)GetValue(PRO_dblMinProperty);
			}
			set
			{
				SetValue(PRO_dblMinProperty, value);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return this;
		}
	}
}
