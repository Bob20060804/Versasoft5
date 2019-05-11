using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Uebergeordnet
{
	[Serializable]
	public class EDC_WartenSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblZeit;

		public double PRO_dblZeit
		{
			get
			{
				return m_dblZeit;
			}
			set
			{
				SetProperty(ref m_dblZeit, value, "PRO_dblZeit");
			}
		}

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.Warten;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_WartenSchritt eDC_WartenSchritt = i_edcVergleichsSchritt as EDC_WartenSchritt;
			if (eDC_WartenSchritt != null)
			{
				return Math.Abs(PRO_dblZeit - eDC_WartenSchritt.PRO_dblZeit) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Warte {PRO_dblZeit}s";
		}
	}
}
