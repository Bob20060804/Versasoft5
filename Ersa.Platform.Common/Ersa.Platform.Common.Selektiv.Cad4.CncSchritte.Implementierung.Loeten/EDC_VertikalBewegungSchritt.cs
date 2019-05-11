using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Loeten
{
	[Serializable]
	public class EDC_VertikalBewegungSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblZielwertZ;

		private double m_dblGeschwindigkeit;

		public double PRO_dblZielwertZ
		{
			get
			{
				return m_dblZielwertZ;
			}
			set
			{
				SetProperty(ref m_dblZielwertZ, value, "PRO_dblZielwertZ");
			}
		}

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

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.VertikalBewegung;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_VertikalBewegungSchritt eDC_VertikalBewegungSchritt = i_edcVergleichsSchritt as EDC_VertikalBewegungSchritt;
			if (eDC_VertikalBewegungSchritt != null && Math.Abs(PRO_dblZielwertZ - eDC_VertikalBewegungSchritt.PRO_dblZielwertZ) < 4.94065645841247E-324)
			{
				return Math.Abs(PRO_dblGeschwindigkeit - eDC_VertikalBewegungSchritt.PRO_dblGeschwindigkeit) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Vertikalbewegung Z: {PRO_dblZielwertZ} V: {PRO_dblGeschwindigkeit}";
		}
	}
}
