using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Fluxer
{
	[Serializable]
	public class EDC_FluxerDosisSchritt : EDC_AbstractCncSchritt
	{
		private double m_dblNeueDosis;

		public double PRO_dblNeueDosis
		{
			get
			{
				return m_dblNeueDosis;
			}
			set
			{
				SetProperty(ref m_dblNeueDosis, value, "PRO_dblNeueDosis");
			}
		}

		public override ENUM_AbstractCncSchrittTyp PRO_edcTyp => ENUM_AbstractCncSchrittTyp.FluxerDosis;

		public override bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt)
		{
			EDC_FluxerDosisSchritt eDC_FluxerDosisSchritt = i_edcVergleichsSchritt as EDC_FluxerDosisSchritt;
			if (eDC_FluxerDosisSchritt != null)
			{
				return Math.Abs(PRO_dblNeueDosis - eDC_FluxerDosisSchritt.PRO_dblNeueDosis) < 4.94065645841247E-324;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Fluxerdosis auf {PRO_dblNeueDosis}%";
		}
	}
}
