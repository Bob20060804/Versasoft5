using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_LinienGrafik : EDC_GrafikBasisObjekt
	{
		public override int PRO_i32HandleAnzahl => 2;

		public EDC_LinienGrafik(Point i_fdcStartpunkt, Point i_fdcEndpunkt, double i_dblStrichStaerke, Color i_fdcGrafikFarbe, double i_dblSkalierung)
		{
			m_dblStrichStaerke = i_dblStrichStaerke;
			m_fdcGrafikFarbe = i_fdcGrafikFarbe;
			m_dblSkalierung = i_dblSkalierung;
			base.PRO_fdcStartPunkt = i_fdcStartpunkt;
			base.PRO_fdcEndPunkt = i_fdcEndpunkt;
		}

		public EDC_LinienGrafik()
			: this(new Point(0.0, 0.0), new Point(100.0, 100.0), 1.0, Colors.Black, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			Matrix matrix = default(Matrix);
			Vector vector = base.PRO_fdcEndPunkt - base.PRO_fdcStartPunkt;
			vector.Normalize();
			vector *= 20.0;
			matrix.Rotate(165.0);
			Point point = base.PRO_fdcEndPunkt + vector * matrix;
			matrix.Rotate(30.0);
			Point point2 = base.PRO_fdcEndPunkt + vector * matrix;
			double num = base.PRO_dblAktuelleStrichStaerke * 1.25;
			i_fdcDrawingContext.DrawEllipse(new SolidColorBrush(base.PRO_fdcGrafikFarbe), new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), base.PRO_fdcStartPunkt, num, num);
			i_fdcDrawingContext.DrawEllipse(new SolidColorBrush(base.PRO_fdcGrafikFarbe), new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), base.PRO_fdcEndPunkt, num, num);
			Pen pen = new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke);
			i_fdcDrawingContext.DrawLine(pen, base.PRO_fdcStartPunkt, base.PRO_fdcEndPunkt);
			Pen pen2 = new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke / 2.0);
			i_fdcDrawingContext.DrawLine(pen2, base.PRO_fdcEndPunkt, point);
			i_fdcDrawingContext.DrawLine(pen2, base.PRO_fdcEndPunkt, point2);
			i_fdcDrawingContext.DrawLine(pen2, point, point2);
			base.Draw(i_fdcDrawingContext);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			return new LineGeometry(base.PRO_fdcStartPunkt, base.PRO_fdcEndPunkt).StrokeContains(new Pen(Brushes.Black, base.PRO_dblLinienTrefferTestBreite), i_fdcPunkt);
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_LinienEigenschaften(this);
		}

		public override Point FUN_fdcGetHandle(int i_i32HandleNummer)
		{
			if (i_i32HandleNummer == 1)
			{
				return base.PRO_fdcStartPunkt;
			}
			return base.PRO_fdcEndPunkt;
		}

		public override int FUN_i32MacheTrefferTest(Point i_fdcPunkt)
		{
			if (base.PRO_blnIstSelektiert)
			{
				for (int i = 1; i <= PRO_i32HandleAnzahl; i++)
				{
					if (FUN_fdcHoleHandleRechteck(i).Contains(i_fdcPunkt))
					{
						if (Point.Equals(base.PRO_fdcStartPunkt, base.PRO_fdcEndPunkt))
						{
							return 0;
						}
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
			RectangleGeometry geometry = new RectangleGeometry(i_fdcRechteck);
			PathGeometry widenedPathGeometry = new LineGeometry(base.PRO_fdcStartPunkt, base.PRO_fdcEndPunkt).GetWidenedPathGeometry(new Pen(Brushes.Black, base.PRO_dblLinienTrefferTestBreite));
			return !Geometry.Combine(geometry, widenedPathGeometry, GeometryCombineMode.Intersect, null).IsEmpty();
		}

		public override Cursor FUN_fdcHoleCursor(int i_i32HandleNummer)
		{
			if ((uint)(i_i32HandleNummer - 1) <= 1u)
			{
				return Cursors.SizeAll;
			}
			return EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
		}

		public override void SUB_BewegeHandleZu(Point i_fdcPunkt, int i_i32HandleNummer)
		{
			if (i_i32HandleNummer == 1)
			{
				base.PRO_fdcStartPunkt = i_fdcPunkt;
			}
			else
			{
				base.PRO_fdcEndPunkt = i_fdcPunkt;
			}
			SUB_Refresh();
		}

		public override void SUB_BewegeObjekt(double i_dblDeltax, double i_dblDeltay)
		{
			Point pRO_fdcStartPunkt = base.PRO_fdcStartPunkt;
			pRO_fdcStartPunkt.X += i_dblDeltax;
			pRO_fdcStartPunkt.Y += i_dblDeltay;
			base.PRO_fdcStartPunkt = pRO_fdcStartPunkt;
			pRO_fdcStartPunkt = base.PRO_fdcEndPunkt;
			pRO_fdcStartPunkt.X += i_dblDeltax;
			pRO_fdcStartPunkt.Y += i_dblDeltay;
			base.PRO_fdcEndPunkt = pRO_fdcStartPunkt;
			SUB_Refresh();
		}
	}
}
