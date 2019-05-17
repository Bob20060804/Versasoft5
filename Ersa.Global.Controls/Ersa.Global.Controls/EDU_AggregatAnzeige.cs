using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_AggregatAnzeige : Control
	{
		public static readonly DependencyProperty PRO_lstEintraegeProperty;

		public static readonly DependencyProperty PRO_lstZusatzEintraegeProperty;

		public static readonly DependencyProperty PRO_blnZusatzEintraegeSichtbarProperty;

		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_strIconAktivUriProperty;

		public static readonly DependencyProperty PRO_strIconUriWennObenProperty;

		public static readonly DependencyProperty PRO_strIconAktivUriWennObenProperty;

		public static readonly DependencyProperty PRO_cmdCommandProperty;

		public static readonly DependencyProperty PRO_objCommandParameterProperty;

		public static readonly DependencyProperty PRO_blnIstAktivProperty;

		public static readonly DependencyProperty PRO_blnDatenOberhalbAnzeigenProperty;

		public static readonly DependencyProperty PRO_blnIstErstesAggregatProperty;

		public static readonly DependencyProperty PRO_blnIstLetztesAggregatProperty;

		public static readonly DependencyProperty PRO_strToolTipProperty;

		public static readonly DependencyProperty PRO_blnEintraegeUeberRandVerschiebenProperty;

		public static readonly DependencyProperty PRO_dblAbstandEintraegeProperty;

		public ObservableCollection<FrameworkElement> PRO_lstEintraege
		{
			get
			{
				return (ObservableCollection<FrameworkElement>)GetValue(PRO_lstEintraegeProperty);
			}
			set
			{
				SetValue(PRO_lstEintraegeProperty, value);
			}
		}

		public ObservableCollection<FrameworkElement> PRO_lstZusatzEintraege
		{
			get
			{
				return (ObservableCollection<FrameworkElement>)GetValue(PRO_lstZusatzEintraegeProperty);
			}
			set
			{
				SetValue(PRO_lstZusatzEintraegeProperty, value);
			}
		}

		public bool PRO_blnZusatzEintraegeSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnZusatzEintraegeSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnZusatzEintraegeSichtbarProperty, value);
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

		public string PRO_strIconAktivUri
		{
			get
			{
				return (string)GetValue(PRO_strIconAktivUriProperty);
			}
			set
			{
				SetValue(PRO_strIconAktivUriProperty, value);
			}
		}

		public string PRO_strIconUriWennOben
		{
			get
			{
				return (string)GetValue(PRO_strIconUriWennObenProperty);
			}
			set
			{
				SetValue(PRO_strIconUriWennObenProperty, value);
			}
		}

		public string PRO_strIconAktivUriWennOben
		{
			get
			{
				return (string)GetValue(PRO_strIconAktivUriWennObenProperty);
			}
			set
			{
				SetValue(PRO_strIconAktivUriWennObenProperty, value);
			}
		}

		public ICommand PRO_cmdCommand
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdCommandProperty, value);
			}
		}

		public object PRO_objCommandParameter
		{
			get
			{
				return GetValue(PRO_objCommandParameterProperty);
			}
			set
			{
				SetValue(PRO_objCommandParameterProperty, value);
			}
		}

		public bool PRO_blnIstAktiv
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAktivProperty);
			}
			set
			{
				SetValue(PRO_blnIstAktivProperty, value);
			}
		}

		public bool PRO_blnDatenOberhalbAnzeigen
		{
			get
			{
				return (bool)GetValue(PRO_blnDatenOberhalbAnzeigenProperty);
			}
			set
			{
				SetValue(PRO_blnDatenOberhalbAnzeigenProperty, value);
			}
		}

		public bool PRO_blnIstErstesAggregat
		{
			get
			{
				return (bool)GetValue(PRO_blnIstErstesAggregatProperty);
			}
			set
			{
				SetValue(PRO_blnIstErstesAggregatProperty, value);
			}
		}

		public bool PRO_blnIstLetztesAggregat
		{
			get
			{
				return (bool)GetValue(PRO_blnIstLetztesAggregatProperty);
			}
			set
			{
				SetValue(PRO_blnIstLetztesAggregatProperty, value);
			}
		}

		public string PRO_strToolTip
		{
			get
			{
				return (string)GetValue(PRO_strToolTipProperty);
			}
			set
			{
				SetValue(PRO_strToolTipProperty, value);
			}
		}

		public bool PRO_blnEintraegeUeberRandVerschieben
		{
			get
			{
				return (bool)GetValue(PRO_blnEintraegeUeberRandVerschiebenProperty);
			}
			set
			{
				SetValue(PRO_blnEintraegeUeberRandVerschiebenProperty, value);
			}
		}

		public double PRO_dblAbstandEintraege
		{
			get
			{
				return (double)GetValue(PRO_dblAbstandEintraegeProperty);
			}
			set
			{
				SetValue(PRO_dblAbstandEintraegeProperty, value);
			}
		}

		static EDU_AggregatAnzeige()
		{
			PRO_lstEintraegeProperty = DependencyProperty.Register("PRO_lstEintraege", typeof(ObservableCollection<FrameworkElement>), typeof(EDU_AggregatAnzeige));
			PRO_lstZusatzEintraegeProperty = DependencyProperty.Register("PRO_lstZusatzEintraege", typeof(ObservableCollection<FrameworkElement>), typeof(EDU_AggregatAnzeige));
			PRO_blnZusatzEintraegeSichtbarProperty = DependencyProperty.Register("PRO_blnZusatzEintraegeSichtbar", typeof(bool), typeof(EDU_AggregatAnzeige), new FrameworkPropertyMetadata(true));
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_AggregatAnzeige));
			PRO_strIconAktivUriProperty = DependencyProperty.Register("PRO_strIconAktivUri", typeof(string), typeof(EDU_AggregatAnzeige));
			PRO_strIconUriWennObenProperty = DependencyProperty.Register("PRO_strIconUriWennOben", typeof(string), typeof(EDU_AggregatAnzeige));
			PRO_strIconAktivUriWennObenProperty = DependencyProperty.Register("PRO_strIconAktivUriWennOben", typeof(string), typeof(EDU_AggregatAnzeige));
			PRO_cmdCommandProperty = DependencyProperty.Register("PRO_cmdCommand", typeof(ICommand), typeof(EDU_AggregatAnzeige));
			PRO_objCommandParameterProperty = DependencyProperty.Register("PRO_objCommandParameter", typeof(object), typeof(EDU_AggregatAnzeige));
			PRO_blnIstAktivProperty = DependencyProperty.Register("PRO_blnIstAktiv", typeof(bool), typeof(EDU_AggregatAnzeige));
			PRO_blnDatenOberhalbAnzeigenProperty = DependencyProperty.Register("PRO_blnDatenOberhalbAnzeigen", typeof(bool), typeof(EDU_AggregatAnzeige));
			PRO_blnIstErstesAggregatProperty = DependencyProperty.Register("PRO_blnIstErstesAggregat", typeof(bool), typeof(EDU_AggregatAnzeige));
			PRO_blnIstLetztesAggregatProperty = DependencyProperty.Register("PRO_blnIstLetztesAggregat", typeof(bool), typeof(EDU_AggregatAnzeige));
			PRO_strToolTipProperty = DependencyProperty.Register("PRO_strToolTip", typeof(string), typeof(EDU_AggregatAnzeige));
			PRO_blnEintraegeUeberRandVerschiebenProperty = DependencyProperty.Register("PRO_blnEintraegeUeberRandVerschieben", typeof(bool), typeof(EDU_AggregatAnzeige));
			PRO_dblAbstandEintraegeProperty = DependencyProperty.Register("PRO_dblAbstandEintraege", typeof(double), typeof(EDU_AggregatAnzeige));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_AggregatAnzeige), new FrameworkPropertyMetadata(typeof(EDU_AggregatAnzeige)));
		}

		public EDU_AggregatAnzeige()
		{
			PRO_lstEintraege = new ObservableCollection<FrameworkElement>();
			PRO_lstZusatzEintraege = new ObservableCollection<FrameworkElement>();
		}
	}
}
