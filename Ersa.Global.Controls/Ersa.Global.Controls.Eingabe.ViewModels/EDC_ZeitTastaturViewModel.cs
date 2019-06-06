using System;
using System.Collections.Generic;

namespace Ersa.Global.Controls.Eingabe.ViewModels
{
	public class EDC_ZeitTastaturViewModel : EDC_TastaturViewModel
	{
		private const string mC_strMaske = "__:__";

		private const string mC_strMaskeMitSekunden = "__:__:__";

		private static bool m_blnMitSekunden;

		private readonly IDictionary<string, Func<string, string>> m_dicSonderAktionen;

		private string m_strMaske = "__:__";

		private TimeSpan? m_sttWert;

		public TimeSpan? PRO_sttWert
		{
			get
			{
				return m_sttWert;
			}
			private set
			{
				if (!object.Equals(m_sttWert, value))
				{
					m_sttWert = value;
					SUB_OnPropertyChanged("PRO_sttWert");
				}
			}
		}

		public override bool PRO_blnKommaSichtbar => false;

		public override bool PRO_blnNegationSichtbar => false;

		private TimeSpan? PRO_sttMin => FUN_strZeitStringNachTimeSpan(base.PRO_strMin);

		private TimeSpan? PRO_sttMax => FUN_strZeitStringNachTimeSpan(base.PRO_strMax);

		public EDC_ZeitTastaturViewModel(bool i_blnMitSekunden = false)
		{
			m_blnMitSekunden = i_blnMitSekunden;
			if (m_blnMitSekunden)
			{
				m_strMaske = "__:__:__";
			}
			base.PRO_strWert = m_strMaske;
			m_dicSonderAktionen = new Dictionary<string, Func<string, string>>
			{
				{
					"C",
					(string i_strWert) => m_strMaske
				},
				{
					"back",
					FUN_strLetztesZeichenEntfernen
				}
			};
		}

		protected override void SUB_TextEingabe(string i_strText, bool i_blnAktuellenWertErsetzen)
		{
			if (m_dicSonderAktionen.ContainsKey(i_strText))
			{
				base.PRO_strWert = m_dicSonderAktionen[i_strText](base.PRO_strWert);
			}
			else
			{
				if (i_strText.Length != 1 || !int.TryParse(i_strText, out int _))
				{
					return;
				}
				if (i_blnAktuellenWertErsetzen)
				{
					base.PRO_strWert = i_strText + m_strMaske.Substring(i_strText.Length);
					return;
				}
				int num = base.PRO_strWert.IndexOf('_');
				if (num > -1)
				{
					base.PRO_strWert = base.PRO_strWert.Substring(0, num) + i_strText + base.PRO_strWert.Substring(num + 1);
				}
			}
		}

		protected override string FUN_strWertValidieren()
		{
			if (!PRO_sttWert.HasValue)
			{
				return "10_1537";
			}
			bool num = PRO_sttMin.HasValue && PRO_sttWert.Value < PRO_sttMin.Value;
			bool flag = PRO_sttMax.HasValue && PRO_sttWert.Value > PRO_sttMax.Value;
			if (!num)
			{
				if (!flag)
				{
					return string.Empty;
				}
				return "4_9180";
			}
			return "4_3014";
		}

		protected override void SUB_TextGeaendert(string i_strText)
		{
			PRO_sttWert = FUN_strZeitStringNachTimeSpan(i_strText);
		}

		protected override void SUB_MaxSetzen()
		{
			base.PRO_strWert = base.PRO_strMax;
		}

		protected override void SUB_MinSetzen()
		{
			base.PRO_strWert = base.PRO_strMin;
		}

