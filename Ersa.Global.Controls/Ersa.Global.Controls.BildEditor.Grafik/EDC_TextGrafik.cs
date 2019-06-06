using Ersa.Global.Controls.BildEditor.Eigenschaften;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public class EDC_TextGrafik : EDC_RechteckBasisObjekt
	{
		private FormattedText m_fdcFormatierterText;

		private string m_strText;

		private string m_strFontFamilyName;

		private FontStyle m_fdcFontStyle;

		private FontWeight m_fdcFontWeight;

		private FontStretch m_fdcFontStretch;

		private double m_dblFontSize;

		public string PRO_strText
		{
			get
			{
				return m_strText;
			}
			set
			{
				m_strText = value;
				SUB_Refresh();
			}
		}

		public string PRO_strFontFamilyName
		{
			get
			{
				return m_strFontFamilyName;
			}
			set
			{
				m_strFontFamilyName = value;
				SUB_Refresh();
			}
		}

		public FontStyle PRO_fdcFontStyle
		{
			get
			{
				return m_fdcFontStyle;
			}
			set
			{
				m_fdcFontStyle = value;
				SUB_Refresh();
			}
		}

		public FontWeight PRO_fdcFontWeight
		{
			get
			{
				return m_fdcFontWeight;
			}
			set
			{
				m_fdcFontWeight = value;
				SUB_Refresh();
			}
		}

		public FontStretch PRO_fdcFontStretch
		{
			get
			{
				return m_fdcFontStretch;
			}
			set
			{
				m_fdcFontStretch = value;
				SUB_Refresh();
			}
		}

		public double PRO_dblFontSize
		{
			get
			{
				return m_dblFontSize;
			}
			set
			{
				m_dblFontSize = value;
				SUB_Refresh();
			}
		}

		public EDC_TextGrafik(string i_strText, double i_dblLinks, double i_dblOben, double i_dblRechts, double i_dblUnten, Color i_objFarbe, double i_dblFontSize, string i_strFontFamilyName, FontStyle i_fdcFontStyle, FontWeight i_fdcFontWeight, FontStretch i_fdcFontStretch, double i_dblSkalierung)
		{
			m_dblStrichStaerke = 2.0;
			m_fdcGrafikFarbe = i_objFarbe;
			m_dblSkalierung = i_dblSkalierung;
			base.PRO_fdcStartPunkt = new Point(i_dblLinks, i_dblUnten);
			base.PRO_fdcEndPunkt = new Point(i_dblRechts, i_dblOben);
			m_strText = i_strText;
			m_dblFontSize = i_dblFontSize;
			m_strFontFamilyName = i_strFontFamilyName;
			m_fdcFontStyle = i_fdcFontStyle;
			m_fdcFontWeight = i_fdcFontWeight;
			m_fdcFontStretch = i_fdcFontStretch;
		}

		public EDC_TextGrafik()
			: this("Unbekannt", 0.0, 0.0, 0.0, 0.0, Colors.Black, 12.0, "Arial", FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 1.0)
		{
		}

		public override void Draw(DrawingContext i_fdcDrawingContext)
		{
			if (i_fdcDrawingContext == null)
			{
				throw new ArgumentNullException("i_fdcDrawingContext");
			}
			SUB_ErstelleFormatiertenText();
			Rect pRO_fdcRechteck = base.PRO_fdcRechteck;
			i_fdcDrawingContext.PushClip(new RectangleGeometry(pRO_fdcRechteck));
			i_fdcDrawingContext.DrawText(m_fdcFormatierterText, new Point(pRO_fdcRechteck.Left, pRO_fdcRechteck.Top));
			i_fdcDrawingContext.Pop();
			if (base.PRO_blnIstSelektiert)
			{
				i_fdcDrawingContext.DrawRectangle(null, new Pen(new SolidColorBrush(base.PRO_fdcGrafikFarbe), base.PRO_dblAktuelleStrichStaerke), pRO_fdcRechteck);
			}
			base.Draw(i_fdcDrawingContext);
		}

		public void SUB_AktualisiereDasRechteck()
		{
			SUB_Refresh();
		}

		public override bool FUN_blnEnthaeltPunkt(Point i_fdcPunkt)
		{
			return base.PRO_fdcRechteck.Contains(i_fdcPunkt);
		}

		public override EDC_GrafikEigenschaften FUN_edcSerialisiereObjekt()
		{
			return new EDC_TextEigenschaften(this);
		}

		private void SUB_ErstelleFormatiertenText()
		{
			if (string.IsNullOrEmpty(PRO_strFontFamilyName))
			{
				PRO_strFontFamilyName = "Arial";
			}
			if (PRO_strText == null)
			{
				PRO_strText = string.Empty;
			}
			if (PRO_dblFontSize <= 0.0)
			{
				PRO_dblFontSize = 20.0;
			}
			Typeface typeface = new Typeface(new FontFamily(PRO_strFontFamilyName), PRO_fdcFontStyle, PRO_fdcFontWeight, PRO_fdcFontStretch);
			SolidColorBrush foreground = new SolidColorBrush(base.PRO_fdcGrafikFarbe);
			m_fdcFormatierterText = new FormattedText(PRO_strText, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, PRO_dblFontSize, foreground)
			{
				MaxTextWidth = base.PRO_fdcRechteck.Width
			};
		}
	}
}
