using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_TaetigkeitsAnzeige : ContentControl
	{
		public static readonly DependencyProperty PRO_blnIstBeschaeftigtProperty;

		public static readonly DependencyProperty PRO_strAnzeigeTextProperty;

		public static readonly DependencyProperty PRO_fdcCancellationTokenSourceProperty;

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty;

		public bool PRO_blnIstBeschaeftigt
		{
			get
			{
				return (bool)GetValue(PRO_blnIstBeschaeftigtProperty);
			}
			set
			{
				SetValue(PRO_blnIstBeschaeftigtProperty, value);
			}
		}

		public string PRO_strAnzeigeText
		{
			get
			{
				return (string)GetValue(PRO_strAnzeigeTextProperty);
			}
			set
			{
				SetValue(PRO_strAnzeigeTextProperty, value);
			}
		}

		public CancellationTokenSource PRO_fdcCancellationTokenSource
		{
			get
			{
				return (CancellationTokenSource)GetValue(PRO_fdcCancellationTokenSourceProperty);
			}
			set
			{
				SetValue(PRO_fdcCancellationTokenSourceProperty, value);
			}
		}

		public string PRO_strAbbrechenText
		{
			get
			{
				return (string)GetValue(PRO_strAbbrechenTextProperty);
			}
			set
			{
				SetValue(PRO_strAbbrechenTextProperty, value);
			}
		}

		public ICommand PRO_cmdAbbrechen => new DelegateCommand(delegate
		{
			PRO_fdcCancellationTokenSource.Cancel();
		});

		static EDU_TaetigkeitsAnzeige()
		{
			PRO_blnIstBeschaeftigtProperty = DependencyProperty.Register("PRO_blnIstBeschaeftigt", typeof(bool), typeof(EDU_TaetigkeitsAnzeige));
			PRO_strAnzeigeTextProperty = DependencyProperty.Register("PRO_strAnzeigeText", typeof(string), typeof(EDU_TaetigkeitsAnzeige));
			PRO_fdcCancellationTokenSourceProperty = DependencyProperty.Register("PRO_fdcCancellationTokenSource", typeof(CancellationTokenSource), typeof(EDU_TaetigkeitsAnzeige), new PropertyMetadata((object)null));
			PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_TaetigkeitsAnzeige), new PropertyMetadata((object)null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_TaetigkeitsAnzeige), new FrameworkPropertyMetadata(typeof(EDU_TaetigkeitsAnzeige)));
		}
	}
}
