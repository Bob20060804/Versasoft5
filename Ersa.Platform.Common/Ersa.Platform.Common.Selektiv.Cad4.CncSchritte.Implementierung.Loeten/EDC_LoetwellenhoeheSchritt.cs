using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Loeten
{
	[Serializable]
	public class EDC_LoetwellenhoeheSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblNeueHoehe;

		private double m_dblZeit;

		public double PRO_dblNeueHoehe
		{
			get
			{
				return m_dblNeueHoehe;
			}
			set
			{
				SetProperty(ref m_dblNeueHoehe, value, "PRO_dblNeueHoehe");
			}
		}

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

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.LoetwellenHoehe;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_LoetwellenhoeheSchritt eDC_LoetwellenhoeheSchritt = i_edcVergleichsSchritt as EDC_LoetwellenhoeheSchritt;
			if (eDC_LoetwellenhoeheSchritt != null && Math.Abs(PRO_dblNeueHoehe - eDC_LoetwellenhoeheSchritt.PRO_dblNeueHoehe) < 4.94065645841247E-324)
			{
				return Math.Abs(PRO_dblZeit - eDC_LoetwellenhoeheSchritt.PRO_dblZeit) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Lötwellenhöhe auf {PRO_dblNeueHoehe} in {PRO_dblZeit}s";
		}
	}
}
