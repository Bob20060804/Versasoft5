using Ersa.Global.Common.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ersa.Global.Controls.Eingabe.ViewModels
{
	public class EDC_NumTastaturViewModel : EDC_TastaturViewModel
	{
		private readonly IDictionary<string, Func<string, string>> m_dicSonderAktionen;

		private double m_dblWert;

		public int PRO_i32NachkommaStellen
		{
			get;
			set;
		}

		public double PRO_dblWert
		{
			get
			{
				return m_dblWert;
			}
			private set
			{
				if (!(Math.Abs(m_dblWert - value) < 1E-10))
				{
					m_dblWert = value;
					SUB_OnPropertyChanged("PRO_dblWert");
				}
			}
		}

		public override bool PRO_blnKommaSichtbar => PRO_i32NachkommaStellen > 0;

		public override bool PRO_blnNegationSichtbar => true;

		private double PRO_dblMin
		{
			get
			{
				if (double.TryParse(base.PRO_strMin, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
				{
					return result;
				}
				return -10000.0;
			}
		}

		private double PRO_dblMax
		{
			get
			{
				if (double.TryParse(base.PRO_strMax, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
				{
					return result;
				}
				return 10000.0;
			}
		}

		public EDC_NumTastaturViewModel()
		{
			m_dicSonderAktionen = new Dictionary<string, Func<string, string>>
			{
				{
					"+",
					(string i_str) => base.PRO_strWert
				},
				{
					"-",
					FUN_strVorzeichenToggeln
				},
				{
					".",
					(string i_strWert) => FUN_strKommaEinfuegen(i_strWert, PRO_i32NachkommaStellen)
				},
				{
					",",
					(string i_strWert) => FUN_strKommaEinfuegen(i_strWert, PRO_i32NachkommaStellen)
				},
				{
					"C",
					(string i_strWert) => "0"
				},
				{
					"back",
					delegate(string i_strWert)
					{
						if (i_strWert.Length <= 0)
						{
							return i_strWert;
						}
						return i_strWert.Remove(i_strWert.Length - 1);
					}
				}
			};
		}

		protected override void SUB_TextEingabe(string i_strText, bool i_blnAktuellenWertErsetzen)
		{
			if (m_dicSonderAktionen.ContainsKey(i_strText))
			{
				if (i_blnAktuellenWertErsetzen && i_strText == "-")
				{
					base.PRO_strWert = string.Empty;
				}
				base.PRO_strWert = m_dicSonderAktionen[i_strText](base.PRO_strWert);
				return;
			}
			if (i_blnAktuellenWertErsetzen)
			{
				base.PRO_strWert = i_strText;
				return;
			}
			string text = base.PRO_strWert + i_strText;
			while (text.Length > 1 && text[0] == '0' && text[1] != '.')
			{
				text = text.Substring(1);
			}
			if (text.Contains("."))
			{
				string text2 = text.Split('.').Last();
				if (text2.Length > PRO_i32NachkommaStellen)
				{
					text = text.Substring(0, text.Length - text2.Length + PRO_i32NachkommaStellen);
				}
			}
			base.PRO_strWert = text;
		}

		protected override string FUN_strWertValidieren()
		{
			if (!EDC_ZahlenFormatHelfer.FUN_blnIstZahl(base.PRO_strWert))
			{
				return "10_1537";
			}
			bool num = PRO_dblWert < PRO_dblMin;
			bool flag = PRO_dblWert > PRO_dblMax;
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
			if (double.TryParse(i_strText, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
			{
				PRO_dblWert = result;
			}
		}

		protected override void SUB_MaxSetzen()
		{
			base.PRO_strWert = EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblMax, PRO_i32NachkommaStellen);
			m_blnAktuellenWertErsetzen = false;
		}

		protected override void SUB_MinSetzen()
		{
			base.PRO_strWert = EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblMin, PRO_i32NachkommaStellen);
			m_blnAktuellenWertErsetzen = false;
		}

		protected override string FUN_strSkalaWertNachWertKonvertieren(double i_dblSkalaWert)
		{
			return EDC_ZahlenFormatHelfer.FUN_strWert(PRO_dblMin + i_dblSkalaWert * (PRO_dblMax - PRO_dblMin) / 100.0, PRO_i32NachkommaStellen);
		}

		protected override double? FUN_dblWertNachSkalaWertKonvertieren(string i_strWert)
		{
			if (Math.Abs(PRO_dblMax - PRO_dblMin) < 1E-10)
			{
				return null;
			}
			return 100.0 * (PRO_dblWert - PRO_dblMin) / (PRO_dblMax - PRO_dblMin);
		}

		private static string FUN_strVorzeichenToggeln(string i_strWert)
		{
			if (i_strWert.Length > 0)
			{
				if (!i_strWert.StartsWith("-"))
				{
					return "-" + i_strWert;
				}
				return i_strWert.Remove(0, 1);
			}
			return "-";
		}

		private static string FUN_strKommaEinfuegen(string i_strWert, int i_i32NachkommaStellen)
		{
			if (i_i32NachkommaStellen == 0 || i_strWert.Contains("."))
			{
				return i_strWert;
			}
			return i_strWert + ".";
		}
	}
}
