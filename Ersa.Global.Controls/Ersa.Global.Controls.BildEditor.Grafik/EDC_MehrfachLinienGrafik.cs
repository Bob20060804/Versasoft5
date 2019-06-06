using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_MehrfachLinienGrafik : EDC_GrafikBasisObjekt
	{
		private const string mC_strCursorUri = "pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/MehrfachLinie.cur";

		private static Cursor ms_fdcHandleCursor;

		private PathGeometry m_fdcPathGeometry;

		private Point[] ma_fdcPunkte;

		public override int PRO_i32HandleAnzahl => m_fdcPathGeometry.Figures[0].Segments.Count + 1;

		static EDC_MehrfachLinienGrafik()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/MehrfachLinie.cur"));
			ms_fdcHandleCursor = ((resourceStream != null) ? new Cursor(resourceStream.Stream) : Cursors.Arrow);
		}

		public EDC_MehrfachLinienGrafik(Point[] ia_fdcPunkte, double i_dblStrichstaerke, Color i_dblGrafikFarbe, double i_dblSkalierung)
		{
			SUB_ErstelleLinien(ia_fdcPunkte, i_dblStrichstaerke, i_dblGrafikFarbe, i_dblSkalierung);
		}

		public EDC_MehrfachLinienGrafik()
			: this(new Point[2]
			{
				new Point(0.0, 0.0),
				new Point(100.0, 100.0)
			}, 1.0, Colors.Black, 1.0)
		{
		}

		public void SUB_FuegePunktHinzu(Point i_fdcPunkt)
		{
			LineSegment value = new LineSegment(i_fdcPunkt, isStroked: true)
			{
				IsSmoothJoin = true
			};
			m_fdcPathGeometry.Figures[0].Segments.Add(value);
			SUB_ErstellePunkte();
		}

		public Point[] FUN_fdcHolePunkte()
		{
			return ma_fdcPunkte;
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			i_fdcDrawingContext.DrawGeometry(null, new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), m_fdcPathGeometry);
			base.Draw(i_fdcDrawingContext);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			if (!m_fdcPathGeometry.FillContains(i_fdcPunkt))
			{
				return m_fdcPathGeometry.StrokeContains(new Pen(Brushes.Black, base.PRO_dblLinienTrefferTestBreite), i_fdcPunkt);
			}
			return true;
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_MehrfachLinienEigensachften(this);
		}

		public override Point FUN_fdcGetHandle(int i_i32HandleNummer)
		{
			if (i_i32HandleNummer < 1)
			{
				i_i32HandleNummer = 1;
			}
			if (i_i32HandleNummer > ma_fdcPunkte.Length)
			{
				i_i32HandleNummer = ma_fdcPunkte.Length;
			}
			return ma_fdcPunkte[i_i32HandleNummer - 1];
		}

		public override Cursor FUN_fdcHoleCursor(int i_i32HandleNummer)
		{
			return ms_fdcHandleCursor;
		}

		public override void SUB_BewegeHandleZu(Point i_fdcPunkt, int i_i32HandleNummer)
		{
			if (i_i32HandleNummer == 1)
			{
				m_fdcPathGeometry.Figures[0].StartPoint = i_fdcPunkt;
			}
			else
			{
				((LineSegment)m_fdcPathGeometry.Figures[0].Segments[i_i32HandleNummer - 2]).Point = i_fdcPunkt;
			}
			SUB_ErstellePunkte();
			SUB_Refresh();
		}

		public override void SUB_BewegeObjekt(double i_dblDeltax, double i_dblDeltay)
		{
			for (int i = 0; i < ma_fdcPunkte.Length; i++)
			{
				ma_fdcPunkte[i].X += i_dblDeltax;
				ma_fdcPunkte[i].Y += i_dblDeltay;
			}
			SUB_ErstelleObjektAusPunkten(ref ma_fdcPunkte);
			SUB_Refresh();
		}

		public override int FUN_i32MacheTrefferTest(Point i_fdcPunkt)
		{
			if (base.PRO_blnIstSelektiert)
			{
				for (int i = 1; i <= PRO_i32HandleAnzahl; i++)
				{
					if (FUN_fdcHoleHandleRechteck(i).Contains(i_fdcPunkt))
					{
						return i;
					}
				}
			}
			if (FUN_blnEnthaeltPunkt(i_fdcPunkt))
			{
				return 0;
			}
			return -1;
		}

		public override bool FUN_blnSchneidenSich(Rect i_fdcRechteck)
		{
			return !Geometry.Combine(new RectangleGeometry(i_fdcRechteck), m_fdcPathGeometry, GeometryCombineMode.Intersect, null).IsEmpty();
		}

		private void SUB_ErstellePunkte()
		{
			ma_fdcPunkte = new Point[m_fdcPathGeometry.Figures[0].Segments.Count + 1];
			ma_fdcPunkte[0] = m_fdcPathGeometry.Figures[0].StartPoint;
			for (int i = 0; i < m_fdcPathGeometry.Figures[0].Segments.Count; i++)
			{
				ma_fdcPunkte[i + 1] = ((LineSegment)m_fdcPathGeometry.Figures[0].Segments[i]).Point;
			}
		}

		private void SUB_ErstelleObjektAusPunkten(ref Point[] ia_fdcPunkte)
		{
			if (ia_fdcPunkte == null)
			{
				ia_fdcPunkte = new Point[2];
			}
			PathFigure pathFigure = new PathFigure();
			if (ia_fdcPunkte.Length >= 1)
			{
				pathFigure.StartPoint = ia_fdcPunkte[0];
			}
			for (int i = 1; i < ia_fdcPunkte.Length; i++)
			{
				LineSegment value = new LineSegment(ia_fdcPunkte[i], isStroked: true)
				{
					IsSmoothJoin = true
				};
				pathFigure.Segments.Add(value);
			}
			m_fdcPathGeometry = new PathGeometry();
			m_fdcPathGeometry.Figures.Add(pathFigure);
			SUB_ErstellePunkte();
		}

		private void SUB_ErstelleLinien(Point[] ia_fdcPunkte, double i_dblStrichstaerke, Color i_dblGrafikFarbe, double i_dblSkalierung)
		{
			SUB_ErstelleObjektAusPunkten(ref ia_fdcPunkte);
			m_dblStrichStaerke = i_dblStrichstaerke;
			m_fdcGrafikFarbe = i_dblGrafikFarbe;
			m_dblSkalierung = i_dblSkalierung;
		}
	}
}
