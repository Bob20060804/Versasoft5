using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Uebergeordnet
{
	[Serializable]
	public class EDC_HorizontalBewegungSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblGeschwindigkeit;

		public double PRO_dblGeschwindigkeit
		{
			get
			{
				return m_dblGeschwindigkeit;
			}
			set
			{
				SetProperty(ref m_dblGeschwindigkeit, value, "PRO_dblGeschwindigkeit");
			}
		}

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.HorizontalBewegung;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_HorizontalBewegungSchritt eDC_HorizontalBewegungSchritt = i_edcVergleichsSchritt as EDC_HorizontalBewegungSchritt;
			if (eDC_HorizontalBewegungSchritt != null)
			{
				return Math.Abs(PRO_dblGeschwindigkeit - eDC_HorizontalBewegungSchritt.PRO_dblGeschwindigkeit) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Horizontalbewegung V: {PRO_dblGeschwindigkeit}";
		}
	}
}