		protected override string FUN_strSkalaWertNachWertKonvertieren(double i_dblSkalaWert)
		{
			if (PRO_sttMin.HasValue && PRO_sttMax.HasValue)
			{
				if (!m_blnMitSekunden)
				{
					int num = FUN_i32ZeitNachMinutenTotal(PRO_sttMin.Value);
					int num2 = FUN_i32ZeitNachMinutenTotal(PRO_sttMax.Value);
					double num3 = (double)num + i_dblSkalaWert * (double)(num2 - num) / 100.0;
					int num4 = (int)num3 / 60;
					int num5 = (int)Math.Round(num3 % 60.0);
					if (num5 == 60)
					{
						num4++;
						num5 = 0;
					}
					return $"{num4:00}:{num5:00}";
				}
				int num6 = FUN_i32ZeitNachSekundenTotal(PRO_sttMin.Value);
				int num7 = FUN_i32ZeitNachSekundenTotal(PRO_sttMax.Value);
				double num8 = (double)num6 + i_dblSkalaWert * (double)(num7 - num6) / 100.0;
				int num9 = (int)num8 / 3600;
				double num10 = num8 - (double)(num9 * 3600);
				int num11 = (int)num10 / 60;
				int num12 = (int)Math.Round(num10 % 60.0);
				if (num12 == 60)
				{
					num11++;
					num12 = 0;
				}
				if (num11 == 60)
				{
					num9++;
					num11 = 0;
				}
				return $"{num9:00}:{num11:00}:{num12:00}";
			}
			return null;
		}

		protected override double? FUN_dblWertNachSkalaWertKonvertieren(string i_strWert)
		{
			TimeSpan? timeSpan = FUN_strZeitStringNachTimeSpan(i_strWert);
			if (PRO_sttMin.HasValue && PRO_sttMax.HasValue && timeSpan.HasValue && PRO_sttMin.Value != PRO_sttMax.Value)
			{
				if (!m_blnMitSekunden)
				{
					int num = FUN_i32ZeitNachMinutenTotal(PRO_sttMin.Value);
					int num2 = FUN_i32ZeitNachMinutenTotal(PRO_sttMax.Value);
					int num3 = FUN_i32ZeitNachMinutenTotal(timeSpan.Value);
					return 100.0 * (double)(num3 - num) / (double)(num2 - num);
				}
				int num4 = FUN_i32ZeitNachSekundenTotal(PRO_sttMin.Value);
				int num5 = FUN_i32ZeitNachSekundenTotal(PRO_sttMax.Value);
				int num6 = FUN_i32ZeitNachSekundenTotal(timeSpan.Value);
				return 100.0 * (double)(num6 - num4) / (double)(num5 - num4);
			}
			return null;
		}

		private static int FUN_i32ZeitNachMinutenTotal(TimeSpan i_sttZeit)
		{
			return i_sttZeit.Hours * 60 + i_sttZeit.Minutes;
		}

		private static int FUN_i32ZeitNachSekundenTotal(TimeSpan i_sttZeit)
		{
			return (i_sttZeit.Hours * 60 + i_sttZeit.Minutes) * 60 + i_sttZeit.Seconds;
		}

		private static TimeSpan? FUN_strZeitStringNachTimeSpan(string i_strZeit)
		{
			if (i_strZeit != null && i_strZeit.Contains(":"))
			{
				string[] array = i_strZeit.Split(':');
				int result2;
				int result;
				if (i_strZeit.Length == 5 && !m_blnMitSekunden && array.Length == 2 && int.TryParse(array[0], out result) && int.TryParse(array[1], out result2) && result >= 0 && result < 24 && result2 >= 0 && result2 < 60)
				{
					return new TimeSpan(result, result2, 0);
				}
				int result5;
				int result4;
				int result3;
				if (i_strZeit.Length == 8 && m_blnMitSekunden && array.Length == 3 && int.TryParse(array[0], out result3) && int.TryParse(array[1], out result4) && int.TryParse(array[2], out result5) && result3 >= 0 && result3 < 24 && result4 >= 0 && result4 < 60 && result5 >= 0 && result5 < 60)
				{
					return new TimeSpan(result3, result4, result5);
				}
			}
			return null;
		}

		private static string FUN_strLetztesZeichenEntfernen(string i_strWert)
		{
			for (int num = i_strWert.Length - 1; num >= 0; num--)
			{
				char c = i_strWert[num];
				if (c != ':' && c != '_')
				{
					string text = i_strWert.Substring(0, num) + "_";
					if (i_strWert.Length >= num)
					{
						text += i_strWert.Substring(num + 1);
					}
					i_strWert = text;
					break;
				}
			}
			return i_strWert;
		}
	}
}
