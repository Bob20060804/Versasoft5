using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_EllipsenGrafik : EDC_RechteckBasisObjekt
	{
		public EDC_EllipsenGrafik(double i_dblLinks, double i_dblOben, double i_dblRechts, double i_dblUnten, double i_dblStrichstaerke, Color i_objFarbe, double i_dblSkalierung)
		{
			m_dblStrichStaerke = i_dblStrichstaerke;
			m_fdcGrafikFarbe = i_objFarbe;
			m_dblSkalierung = i_dblSkalierung;
			base.PRO_fdcStartPunkt = new Point(i_dblLinks, i_dblUnten);
			base.PRO_fdcEndPunkt = new Point(i_dblRechts, i_dblOben);
		}

		public EDC_EllipsenGrafik()
			: this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			Rect pRO_fdcRechteck = base.PRO_fdcRechteck;
			Point center = new Point((pRO_fdcRechteck.Left + pRO_fdcRechteck.Right) / 2.0, (pRO_fdcRechteck.Top + pRO_fdcRechteck.Bottom) / 2.0);
			double radiusX = (pRO_fdcRechteck.Right - pRO_fdcRechteck.Left) / 2.0;
			double radiusY = (pRO_fdcRechteck.Bottom - pRO_fdcRechteck.Top) / 2.0;
			i_fdcDrawingContext.DrawEllipse(null, new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), center, radiusX, radiusY);
			base.Draw(i_fdcDrawingContext);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			if (base.PRO_blnIstSelektiert)
			{
				return base.PRO_fdcRechteck.Contains(i_fdcPunkt);
			}
			EllipseGeometry ellipseGeometry = new EllipseGeometry(base.PRO_fdcRechteck);
			if (!ellipseGeometry.FillContains(i_fdcPunkt))
			{
				return ellipseGeometry.StrokeContains(new Pen(Brushes.Black, base.PRO_dblAktuelleStrichStaerke), i_fdcPunkt);
			}
			return true;
		}

		public override bool FUN_blnSchneidenSich(Rect i_fdcRechteck)
		{
			RectangleGeometry geometry = new RectangleGeometry(i_fdcRechteck);
			EllipseGeometry geometry2 = new EllipseGeometry(base.PRO_fdcRechteck);
			return !Geometry.Combine(geometry, geometry2, GeometryCombineMode.Intersect, null).IsEmpty();
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_EllipsenEigenschaften(this);
		}
	}
}
