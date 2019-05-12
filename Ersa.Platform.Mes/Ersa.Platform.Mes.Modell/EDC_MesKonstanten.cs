using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Konfiguration;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Modell
{
	public static class EDC_MesKonstanten
	{
		public const string gC_strMefContractNameItac = "Itac";

		public const string gC_strMefContractNameRs232App = "Rs232App";

		public const string gC_strMefContractNameSiemensWien = "SiemensWien";

		public const string gC_strMefContractNameZollner = "Zollner";

		public const string gC_strMefContractNameZvei = "Zvei";

		public const string gC_strBibliothekProgrammTrennzeichen = "/";

		public const string gC_strCodeTrennzeichen = ";";

		public const int gC_i32PingIntervall = 10;

		public static readonly EDC_MesKonfiguration gs_edcMesDefaultKonfiguration = new EDC_MesKonfiguration
		{
			PRO_enuMesTyp = ENUM_MesTyp.KeinMes,
			PRO_blnIstFunctionExitAktiv = false,
			PRO_lstMesFunktionen = new List<EDC_MesTypFunktionenListe>()
		};
	}
}
