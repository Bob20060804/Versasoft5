using Ersa.Global.Common.Data.Cad;
using Ersa.Global.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public abstract class EDC_EditorElement : BindableBase
	{
		private bool m_blnSichtbar;

		private Brush m_fdcFarbe;

		private Brush m_fdcAuswahlFarbe;

		private Brush m_fdcFehlerFarbe;

		private Brush m_fdcFehlerAuswahlFarbe;

		private bool m_blnAuswaehlbar;

		private bool m_blnAusgewaehlt;

		private Point m_sttPosition;

		private double m_dblSkalierung;

		private object m_objDaten;

		private bool m_blnUebergehtSkalierung;

		private bool m_blnIstFehlerhaft;

		private int m_i32ZIndex;

		private string m_strToolTip;

		private Action<Point> m_delPositionGeaendertAktion;

		private ENUM_Blickrichtung? m_enmBlickrichtung;

		public bool PRO_blnSichtbar
		{
			get
			{
				return m_blnSichtbar;
			}
			set
			{
				SetProperty(ref m_blnSichtbar, value, "PRO_blnSichtbar");
			}
		}

		public Brush PRO_fdcFarbe
		{
			get
			{
				return m_fdcFarbe;
			}
			set
			{
				SetProperty(ref m_fdcFarbe, value, "PRO_fdcFarbe");
			}
		}

		public Brush PRO_fdcAuswahlFarbe
		{
			get
			{
				return m_fdcAuswahlFarbe;
			}
			set
			{
				SetProperty(ref m_fdcAuswahlFarbe, value, "PRO_fdcAuswahlFarbe");
			}
		}

		public Brush PRO_fdcFehlerFarbe
		{
			get
			{
				return m_fdcFehlerFarbe;
			}
			set
			{
				SetProperty(ref m_fdcFehlerFarbe, value, "PRO_fdcFehlerFarbe");
			}
		}

		public Brush PRO_fdcFehlerAuswahlFarbe
		{
			get
			{
				return m_fdcFehlerAuswahlFarbe;
			}
			set
			{
				SetProperty(ref m_fdcFehlerAuswahlFarbe, value, "PRO_fdcFehlerAuswahlFarbe");
			}
		}

		public bool PRO_blnAuswaehlbar
		{
			get
			{
				return m_blnAuswaehlbar;
			}
			set
			{
				SetProperty(ref m_blnAuswaehlbar, value, "PRO_blnAuswaehlbar");
			}
		}

		public bool PRO_blnAusgewaehlt
		{
			get
			{
				return m_blnAusgewaehlt;
			}
			set
			{
				if (!(!PRO_blnAuswaehlbar && value) && SetProperty(ref m_blnAusgewaehlt, value, "PRO_blnAusgewaehlt"))
				{
					SUB_OnAuswahlGeaendert(PRO_blnAusgewaehlt);
				}
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				return m_blnIstFehlerhaft;
			}
			set
			{
				SetProperty(ref m_blnIstFehlerhaft, value, "PRO_blnIstFehlerhaft");
			}
		}

		public string PRO_strToolTip
		{
			get
			{
				return m_strToolTip;
			}
			set
			{
				SetProperty(ref m_strToolTip, value, "PRO_strToolTip");
			}
		}

		public Point PRO_sttPosition
		{
			get
			{
				return PRO_sttTemporaerePosition ?? m_sttPosition;
			}
			set
			{
				if (SetProperty(ref m_sttPosition, value, "PRO_sttPosition"))
				{
					SUB_OnPositionGeaendert(PRO_sttPosition);
				}
			}
		}

		public Point? PRO_sttTemporaerePosition
		{
			get;
			private set;
		}

		public Point PRO_sttOriginalPosition => m_sttPosition;

		public bool PRO_blnUebergehtSkalierung
		{
			get
			{
				return m_blnUebergehtSkalierung;
			}
			set
			{
				if (SetProperty(ref m_blnUebergehtSkalierung, value, "PRO_blnUebergehtSkalierung"))
				{
					SUB_OnSkalierungGeaendert();
				}
			}
		}

		public int PRO_i32ZIndex
		{
			get
			{
				return m_i32ZIndex;
			}
			set
			{
				SetProperty(ref m_i32ZIndex, value, "PRO_i32ZIndex");
			}
		}

		public ENUM_Blickrichtung? PRO_enmBlickrichtung
		{
			get
			{
				return m_enmBlickrichtung;
			}
			set
			{
				SetProperty(ref m_enmBlickrichtung, value, "PRO_enmBlickrichtung");
			}
		}

		public object PRO_objDaten
		{
			get
			{
				return m_objDaten;
			}
			set
			{
				SetProperty(ref m_objDaten, value, "PRO_objDaten");
			}
		}

		protected double PRO_dblSkalierung
		{
			get
			{
				return m_dblSkalierung;
			}
			private set
			{
				if (Math.Abs(m_dblSkalierung - value) > 1E-10)
				{
					m_dblSkalierung = value;
					SUB_OnSkalierungGeaendert();
				}
			}
		}

		protected EDC_EditorElement()
		{
			m_blnSichtbar = true;
			m_fdcFarbe = EDC_EditorKonstanten.PRO_fdcFarbeDunkel;
			m_fdcAuswahlFarbe = EDC_EditorKonstanten.PRO_fdcAuswahlFarbeDunkel;
			m_fdcFehlerFarbe = EDC_EditorKonstanten.PRO_fdcFehlerFarbeDunkel;
			m_fdcFehlerAuswahlFarbe = EDC_EditorKonstanten.PRO_fdcFehlerAuswahlFarbeDunkel;
			m_blnAuswaehlbar = true;
			m_dblSkalierung = 1.0;
		}

		public virtual bool FUN_blnIstInBereich(Rect i_sttBereich)
		{
			return false;
		}

		public void SUB_SetzeTemporaerePosition(Point i_sttPosition)
		{
			PRO_sttTemporaerePosition = i_sttPosition;
			RaisePropertyChanged("PRO_sttPosition");
		}

		public void SUB_UebernehmeTemporaerePosition()
		{
			if (PRO_sttTemporaerePosition.HasValue)
			{
				Point value = PRO_sttTemporaerePosition.Value;
				PRO_sttTemporaerePosition = null;
				PRO_sttPosition = value;
			}
		}

		public void SUB_VerwerfeTemporaerePosition()
		{
			PRO_sttTemporaerePosition = null;
			RaisePropertyChanged("PRO_sttPosition");
		}

		public void SUB_PositionshandlerRegistrieren(Action<Point> i_delAktion)
		{
			m_delPositionGeaendertAktion = i_delAktion;
		}

		internal void SUB_SetzeSkalierung(double i_dblSkalierung)
		{
			PRO_dblSkalierung = i_dblSkalierung;
		}

		protected virtual void SUB_OnAuswahlGeaendert(bool i_blnAusgewaehlt)
		{
		}

		protected virtual void SUB_OnPositionGeaendert(Point i_sttPosition)
		{
			m_delPositionGeaendertAktion?.Invoke(i_sttPosition);
		}

		protected virtual void SUB_OnSkalierungGeaendert()
		{
		}
	}
}
