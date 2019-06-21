using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_RechteckElement : EDC_EditorElementMitPunkten
	{
		private double m_dblRahmenBreite;

		private DoubleCollection m_lstRahmenStruktur;

		private double m_dblBreite;

		private double m_dblHoehe;

		private Brush m_fdcInnenFarbe;

		private Brush m_fdcAuswahlInnenFarbe;

		private Brush m_fdcFehlerInnenFarbe;

		private Brush m_fdcFehlerAuswahlInnenFarbe;

		public double PRO_dblRahmenBreite
		{
			get
			{
				if (!base.PRO_blnUebergehtSkalierung)
				{
					if (base.PRO_dblSkalierung == 0.0)
					{
						return 0.0;
					}
					return m_dblRahmenBreite / base.PRO_dblSkalierung;
				}
				return m_dblRahmenBreite;
			}
			set
			{
				SetProperty(ref m_dblRahmenBreite, value, "PRO_dblRahmenBreite");
			}
		}

		public DoubleCollection PRO_lstRahmenStruktur
		{
			get
			{
				return m_lstRahmenStruktur;
			}
			set
			{
				SetProperty(ref m_lstRahmenStruktur, value, "PRO_lstRahmenStruktur");
			}
		}

		public double PRO_dblBreite
		{
			get
			{
				return m_dblBreite;
			}
			private set
			{
				SetProperty(ref m_dblBreite, value, "PRO_dblBreite");
			}
		}

		public double PRO_dblHoehe
		{
			get
			{
				return m_dblHoehe;
			}
			private set
			{
				SetProperty(ref m_dblHoehe, value, "PRO_dblHoehe");
			}
		}

		public Brush PRO_fdcInnenFarbe
		{
			get
			{
				return m_fdcInnenFarbe;
			}
			set
			{
				SetProperty(ref m_fdcInnenFarbe, value, "PRO_fdcInnenFarbe");
			}
		}

		public Brush PRO_fdcAuswahlInnenFarbe
		{
			get
			{
				return m_fdcAuswahlInnenFarbe;
			}
			set
			{
				SetProperty(ref m_fdcAuswahlInnenFarbe, value, "PRO_fdcAuswahlInnenFarbe");
			}
		}

		public Brush PRO_fdcFehlerInnenFarbe
		{
			get
			{
				return m_fdcFehlerInnenFarbe;
			}
			set
			{
				SetProperty(ref m_fdcFehlerInnenFarbe, value, "PRO_fdcFehlerInnenFarbe");
			}
		}

		public Brush PRO_fdcFehlerAuswahlInnenFarbe
		{
			get
			{
				return m_fdcFehlerAuswahlInnenFarbe;
			}
			set
			{
				SetProperty(ref m_fdcFehlerAuswahlInnenFarbe, value, "PRO_fdcFehlerAuswahlInnenFarbe");
			}
		}

		public EDC_RechteckElement()
		{
			m_dblRahmenBreite = 2.0;
			m_lstRahmenStruktur = new DoubleCollection(new double[2]
			{
				2.0,
				1.0
			});
			m_fdcInnenFarbe = EDC_EditorKonstanten.PRO_fdcFarbeHell;
			m_fdcAuswahlInnenFarbe = EDC_EditorKonstanten.PRO_fdcAuswahlFarbeHell;
			m_fdcFehlerInnenFarbe = EDC_EditorKonstanten.PRO_fdcFehlerFarbeHell;
			m_fdcFehlerAuswahlInnenFarbe = EDC_EditorKonstanten.PRO_fdcFehlerAuswahlFarbeHell;
		}

		public override object FUN_objPunktVerschieben(object i_objPunktReferenz, Vector i_sttDelta)
		{
			if (!(i_objPunktReferenz is int))
			{
				return i_objPunktReferenz;
			}
			int num = (int)i_objPunktReferenz;
			Point point;
			Vector vector;
			switch (num)
			{
			case 0:
				point = base.PRO_sttPosition + i_sttDelta;
				vector = new Vector(PRO_dblBreite - i_sttDelta.X, PRO_dblHoehe - i_sttDelta.Y);
				break;
			case 1:
				point = base.PRO_sttPosition + new Vector(0.0, i_sttDelta.Y);
				vector = new Vector(PRO_dblBreite + i_sttDelta.X, PRO_dblHoehe - i_sttDelta.Y);
				break;
			case 2:
				point = base.PRO_sttPosition;
				vector = new Vector(PRO_dblBreite + i_sttDelta.X, PRO_dblHoehe + i_sttDelta.Y);
				break;
			case 3:
				point = base.PRO_sttPosition + new Vector(i_sttDelta.X, 0.0);
				vector = new Vector(PRO_dblBreite - i_sttDelta.X, PRO_dblHoehe + i_sttDelta.Y);
				break;
			default:
				return i_objPunktReferenz;
			}
			bool i_blnHorizontalNormalisiert = false;
			bool i_blnVertikalNormalisiert = false;
			if (vector.X < 0.0)
			{
				vector = new Vector(0.0 - vector.X, vector.Y);
				point.Offset(0.0 - vector.X, 0.0);
				i_blnHorizontalNormalisiert = true;
			}
			if (vector.Y < 0.0)
			{
				vector = new Vector(vector.X, 0.0 - vector.Y);
				point.Offset(0.0, 0.0 - vector.Y);
				i_blnVertikalNormalisiert = true;
			}
			SUB_SetzePunkte(new Rect(point, vector));
			return FUN_i32HoleVerschiebungsIndexNachNormalisierung(num, i_blnHorizontalNormalisiert, i_blnVertikalNormalisiert);
		}

		public override void SUB_SetzePunkte(IEnumerable<Point> i_enuPunkte)
		{
			List<Point> obj = i_enuPunkte?.ToList() ?? new List<Point>();
			if (obj.Count != 4)
			{
				throw new ArgumentOutOfRangeException("i_enuPunkte", "Point enumeration must contain exactly four elements");
			}
			double num = obj.Min((Point i_sttPunkt) => i_sttPunkt.X);
			double num2 = obj.Max((Point i_sttPunkt) => i_sttPunkt.X);
			double num3 = obj.Min((Point i_sttPunkt) => i_sttPunkt.Y);
			double num4 = obj.Max((Point i_sttPunkt) => i_sttPunkt.Y);
			SUB_SetzePunkte(new Rect(num, num3, num2 - num, num4 - num3));
		}

		public void SUB_SetzePunkte(Rect i_sttRechteck)
		{
			base.PRO_sttPosition = new Point(i_sttRechteck.X, i_sttRechteck.Y);
			SUB_AendereGroesse(i_sttRechteck.Size);
		}

		public void SUB_AendereGroesse(Size i_sttGroesse)
		{
			PRO_dblBreite = i_sttGroesse.Width;
			PRO_dblHoehe = i_sttGroesse.Height;
			base.PRO_enuNormalisiertePunkte = new Point[4]
			{
				new Point(0.0, 0.0),
				new Point(i_sttGroesse.Width, 0.0),
				new Point(i_sttGroesse.Width, i_sttGroesse.Height),
				new Point(0.0, i_sttGroesse.Height)
			};
		}

		public override bool FUN_blnIstInBereich(Rect i_sttBereich)
		{
			return FUN_sttErmittleBereich().IntersectsWith(i_sttBereich);
		}

		public Rect FUN_sttErmittleBereich()
		{
			return new Rect(base.PRO_sttPosition, new Vector(PRO_dblBreite, PRO_dblHoehe));
		}

		protected override void SUB_OnSkalierungGeaendert()
		{
			base.SUB_OnSkalierungGeaendert();
			RaisePropertyChanged("PRO_dblRahmenBreite");
		}

		private static int FUN_i32HoleVerschiebungsIndexNachNormalisierung(int i_i32Index, bool i_blnHorizontalNormalisiert, bool i_blnVertikalNormalisiert)
		{
			if (i_blnHorizontalNormalisiert && i_blnVertikalNormalisiert)
			{
				return (i_i32Index + 2) % 4;
			}
			if (i_blnHorizontalNormalisiert && !i_blnVertikalNormalisiert)
			{
				if (i_i32Index == 0 || i_i32Index == 2)
				{
					return i_i32Index + 1;
				}
				return i_i32Index - 1;
			}
			if (!i_blnHorizontalNormalisiert && i_blnVertikalNormalisiert)
			{
				return 3 - i_i32Index;
			}
			return i_i32Index;
		}
	}
}
