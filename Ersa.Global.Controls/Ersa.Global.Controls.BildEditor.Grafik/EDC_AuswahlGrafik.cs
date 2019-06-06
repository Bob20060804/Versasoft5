using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_AuswahlGrafik : EDC_RechteckBasisObjekt
	{
		private const int mC_i32DashCount = 5;

		private const double mC_dblStrichstaerke = 1.0;

		public EDC_AuswahlGrafik(double i_dblLinks, double i_dblOben, double i_dblRechts, double i_dblUnten, double i_dblSkalierung)
		{
			m_dblSkalierung = i_dblSkalierung;
			m_dblStrichStaerke = 1.0;
			m_fdcGrafikFarbe = Colors.Black;
			base.PRO_fdcStartPunkt = new Point(i_dblLinks, i_dblUnten);
			base.PRO_fdcEndPunkt = new Point(i_dblRechts, i_dblOben);
		}

		public EDC_AuswahlGrafik()
			: this(0.0, 0.0, 100.0, 100.0, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			i_fdcDrawingContext.DrawRectangle(null, new Pen(Brushes.White, base.PRO_dblStrichStaerke), base.PRO_fdcRechteck);
			DashStyle dashStyle = new DashStyle();
			dashStyle.Dashes.Add(5.0);
			Pen pen = new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblStrichStaerke)
			{
				DashStyle = dashStyle
			};
			i_fdcDrawingContext.DrawRectangle(null, pen, base.PRO_fdcRechteck);
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			return base.PRO_fdcRechteck.Contains(i_fdcPunkt);
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return null;
		}
	}
}
