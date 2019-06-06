using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public abstract class EDC_GrafikBasisObjekt : DrawingVisual
	{
		protected double m_dblStrichStaerke;

		protected Color m_fdcGrafikFarbe;

		protected double m_dblSkalierung;

		private static readonly SolidColorBrush ms_fdcExternerBrush = new SolidColorBrush(EDC_BildEditorKonstanten.ms_fdcDefaultTrackerFarbeExtern);

		private static readonly SolidColorBrush ms_fdcMittlererBrush = new SolidColorBrush(EDC_BildEditorKonstanten.ms_fdcDefaultTrackerFarbeMitte);

		private static readonly SolidColorBrush ms_fdcInternerBrush = new SolidColorBrush(EDC_BildEditorKonstanten.ms_fdcDefaultTrackerFarbeIntern);

		private bool m_blnIstSichtbar = true;

		private Point m_fdcPosition;

		private Point m_fdcStartPunkt;

		private Point m_fdcEndPunkt;

		private bool m_blnIsteSelektiert;

		public int PRO_i32ObjektId
		{
			get;
			set;
		}

		public Point PRO_fdcPosition
		{
			get
			{
				return m_fdcPosition;
			}
			set
			{
				m_fdcPosition = value;
				SUB_Refresh();
			}
		}

		public Point PRO_fdcStartPunkt
		{
			get
			{
				return m_fdcStartPunkt;
			}
			set
			{
				m_fdcStartPunkt = value;
				SUB_Refresh();
			}
		}

		public Point PRO_fdcEndPunkt
		{
			get
			{
				return m_fdcEndPunkt;
			}
			set
			{
				m_fdcEndPunkt = value;
				SUB_Refresh();
			}
		}

		public bool PRO_blnIstSelektiert
		{
			get
			{
				return m_blnIsteSelektiert;
			}
			set
			{
				m_blnIsteSelektiert = value;
				SUB_Refresh();
			}
		}

		public double PRO_dblStrichStaerke
		{
			get
			{
				return m_dblStrichStaerke;
			}
			set
			{
				m_dblStrichStaerke = value;
				SUB_Refresh();
			}
		}

		public Color PRO_fdcGrafikFarbe
		{
			get
			{
				return m_fdcGrafikFarbe;
			}
			set
			{
				m_fdcGrafikFarbe = value;
				SUB_Refresh();
			}
		}

		public double PRO_dblSkalierung
		{
			get
			{
				return m_dblSkalierung;
			}
			set
			{
				m_dblSkalierung = value;
				SUB_Refresh();
			}
		}

		public double PRO_dblAktuelleStrichStaerke
		{
			get
			{
				if (!(m_dblSkalierung <= 0.0))
				{
					return m_dblStrichStaerke / m_dblSkalierung;
				}
				return m_dblStrichStaerke;
			}
		}

		public double PRO_dblLinienTrefferTestBreite => Math.Max(15.0, PRO_dblAktuelleStrichStaerke);

		public bool PRO_blnIstSichtbar
		{
			get
			{
				return m_blnIstSichtbar;
			}
			set
			{
				m_blnIstSichtbar = value;
				base.VisualOpacity = (m_blnIstSichtbar ? 1 : 0);
			}
		}

		public abstract int PRO_i32HandleAnzahl
		{
			get;
		}

		protected EDC_GrafikBasisObjekt()
		{
			PRO_i32ObjektId = GetHashCode();
			PRO_blnIstSichtbar = true;
		}

		public abstract bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt);

		public abstract EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt();

		public abstract Point FUN_fdcGetHandle(int i_i32HandleNummer);

		public abstract int FUN_i32MacheTrefferTest(Point i_fdcPunkt);

		public abstract bool FUN_blnSchneidenSich(Rect i_fdcRechteck);

		public abstract void SUB_BewegeObjekt(double i_dblDeltax, double i_dblDeltay);

		public abstract void SUB_BewegeHandleZu(Point i_fdcPunkt, int i_i32HandleNummer);

		public abstract Cursor FUN_fdcHoleCursor(int i_i32HandleNummer);

		public virtual void SUB_Normalisiere()
		{
		}

		public virtual void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (PRO_blnIstSelektiert)
			{
				DrawTracker(i_fdcDrawingContext);
			}
		}

		public virtual void DrawTracker(DrawingContext i_fdcDrawingContext)
		{
			for (int i = 1; i <= PRO_i32HandleAnzahl; i++)
			{
				SUB_DrawTrackerRechteck(i_fdcDrawingContext, FUN_fdcHoleHandleRechteck(i));
			}
		}

		public void SUB_Refresh()
		{
			using (DrawingContext i_fdcDrawingContext = RenderOpen())
			{
				Draw(i_fdcDrawingContext);
			}
		}

		public Rect FUN_fdcHoleHandleRechteck(int i_i32HandleNummer)
		{
			Point point = FUN_fdcGetHandle(i_i32HandleNummer);
			double num = Math.Max(10.0 / m_dblSkalierung, PRO_dblAktuelleStrichStaerke * 1.2);
			return new Rect(point.X - num / 2.0, point.Y - num / 2.0, num, num);
		}

		private static void SUB_DrawTrackerRechteck(DrawingContext i_fdcDrawingContext, Rect i_fdcRechteck)
		{
			i_fdcDrawingContext.DrawRectangle(ms_fdcExternerBrush, null, i_fdcRechteck);
			i_fdcDrawingContext.DrawRectangle(rectangle: new Rect(i_fdcRechteck.Left + i_fdcRechteck.Width / 8.0, i_fdcRechteck.Top + i_fdcRechteck.Height / 8.0, i_fdcRechteck.Width * 6.0 / 8.0, i_fdcRechteck.Height * 6.0 / 8.0), brush: ms_fdcMittlererBrush, pen: null);
			i_fdcDrawingContext.DrawRectangle(rectangle: new Rect(i_fdcRechteck.Left + i_fdcRechteck.Width / 4.0, i_fdcRechteck.Top + i_fdcRechteck.Height / 4.0, i_fdcRechteck.Width / 2.0, i_fdcRechteck.Height / 2.0), brush: ms_fdcInternerBrush, pen: null);
		}
	}
}
