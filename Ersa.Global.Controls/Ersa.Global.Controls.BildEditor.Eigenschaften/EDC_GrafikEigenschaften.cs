using Ersa.Global.Controls.BildEditor.Grafik;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Ersa.Global.Controls.BildEditor.Eigenschaften
{
	public abstract class EDC_GrafikEigenschaften
	{
		public Point PRO_fdcStartPunkt
		{
			get;
			set;
		}

		public Point PRO_fdcEndPunkt
		{
			get;
			set;
		}

		public double PRO_dblLinks
		{
			get;
			set;
		}

		public double PRO_dblRechts
		{
			get;
			set;
		}

		public double PRO_dblOben
		{
			get;
			set;
		}

		public double PRO_dblUnten
		{
			get;
			set;
		}

		public double PRO_dblStrichStaerke
		{
			get;
			set;
		}

		public Color PRO_fdcStrichFarbe
		{
			get;
			set;
		}

		[XmlIgnore]
		internal int PRO_i32GrafikObjektId
		{
			get;
			set;
		}

		[XmlIgnore]
		internal bool PRO_blnIstSelektiert
		{
			get;
			set;
		}

		[XmlIgnore]
		internal double PRO_dblSkalierung
		{
			get;
			set;
		}

		public abstract EDC_GrafikBasisObjekt FUN_edcErstelleGrafikObjekt();
	}
}
