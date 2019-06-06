using Ersa.Global.Common.Helper;
using Ersa.Global.Controls.Eingabe.ViewModels;
using Ersa.Global.Controls.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls.Eingabe
{
	public class EDU_NumerischeEingabe : EDU_Eingabe
	{
		public static readonly DependencyProperty PRO_dblWertProperty;

		public static readonly DependencyProperty PRO_dblIstWertProperty;

		public static readonly DependencyProperty PRO_dblMaxProperty;

		public static readonly DependencyProperty PRO_dblMinProperty;

		public static readonly DependencyProperty PRO_dblMaxIstWertProperty;

		public static readonly DependencyProperty PRO_dblMinIstWertProperty;

		public static readonly DependencyProperty PRO_i32AnzahlNachkommastellenProperty;

		public static readonly DependencyProperty PRO_enmSollWertSichtbarkeitProperty;

		public static readonly DependencyProperty PRO_enmIstWertSichtbarkeitProperty;

		public static readonly DependencyProperty PRO_enmBeschriftungSichtbarkeitProperty;

		public static readonly DependencyProperty PRO_enmEinheitSichtbarkeitProperty;

		public static readonly DependencyProperty PRO_enmLayoutVerhaltenProperty;

		public static readonly DependencyProperty PRO_blnIstEingabeGesperrtProperty;

		public double PRO_dblWert
		{
			get
			{
				return (double)GetValue(PRO_dblWertProperty);
			}
			set
			{
				SetValue(PRO_dblWertProperty, value);
			}
		}

		public double PRO_dblIstWert
		{
			get
			{
				return (double)GetValue(PRO_dblIstWertProperty);
			}
			set
			{
				SetValue(PRO_dblIstWertProperty, value);
			}
		}

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

		public double PRO_dblMaxIstWert
		{
			get
			{
				return (double)GetValue(PRO_dblMaxIstWertProperty);
			}
			set
			{
				SetValue(PRO_dblMaxIstWertProperty, value);
			}
		}

		public double PRO_dblMinIstWert
		{
			get
			{
				return (double)GetValue(PRO_dblMinIstWertProperty);
			}
			set
			{
				SetValue(PRO_dblMinIstWertProperty, value);
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

		public Visibility PRO_enmSollWertSichtbarkeit
		{
			get
			{
				return (Visibility)GetValue(PRO_enmSollWertSichtbarkeitProperty);
			}
			set
			{
				SetValue(PRO_enmSollWertSichtbarkeitProperty, value);
			}
		}

		public Visibility PRO_enmIstWertSichtbarkeit
		{
			get
			{
				return (Visibility)GetValue(PRO_enmIstWertSichtbarkeitProperty);
			}
			set
			{
				SetValue(PRO_enmIstWertSichtbarkeitProperty, value);
			}
		}

		public Visibility PRO_enmBeschriftungSichtbarkeit
		{
			get
			{
				return (Visibility)GetValue(PRO_enmBeschriftungSichtbarkeitProperty);
			}
			set
			{
				SetValue(PRO_enmBeschriftungSichtbarkeitProperty, value);
			}
		}

		public Visibility PRO_enmEinheitSichtbarkeit
		{
			get
			{
				return (Visibility)GetValue(PRO_enmEinheitSichtbarkeitProperty);
			}
			set
			{
				SetValue(PRO_enmEinheitSichtbarkeitProperty, value);
			}
		}

		public ENUM_NumerischeEingabeLayoutVerhalten PRO_enmLayoutVerhalten
		{
			get
			{
				return (ENUM_NumerischeEingabeLayoutVerhalten)GetValue(PRO_enmLayoutVerhaltenProperty);
			}
			set
			{
				SetValue(PRO_enmLayoutVerhaltenProperty, value);
			}
		}

		public bool PRO_blnIstEingabeGesperrt
		{
			get
			{
				return (bool)GetValue(PRO_blnIstEingabeGesperrtProperty);
			}
			set
			{
				SetValue(PRO_blnIstEingabeGesperrtProperty, value);
			}
		}

		static EDU_NumerischeEingabe()
		{
			PRO_dblWertProperty = DependencyProperty.Register("PRO_dblWert", typeof(double), typeof(EDU_NumerischeEingabe), new FrameworkPropertyMetadata
			{
				BindsTwoWayByDefault = true
			});
			PRO_dblIstWertProperty = DependencyProperty.Register("PRO_dblIstWert", typeof(double), typeof(EDU_NumerischeEingabe));
			PRO_dblMaxProperty = DependencyProperty.Register("PRO_dblMax", typeof(double), typeof(EDU_NumerischeEingabe), new PropertyMetadata(10000.0));
			PRO_dblMinProperty = DependencyProperty.Register("PRO_dblMin", typeof(double), typeof(EDU_NumerischeEingabe), new PropertyMetadata(-10000.0));
			PRO_dblMaxIstWertProperty = DependencyProperty.Register("PRO_dblMaxIstWert", typeof(double), typeof(EDU_NumerischeEingabe), new PropertyMetadata(10000.0));
			PRO_dblMinIstWertProperty = DependencyProperty.Register("PRO_dblMinIstWert", typeof(double), typeof(EDU_NumerischeEingabe), new PropertyMetadata(-10000.0));
			PRO_i32AnzahlNachkommastellenProperty = DependencyProperty.Register("PRO_i32AnzahlNachkommastellen", typeof(int), typeof(EDU_NumerischeEingabe));
			PRO_enmSollWertSichtbarkeitProperty = DependencyProperty.Register("PRO_enmSollWertSichtbarkeit", typeof(Visibility), typeof(EDU_NumerischeEingabe));
			PRO_enmIstWertSichtbarkeitProperty = DependencyProperty.Register("PRO_enmIstWertSichtbarkeit", typeof(Visibility), typeof(EDU_NumerischeEingabe));
			PRO_enmBeschriftungSichtbarkeitProperty = DependencyProperty.Register("PRO_enmBeschriftungSichtbarkeit", typeof(Visibility), typeof(EDU_NumerischeEingabe));
			PRO_enmEinheitSichtbarkeitProperty = DependencyProperty.Register("PRO_enmEinheitSichtbarkeit", typeof(Visibility), typeof(EDU_NumerischeEingabe));
			PRO_enmLayoutVerhaltenProperty = DependencyProperty.Register("PRO_enmLayoutVerhalten", typeof(ENUM_NumerischeEingabeLayoutVerhalten), typeof(EDU_NumerischeEingabe));
			PRO_blnIstEingabeGesperrtProperty = DependencyProperty.Register("PRO_blnIstEingabeGesperrt", typeof(bool), typeof(EDU_NumerischeEingabe));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_NumerischeEingabe), new FrameworkPropertyMetadata(typeof(EDU_NumerischeEingabe)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			foreach (TextBox item in this.FUN_lstKindElementeSuchen<TextBox>())
			{
				item.ContextMenu = null;
			}
		}

		protected override void SUB_TastaturAnzeigen(Action i_delErfolgsAktion)
		{
			if (PRO_enmSollWertSichtbarkeit == Visibility.Visible && !PRO_blnIstEingabeGesperrt)
			{
				EDC_NumTastaturViewModel edcTastaturViewModel = new EDC_NumTastaturViewModel
				{
					PRO_strMax = EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblMax, PRO_i32AnzahlNachkommastellen),
					PRO_strMin = EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblMin, PRO_i32AnzahlNachkommastellen),
					PRO_i32NachkommaStellen = PRO_i32AnzahlNachkommastellen,
					PRO_strWert = EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblWert, PRO_i32AnzahlNachkommastellen),
					PRO_strTitel = base.PRO_strBeschriftung,
					PRO_strEinheit = base.PRO_strEinheit,
					PRO_strTextWennKeinFehler = base.PRO_strTextWennKeinFehler,
					PRO_strAbbrechenText = base.PRO_strAbbrechenText,
					PRO_strUebernehmenText = base.PRO_strUebernehmenText,
					PRO_fdcLokalisierungsConverter = base.PRO_fdcLokalisierungsConverter
				};
				EDU_NumTastatur eDU_NumTastatur = new EDU_NumTastatur(Application.Current.MainWindow);
				eDU_NumTastatur.PRO_edcViewModel = edcTastaturViewModel;
				eDU_NumTastatur.Closed += delegate
				{
					if (edcTastaturViewModel.PRO_blnDialogResult)
					{
						PRO_dblWert = edcTastaturViewModel.PRO_dblWert;
						if (i_delErfolgsAktion != null)
						{
							base.Dispatcher.BeginInvoke(i_delErfolgsAktion);
						}
					}
				};
				eDU_NumTastatur.Show();
			}
		}
	}
}
