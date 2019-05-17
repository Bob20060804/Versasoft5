using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_RechteckGrafik : EDC_RechteckBasisObjekt
	{
		public EDC_RechteckGrafik(double i_dblLinks, double i_dblOben, double i_dblRechts, double i_dblUnten, double i_dblStrichstaerke, Color i_objFarbe, double i_dblSkalierung)
		{
			m_dblStrichStaerke = i_dblStrichstaerke;
			m_fdcGrafikFarbe = i_objFarbe;
			m_dblSkalierung = i_dblSkalierung;
			base.PRO_fdcStartPunkt = new Point(i_dblLinks, i_dblUnten);
			base.PRO_fdcEndPunkt = new Point(i_dblRechts, i_dblOben);
		}

		public EDC_RechteckGrafik()
			: this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			i_fdcDrawingContext.DrawRectangle(null, new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), base.PRO_fdcRechteck);
			base.Draw(i_fdcDrawingContext);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			return base.PRO_fdcRechteck.Contains(i_fdcPunkt);
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_RechteckEigenschaften(this);
		}
	}
}
