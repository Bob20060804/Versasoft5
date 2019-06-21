using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_RichtungspfeilDefinition
	{
		public double PRO_dblX
		{
			get;
		}

		public double PRO_dblY
		{
			get;
		}

		public double PRO_dblDrehung
		{
			get;
		}

		public Brush PRO_fdcFarbe
		{
			get;
			set;
		}

		public double PRO_dblDicke
		{
			get;
			set;
		}

		public double PRO_dblBreite
		{
			get;
			set;
		}

		public EDC_RichtungspfeilDefinition(Point i_sttPosition, double i_dblDrehung)
		{
			PRO_dblX = i_sttPosition.X;
			PRO_dblY = i_sttPosition.Y;
			PRO_dblDrehung = i_dblDrehung;
		}
	}
}
