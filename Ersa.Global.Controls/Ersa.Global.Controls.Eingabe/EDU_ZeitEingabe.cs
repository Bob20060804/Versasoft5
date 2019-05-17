using Ersa.Global.Controls.Eingabe.ViewModels;
using System;
using System.Windows;

namespace Ersa.Global.Controls.Eingabe
{
	public class EDU_ZeitEingabe : EDU_Eingabe
	{
		public static readonly DependencyProperty PRO_sttZeitProperty;

		public static readonly DependencyProperty PRO_sttMinProperty;

		public static readonly DependencyProperty PRO_sttMaxProperty;

		public static readonly DependencyProperty PRO_blnMitSekundenProperty;

		private string m_strFormat;

		public TimeSpan PRO_sttZeit
		{
			get
			{
				return (TimeSpan)GetValue(PRO_sttZeitProperty);
			}
			set
			{
				SetValue(PRO_sttZeitProperty, value);
			}
		}

		public TimeSpan PRO_sttMin
		{
			get
			{
				return (TimeSpan)GetValue(PRO_sttMinProperty);
			}
			set
			{
				SetValue(PRO_sttMinProperty, value);
			}
		}

		public TimeSpan PRO_sttMax
		{
			get
			{
				return (TimeSpan)GetValue(PRO_sttMaxProperty);
			}
			set
			{
				SetValue(PRO_sttMaxProperty, value);
			}
		}

		public bool PRO_blnMitSekunden
		{
			get
			{
				return (bool)GetValue(PRO_blnMitSekundenProperty);
			}
			set
			{
				SetValue(PRO_blnMitSekundenProperty, value);
			}
		}

		public string PRO_strFormat
		{
			get
			{
				if (m_strFormat == null)
				{
					return "{0:hh}:{0:mm}";
				}
				return m_strFormat;
			}
			set
			{
				m_strFormat = value;
			}
		}

		static EDU_ZeitEingabe()
		{
			PRO_sttZeitProperty = DependencyProperty.Register("PRO_sttZeit", typeof(TimeSpan), typeof(EDU_ZeitEingabe), new FrameworkPropertyMetadata
			{
				BindsTwoWayByDefault = true
			});
			PRO_sttMinProperty = DependencyProperty.Register("PRO_sttMin", typeof(TimeSpan), typeof(EDU_ZeitEingabe), new PropertyMetadata(TimeSpan.Zero));
			PRO_sttMaxProperty = DependencyProperty.Register("PRO_sttMax", typeof(TimeSpan), typeof(EDU_ZeitEingabe), new PropertyMetadata(new TimeSpan(23, 59, 0)));
			PRO_blnMitSekundenProperty = DependencyProperty.Register("PRO_blnMitSekunden", typeof(bool), typeof(EDU_ZeitEingabe), new FrameworkPropertyMetadata(SUB_PropertyMitSekundenGeaendert));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ZeitEingabe), new FrameworkPropertyMetadata(typeof(EDU_ZeitEingabe)));
		}

		protected override void SUB_TastaturAnzeigen(Action i_delErfolgsAktion)
		{
			EDC_ZeitTastaturViewModel edcTastaturViewModel = new EDC_ZeitTastaturViewModel(PRO_blnMitSekunden)
			{
				PRO_strMax = string.Format(PRO_strFormat, PRO_sttMax),
				PRO_strMin = string.Format(PRO_strFormat, PRO_sttMin),
				PRO_strWert = string.Format(PRO_strFormat, PRO_sttZeit),
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
				if (edcTastaturViewModel.PRO_blnDialogResult && edcTastaturViewModel.PRO_sttWert.HasValue)
				{
					PRO_sttZeit = edcTastaturViewModel.PRO_sttWert.Value;
					if (i_delErfolgsAktion != null)
					{
						i_delErfolgsAktion();
					}
				}
			};
			eDU_NumTastatur.Show();
		}

		private static void SUB_PropertyMitSekundenGeaendert(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_ZeitEingabe eDU_ZeitEingabe = i_objSender as EDU_ZeitEingabe;
			if (eDU_ZeitEingabe != null)
			{
				if (eDU_ZeitEingabe.PRO_blnMitSekunden)
				{
					eDU_ZeitEingabe.PRO_strFormat = "{0:hh}:{0:mm}:{0:ss}";
					eDU_ZeitEingabe.PRO_sttMax = new TimeSpan(23, 59, 59);
				}
				else
				{
					eDU_ZeitEingabe.PRO_strFormat = "{0:hh}:{0:mm}";
					eDU_ZeitEingabe.PRO_sttMax = new TimeSpan(23, 59, 0);
				}
			}
		}
	}
}
