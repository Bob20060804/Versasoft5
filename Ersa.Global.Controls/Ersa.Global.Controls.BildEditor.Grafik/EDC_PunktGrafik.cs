using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_PunktGrafik : EDC_GrafikBasisObjekt
	{
		public override int PRO_i32HandleAnzahl => 1;

		public Rect PRO_fdcRechteck
		{
			get
			{
				Point point = new Point(base.PRO_fdcPosition.X - m_dblStrichStaerke, base.PRO_fdcPosition.Y - m_dblStrichStaerke);
				Point point2 = new Point(base.PRO_fdcPosition.X + m_dblStrichStaerke, base.PRO_fdcPosition.Y + m_dblStrichStaerke);
				return new Rect(point, point2);
			}
		}

		public EDC_PunktGrafik(Point i_fdcPosition, double i_dblRadius, Color i_fdcGrafikFarbe, double i_dblSkalierung)
		{
			m_dblStrichStaerke = i_dblRadius;
			m_fdcGrafikFarbe = i_fdcGrafikFarbe;
			m_dblSkalierung = i_dblSkalierung;
			base.PRO_fdcPosition = i_fdcPosition;
		}

		public EDC_PunktGrafik()
			: this(new Point(0.0, 0.0), 5.0, Colors.Black, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			i_fdcDrawingContext.DrawEllipse(new SolidColorBrush(base.PRO_fdcGrafikFarbe), new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), base.PRO_fdcPosition, m_dblStrichStaerke, m_dblStrichStaerke);
			base.Draw(i_fdcDrawingContext);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			if (base.PRO_blnIstSelektiert)
			{
				return PRO_fdcRechteck.Contains(i_fdcPunkt);
			}
			EllipseGeometry ellipseGeometry = new EllipseGeometry(PRO_fdcRechteck);
			if (!ellipseGeometry.FillContains(i_fdcPunkt))
			{
				return ellipseGeometry.StrokeContains(new Pen(Brushes.Black, base.PRO_dblAktuelleStrichStaerke), i_fdcPunkt);
			}
			return true;
		}

		public override bool FUN_blnSchneidenSich(Rect i_fdcRechteck)
		{
			RectangleGeometry geometry = new RectangleGeometry(i_fdcRechteck);
			EllipseGeometry geometry2 = new EllipseGeometry(PRO_fdcRechteck);
			return !Geometry.Combine(geometry, geometry2, GeometryCombineMode.Intersect, null).IsEmpty();
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_PunktEigenschaften(this);
		}

		public override Point FUN_fdcGetHandle(int i_i32HandleNummer)
		{
			return base.PRO_fdcPosition;
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

		public override void SUB_BewegeObjekt(double i_dblDeltax, double i_dblDeltay)
		{
			Point pRO_fdcPosition = base.PRO_fdcPosition;
			pRO_fdcPosition.X += i_dblDeltax;
			pRO_fdcPosition.Y += i_dblDeltay;
			base.PRO_fdcPosition = pRO_fdcPosition;
			SUB_Refresh();
		}

		public override void SUB_BewegeHandleZu(Point i_fdcPunkt, int i_i32HandleNummer)
		{
			if (i_i32HandleNummer == 1)
			{
				base.PRO_fdcPosition = i_fdcPunkt;
			}
			SUB_Refresh();
		}

		public override Cursor FUN_fdcHoleCursor(int i_i32HandleNummer)
		{
			if (i_i32HandleNummer == 1)
			{
				return Cursors.SizeAll;
			}
			return EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
		}
	}
}
