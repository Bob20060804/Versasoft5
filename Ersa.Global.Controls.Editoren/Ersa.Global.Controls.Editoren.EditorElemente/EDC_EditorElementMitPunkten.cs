using Ersa.Global.Controls.Editoren.Helfer;
using Ersa.Global.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public abstract class EDC_EditorElementMitPunkten : EDC_EditorElementMitInhalt
	{
		public class EDC_AnfasserDefinition : BindableBase
		{
			private double m_dblGroesse;

			private double m_dblSkalierung;

			public double PRO_dblGroesse
			{
				get
				{
					if (PRO_dblSkalierung == 0.0)
					{
						return 0.0;
					}
					return m_dblGroesse / PRO_dblSkalierung;
				}
				set
				{
					SetProperty(ref m_dblGroesse, value, "PRO_dblGroesse");
				}
			}

			public double PRO_dblOffset => (0.0 - PRO_dblGroesse) / 2.0;

			public double PRO_dblRahmenBreite => (0.0 - PRO_dblGroesse) / 4.0;

			public double PRO_dblSkalierung
			{
				private get
				{
					return m_dblSkalierung;
				}
				set
				{
					if (Math.Abs(m_dblSkalierung - value) > 1E-10)
					{
						m_dblSkalierung = value;
						RaisePropertyChanged("PRO_dblGroesse");
						RaisePropertyChanged("PRO_dblOffset");
						RaisePropertyChanged("PRO_dblRahmenBreite");
					}
				}
			}
		}

		private IEnumerable<Point> m_enuNormalisiertePunkte = Enumerable.Empty<Point>();

		private bool m_blnPunkteAnzeigen;

		private bool m_blnRichtungspfeileMoeglich;

		private IEnumerable<EDC_RichtungspfeilDefinition> m_enuRichtungsPfeile = Enumerable.Empty<EDC_RichtungspfeilDefinition>();

		public IEnumerable<Point> PRO_enuNormalisiertePunkte
		{
			get
			{
				return m_enuNormalisiertePunkte;
			}
			protected set
			{
				SetProperty(ref m_enuNormalisiertePunkte, value, "PRO_enuNormalisiertePunkte");
			}
		}

		public bool PRO_blnPunkteAnzeigen
		{
			get
			{
				return m_blnPunkteAnzeigen;
			}
			private set
			{
				SetProperty(ref m_blnPunkteAnzeigen, value, "PRO_blnPunkteAnzeigen");
			}
		}

		public EDC_AnfasserDefinition PRO_edcAnfasserDefinition
		{
			get;
		}

		public bool PRO_blnRichtungspfeileMoeglich
		{
			get
			{
				return m_blnRichtungspfeileMoeglich;
			}
			set
			{
				if (SetProperty(ref m_blnRichtungspfeileMoeglich, value, "PRO_blnRichtungspfeileMoeglich"))
				{
					RaisePropertyChanged("PRO_blnRichtungspfeileAnzeigen");
				}
			}
		}

		public bool PRO_blnRichtungspfeileAnzeigen => FUN_blnRichtungspfeileAnzeigen();

		public IEnumerable<EDC_RichtungspfeilDefinition> PRO_enuRichtungsPfeile
		{
			get
			{
				return m_enuRichtungsPfeile;
			}
			private set
			{
				SetProperty(ref m_enuRichtungsPfeile, value, "PRO_enuRichtungsPfeile");
			}
		}

		protected EDC_EditorElementMitPunkten()
		{
			PRO_edcAnfasserDefinition = new EDC_AnfasserDefinition
			{
				PRO_dblGroesse = 10.0
			};
		}

		public abstract object FUN_objPunktVerschieben(object i_objPunktReferenz, Vector i_sttDelta);

		public abstract void SUB_SetzePunkte(IEnumerable<Point> i_enuPunkte);

		public IEnumerable<Point> FUN_enuHolePunkte()
		{
			return from i_sttPunkt in PRO_enuNormalisiertePunkte
			select base.PRO_sttPosition + new Vector(i_sttPunkt.X, i_sttPunkt.Y);
		}

		public object FUN_objErmittlePunktReferenzAnPosition(Point i_sttPosition, double i_dblToleranzBereich = 6.0)
		{
			if (!PRO_blnPunkteAnzeigen)
			{
				return null;
			}
			double num = i_dblToleranzBereich / base.PRO_dblSkalierung;
			Rect rect = new Rect(i_sttPosition - new Vector(num, num), new Size(2.0 * num, 2.0 * num));
			List<Point> list = FUN_enuHolePunkte().ToList();
			for (int i = 0; i < list.Count; i++)
			{
				Point point = list[i];
				if (rect.Contains(point))
				{
					return i;
				}
			}
			return null;
		}

		protected virtual bool FUN_blnRichtungspfeileAnzeigen()
		{
			return PRO_blnRichtungspfeileMoeglich;
		}

		protected override void SUB_OnAuswahlGeaendert(bool i_blnAusgewaehlt)
		{
			base.SUB_OnAuswahlGeaendert(i_blnAusgewaehlt);
			PRO_blnPunkteAnzeigen = i_blnAusgewaehlt;
		}

		protected override void SUB_OnSkalierungGeaendert()
		{
			base.SUB_OnSkalierungGeaendert();
			PRO_edcAnfasserDefinition.PRO_dblSkalierung = base.PRO_dblSkalierung;
		}

		protected void SUB_BerechneRichtungsPfeile(double i_dblPfeilBreite)
		{
			List<EDC_RichtungspfeilDefinition> list = EDC_LinienAbschnittHelfer.FUN_enuHoleRichtungspfeile(PRO_enuNormalisiertePunkte).ToList();
			foreach (EDC_RichtungspfeilDefinition item in list)
			{
				item.PRO_dblBreite = i_dblPfeilBreite;
				item.PRO_dblDicke = ((Math.Abs(i_dblPfeilBreite) < 1E-10) ? 0.0 : (1.0 / i_dblPfeilBreite));
				item.PRO_fdcFarbe = base.PRO_fdcFarbe;
			}
			PRO_enuRichtungsPfeile = list;
		}
	}
}
