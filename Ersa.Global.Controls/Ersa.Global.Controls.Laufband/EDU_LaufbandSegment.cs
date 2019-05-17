using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls.Laufband
{
	public class EDU_LaufbandSegment : Control
	{
		public static readonly DependencyProperty PRO_blnIstAnfangProperty;

		public static readonly DependencyProperty PRO_blnIstEndeProperty;

		public static readonly DependencyProperty PRO_blnEnhaeltTransportStueckProperty;

		public static readonly DependencyProperty PRO_blnIstTransportStueckDahinterProperty;

		public bool PRO_blnIstAnfang
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAnfangProperty);
			}
			set
			{
				SetValue(PRO_blnIstAnfangProperty, value);
			}
		}

		public bool PRO_blnIstEnde
		{
			get
			{
				return (bool)GetValue(PRO_blnIstEndeProperty);
			}
			set
			{
				SetValue(PRO_blnIstEndeProperty, value);
			}
		}

		public bool PRO_blnEnhaeltTransportStueck
		{
			get
			{
				return (bool)GetValue(PRO_blnEnhaeltTransportStueckProperty);
			}
			set
			{
				SetValue(PRO_blnEnhaeltTransportStueckProperty, value);
			}
		}

		public bool PRO_blnIstTransportStueckDahinter
		{
			get
			{
				return (bool)GetValue(PRO_blnIstTransportStueckDahinterProperty);
			}
			set
			{
				SetValue(PRO_blnIstTransportStueckDahinterProperty, value);
			}
		}

		static EDU_LaufbandSegment()
		{
			PRO_blnIstAnfangProperty = DependencyProperty.Register("PRO_blnIstAnfang", typeof(bool), typeof(EDU_LaufbandSegment));
			PRO_blnIstEndeProperty = DependencyProperty.Register("PRO_blnIstEnde", typeof(bool), typeof(EDU_LaufbandSegment));
			PRO_blnEnhaeltTransportStueckProperty = DependencyProperty.Register("PRO_blnEnhaeltTransportStueck", typeof(bool), typeof(EDU_LaufbandSegment));
			PRO_blnIstTransportStueckDahinterProperty = DependencyProperty.Register("PRO_blnIstTransportStueckDahinter", typeof(bool), typeof(EDU_LaufbandSegment));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_LaufbandSegment), new FrameworkPropertyMetadata(typeof(EDU_LaufbandSegment)));
		}
	}
}
