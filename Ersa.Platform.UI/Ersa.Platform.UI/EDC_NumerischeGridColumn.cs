using Ersa.Global.Controls.Eingabe;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Ersa.Platform.UI
{
	public class EDC_NumerischeGridColumn : GridViewBoundColumnBase
	{
		public static readonly DependencyProperty PRO_dblMaxProperty;

		public static readonly DependencyProperty PRO_dblMinProperty;

		public static readonly DependencyProperty PRO_i32AnzahlNachkommastellenProperty;

		public static readonly DependencyProperty PRO_strBeschriftungProperty;

		public static readonly DependencyProperty PRO_strEinheitProperty;

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

		public int PRO_i32AnzahlNachkommastellen
		{
			get
			{
				return (int)GetValue(PRO_i32AnzahlNachkommastellenProperty);
			}
			set
			{
				SetValue(PRO_i32AnzahlNachkommastellenProperty, value);
			}
		}

		public string PRO_strBeschriftung
		{
			get
			{
				return (string)GetValue(PRO_strBeschriftungProperty);
			}
			set
			{
				SetValue(PRO_strBeschriftungProperty, value);
			}
		}

		public string PRO_strEinheit
		{
			get
			{
				return (string)GetValue(PRO_strEinheitProperty);
			}
			set
			{
				SetValue(PRO_strEinheitProperty, value);
			}
		}

		static EDC_NumerischeGridColumn()
		{
			PRO_dblMaxProperty = DependencyProperty.Register("PRO_dblMax", typeof(double), typeof(EDC_NumerischeGridColumn), new PropertyMetadata(10000.0));
			PRO_dblMinProperty = DependencyProperty.Register("PRO_dblMin", typeof(double), typeof(EDC_NumerischeGridColumn), new PropertyMetadata(-10000.0));
			PRO_i32AnzahlNachkommastellenProperty = DependencyProperty.Register("PRO_i32AnzahlNachkommastellen", typeof(int), typeof(EDC_NumerischeGridColumn));
			PRO_strBeschriftungProperty = DependencyProperty.Register("PRO_strBeschriftung", typeof(string), typeof(EDC_NumerischeGridColumn));
			PRO_strEinheitProperty = DependencyProperty.Register("PRO_strEinheit", typeof(string), typeof(EDC_NumerischeGridColumn));
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDC_NumerischeGridColumn), new FrameworkPropertyMetadata(typeof(EDC_NumerischeGridColumn)));
		}

		public override FrameworkElement CreateCellEditElement(GridViewCell i_fdcCell, object i_objDataItem)
		{
			EDU_NumerischeEingabe eDU_NumerischeEingabe = new EDU_NumerischeEingabe();
			eDU_NumerischeEingabe.SetBinding(EDU_NumerischeEingabe.PRO_dblWertProperty, FUN_fdcErzeugeValueBinding());
			eDU_NumerischeEingabe.PRO_blnTastaturAnzeigenBeimLaden = true;
			eDU_NumerischeEingabe.PRO_dblMin = PRO_dblMin;
			eDU_NumerischeEingabe.PRO_dblMax = PRO_dblMax;
			eDU_NumerischeEingabe.PRO_i32AnzahlNachkommastellen = PRO_i32AnzahlNachkommastellen;
			eDU_NumerischeEingabe.PRO_strBeschriftung = PRO_strBeschriftung;
			eDU_NumerischeEingabe.PRO_strEinheit = PRO_strEinheit;
			eDU_NumerischeEingabe.Style = (Style)Application.Current.Resources["RadGridViewCellNumerischeEingabe_Lokalisiert"];
			eDU_NumerischeEingabe.IsEnabled = false;
			return eDU_NumerischeEingabe;
		}

		private Binding FUN_fdcErzeugeValueBinding()
		{
			return new Binding
			{
				Mode = BindingMode.TwoWay,
				NotifyOnValidationError = true,
				ValidatesOnExceptions = true,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
				Path = new PropertyPath(DataMemberBinding.Path.Path)
			};
		}
	}
}
