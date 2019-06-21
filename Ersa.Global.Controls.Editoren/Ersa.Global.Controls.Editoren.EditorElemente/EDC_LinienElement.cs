using Ersa.Global.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_LinienElement : EDC_EditorElementMitPunkten
	{
		private Brush m_fdcInnenFarbe;

		private Brush m_fdcAuswahlInnenFarbe;

		private Brush m_fdcFehlerInnenFarbe;

		private Brush m_fdcFehlerAuswahlInnenFarbe;

		private double m_dblInnenBreite;

		private double m_dblAussenBreite;

		private DoubleCollection m_lstAussenStruktur;

		private List<object> m_lstPunktContentDefinitionen;

		private IEnumerable<EDC_PunktContentDefinition> m_enuPunktContentDefinitionen;

		public bool PRO_blnIstEinzelpunkt
		{
			get
			{
				IEnumerable<Point> pRO_enuNormalisiertePunkte = base.PRO_enuNormalisiertePunkte;
				if (pRO_enuNormalisiertePunkte == null)
				{
					return false;
				}
				return pRO_enuNormalisiertePunkte.Count() == 1;
			}
		}

		public double PRO_dblEinzelpunktOffset => (0.0 - PRO_dblAussenBreite) / 2.0;

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

		public double PRO_dblInnenBreite
		{
			get
			{
				return m_dblInnenBreite;
			}
			set
			{
				if (SetProperty(ref m_dblInnenBreite, value, "PRO_dblInnenBreite"))
				{
					if (PRO_dblInnenBreite < 0.0)
					{
						throw new ArgumentOutOfRangeException("PRO_dblInnenBreite", "PRO_dblInnenBreite must be >= 0");
					}
					if (PRO_dblInnenBreite + PRO_dblAussenBreite <= 0.0)
					{
						throw new ArgumentOutOfRangeException("PRO_dblInnenBreite", "Sum of PRO_dblInnenBreite and PRO_dblAussenBreite must be greater than 0");
					}
				}
			}
		}

		public double PRO_dblAussenBreite
		{
			get
			{
				return m_dblAussenBreite;
			}
			set
			{
				if (SetProperty(ref m_dblAussenBreite, value, "PRO_dblAussenBreite"))
				{
					if (PRO_dblAussenBreite < 0.0)
					{
						throw new ArgumentOutOfRangeException("PRO_dblAussenBreite", "PRO_dblAussenBreite must be >= 0");
					}
					if (PRO_dblInnenBreite + PRO_dblAussenBreite <= 0.0)
					{
						throw new ArgumentOutOfRangeException("PRO_dblAussenBreite", "Sum of PRO_dblInnenBreite and PRO_dblAussenBreite must be greater than 0");
					}
					RaisePropertyChanged("PRO_dblEinzelpunktOffset");
					SUB_BerechneRichtungsPfeile();
				}
			}
		}

		public DoubleCollection PRO_lstAussenStruktur
		{
			get
			{
				return m_lstAussenStruktur;
			}
			set
			{
				SetProperty(ref m_lstAussenStruktur, value, "PRO_lstAussenStruktur");
			}
		}

		public IEnumerable<EDC_PunktContentDefinition> PRO_enuPunktContentDefinitionen
		{
			get
			{
				return m_enuPunktContentDefinitionen;
			}
			private set
			{
				SetProperty(ref m_enuPunktContentDefinitionen, value, "PRO_enuPunktContentDefinitionen");
			}
		}

		public EDC_LinienElement()
		{
			m_fdcInnenFarbe = EDC_EditorKonstanten.PRO_fdcFarbeHell;
			m_fdcAuswahlInnenFarbe = EDC_EditorKonstanten.PRO_fdcAuswahlFarbeHell;
			m_fdcFehlerInnenFarbe = EDC_EditorKonstanten.PRO_fdcFehlerFarbeHell;
			m_fdcFehlerAuswahlInnenFarbe = EDC_EditorKonstanten.PRO_fdcFehlerAuswahlFarbeHell;
			m_dblInnenBreite = 1.0;
			m_dblAussenBreite = 1.0;
			m_lstAussenStruktur = new DoubleCollection();
			m_lstPunktContentDefinitionen = new List<object>();
		}

		public override void SUB_SetzePunkte(IEnumerable<Point> i_enuPunkte)
		{
			List<Point> list = i_enuPunkte.ToList();
			double num = 0.0;
			double num2 = 0.0;
			if (list.Count > 0)
			{
				num = list.Min((Point i_sttPunkt) => i_sttPunkt.X);
				num2 = list.Min((Point i_sttPunkt) => i_sttPunkt.Y);
			}
			List<Point> list2 = new List<Point>();
			foreach (Point item in list)
			{
				item.Offset(0.0 - num, 0.0 - num2);
				list2.Add(item);
			}
			base.PRO_sttPosition = new Point(num, num2);
			base.PRO_enuNormalisiertePunkte = list2;
			SUB_BerechneRichtungsPfeile();
			SUB_PunktContentLaengeAnpassen(list.Count);
			SUB_PunktContentDefinitionenAktualisieren();
			RaisePropertyChanged("PRO_blnIstEinzelpunkt");
			RaisePropertyChanged("PRO_blnRichtungspfeileAnzeigen");
		}

		public void SUB_SetzePunktContent(int i_i32PunktId, object i_objContent)
		{
			if (i_i32PunktId >= 0 && i_i32PunktId < m_lstPunktContentDefinitionen.Count)
			{
				m_lstPunktContentDefinitionen[i_i32PunktId] = i_objContent;
				SUB_PunktContentDefinitionenAktualisieren();
			}
		}

		public override object FUN_objPunktVerschieben(object i_objPunktReferenz, Vector i_sttDelta)
		{
			if (i_objPunktReferenz is int)
			{
				int index = (int)i_objPunktReferenz;
				List<Point> list = FUN_enuHolePunkte().ToList();
				list[index] += i_sttDelta;
				SUB_SetzePunkte(list);
			}
			return i_objPunktReferenz;
		}

		public override bool FUN_blnIstInBereich(Rect i_sttBereich)
		{
			Point[] a_sttPunkte = FUN_enuHolePunkte().ToArray();
			return FUN_blnPunkteLiegenInBereich(i_sttBereich, a_sttPunkte);
		}

		internal void SUB_SetzeNormalisiertePunkte(IEnumerable<Point> i_enuPunkte)
		{
			base.PRO_enuNormalisiertePunkte = i_enuPunkte;
			SUB_BerechneRichtungsPfeile();
			RaisePropertyChanged("PRO_blnIstEinzelpunkt");
			RaisePropertyChanged("PRO_blnRichtungspfeileAnzeigen");
		}

		protected override bool FUN_blnRichtungspfeileAnzeigen()
		{
			if (base.FUN_blnRichtungspfeileAnzeigen())
			{
				return !PRO_blnIstEinzelpunkt;
			}
			return false;
		}

		private void SUB_PunktContentLaengeAnpassen(int i_i32PunkteCount)
		{
			int num = i_i32PunkteCount - m_lstPunktContentDefinitionen.Count;
			if (num > 0)
			{
				m_lstPunktContentDefinitionen.AddRange(Enumerable.Repeat<object>(null, num));
			}
			if (num < 0)
			{
				m_lstPunktContentDefinitionen.RemoveRange(m_lstPunktContentDefinitionen.Count - Math.Abs(num), Math.Abs(num));
			}
		}

		private bool FUN_blnPunkteLiegenInBereich(Rect i_sttBereich, Point[] a_sttPunkte)
		{
			double i_dblBreite = Math.Max(PRO_dblAussenBreite, PRO_dblInnenBreite);
			return EDC_GeometrieHelfer.FUN_blnLinieSchneidetRechteck(a_sttPunkte, i_dblBreite, i_sttBereich);
		}

		private void SUB_PunktContentDefinitionenAktualisieren()
		{
			PRO_enuPunktContentDefinitionen = base.PRO_enuNormalisiertePunkte.Zip(m_lstPunktContentDefinitionen, (Point i_sttPoint, object i_objContent) => new EDC_PunktContentDefinition
			{
				PRO_dblX = i_sttPoint.X,
				PRO_dblY = i_sttPoint.Y,
				PRO_objContent = i_objContent
			});
		}

		private void SUB_BerechneRichtungsPfeile()
		{
			SUB_BerechneRichtungsPfeile(PRO_dblAussenBreite / 2.0);
		}
	}
}
