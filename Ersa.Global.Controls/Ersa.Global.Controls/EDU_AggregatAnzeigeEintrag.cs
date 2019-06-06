using Ersa.Global.Common.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Ersa.Global.Controls
{
	public class EDU_AggregatAnzeigeEintrag : Control
	{
		public static readonly DependencyProperty PRO_i32NummerProperty;

		public static readonly DependencyProperty PRO_dblWertProperty;

		public static readonly DependencyProperty PRO_strWertEinheitProperty;

		public static readonly DependencyProperty PRO_enmWertFormatierungProperty;

		public static readonly DependencyProperty PRO_blnIstAusgeblendetProperty;

		private static readonly DependencyPropertyKey PRO_strFormatierterWertPropertyKey;

		public int? PRO_i32Nummer
		{
			get
			{
				return (int?)GetValue(PRO_i32NummerProperty);
			}
			set
			{
				SetValue(PRO_i32NummerProperty, value);
			}
		}

		public double? PRO_dblWert
		{
			get
			{
				return (double?)GetValue(PRO_dblWertProperty);
			}
			set
			{
				SetValue(PRO_dblWertProperty, value);
			}
		}

		public string PRO_strWertEinheit
		{
			get
			{
				return (string)GetValue(PRO_strWertEinheitProperty);
			}
			set
			{
				SetValue(PRO_strWertEinheitProperty, value);
			}
		}

		public ENUM_WertFormatierung PRO_enmWertFormatierung
		{
			get
			{
				return (ENUM_WertFormatierung)GetValue(PRO_enmWertFormatierungProperty);
			}
			set
			{
				SetValue(PRO_enmWertFormatierungProperty, value);
			}
		}

		public string PRO_strFormatierterWert => (string)GetValue(PRO_strFormatierterWertPropertyKey.DependencyProperty);

		public bool PRO_blnIstAusgeblendet
		{
			get
			{
				return (bool)GetValue(PRO_blnIstAusgeblendetProperty);
			}
			set
			{
				SetValue(PRO_blnIstAusgeblendetProperty, value);
			}
		}

		static EDU_AggregatAnzeigeEintrag()
		{
			PRO_i32NummerProperty = DependencyProperty.Register("PRO_i32Nummer", typeof(int?), typeof(EDU_AggregatAnzeigeEintrag));
			PRO_dblWertProperty = DependencyProperty.Register("PRO_dblWert", typeof(double?), typeof(EDU_AggregatAnzeigeEintrag), new PropertyMetadata(SUB_FormatiertenWertBerechnen));
			PRO_strWertEinheitProperty = DependencyProperty.Register("PRO_strWertEinheit", typeof(string), typeof(EDU_AggregatAnzeigeEintrag));
			PRO_enmWertFormatierungProperty = DependencyProperty.Register("PRO_enmWertFormatierung", typeof(ENUM_WertFormatierung), typeof(EDU_AggregatAnzeigeEintrag), new PropertyMetadata(SUB_FormatiertenWertBerechnen));
			PRO_blnIstAusgeblendetProperty = DependencyProperty.Register("PRO_blnIstAusgeblendet", typeof(bool), typeof(EDU_AggregatAnzeigeEintrag));
			PRO_strFormatierterWertPropertyKey = DependencyProperty.RegisterReadOnly("PRO_strFormatierterWert", typeof(string), typeof(EDU_AggregatAnzeigeEintrag), new PropertyMetadata(string.Empty));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_AggregatAnzeigeEintrag), new FrameworkPropertyMetadata(typeof(EDU_AggregatAnzeigeEintrag)));
		}

		private static void SUB_FormatiertenWertBerechnen(DependencyObject i_fdcSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_AggregatAnzeigeEintrag eDU_AggregatAnzeigeEintrag = i_fdcSender as EDU_AggregatAnzeigeEintrag;
			if (eDU_AggregatAnzeigeEintrag != null)
			{
				string value = string.Empty;
				switch (eDU_AggregatAnzeigeEintrag.PRO_enmWertFormatierung)
				{
				case ENUM_WertFormatierung.enmUnformatiert:
					value = (eDU_AggregatAnzeigeEintrag.PRO_dblWert.HasValue ? eDU_AggregatAnzeigeEintrag.PRO_dblWert.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
					break;
				case ENUM_WertFormatierung.enmSekunden:
					value = $"{eDU_AggregatAnzeigeEintrag.PRO_dblWert ?? 0.0:0}";
					break;
				case ENUM_WertFormatierung.enmMinutenSekunden:
					value = TimeSpan.FromSeconds(eDU_AggregatAnzeigeEintrag.PRO_dblWert ?? 0.0).ToString("m\\:ss");
					break;
				case ENUM_WertFormatierung.enmNumerischEineNachkommaStelle:
					value = EDC_ZahlenFormatHelfer.FUN_strWert(eDU_AggregatAnzeigeEintrag.PRO_dblWert.GetValueOrDefault(), 1);
					break;
				}
				eDU_AggregatAnzeigeEintrag.SetValue(PRO_strFormatierterWertPropertyKey, value);
			}
		}
	}
}
