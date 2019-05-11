using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Uebergeordnet
{
	[Serializable]
	public class EDC_WeiterfahrenSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblStrecke;

		public double PRO_dblStrecke
		{
			get
			{
				return m_dblStrecke;
			}
			set
			{
				SetProperty(ref m_dblStrecke, value, "PRO_dblStrecke");
			}
		}

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.Weiterfahren;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_WeiterfahrenSchritt eDC_WeiterfahrenSchritt = i_edcVergleichsSchritt as EDC_WeiterfahrenSchritt;
			if (eDC_WeiterfahrenSchritt != null)
			{
				return Math.Abs(PRO_dblStrecke - eDC_WeiterfahrenSchritt.PRO_dblStrecke) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Weiterfahren um {PRO_dblStrecke}";
		}
	}
}
