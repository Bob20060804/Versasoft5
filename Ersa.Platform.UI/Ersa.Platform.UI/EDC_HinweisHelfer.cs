using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Benutzerabfrage;
using System;

namespace Ersa.Platform.UI
{
	public static class EDC_HinweisHelfer
	{
		[Obsolete("Wird durch Umstellung auf InteractionController nicht mehr benötigt und demnächste entfernt.")]
		public static EDC_JaNeinAbbrechenHinweis FUN_edcHinweisUngespeicherteAenderungErstellen(INF_LokalisierungsDienst i_edcLokalisierungsDienst)
		{
			return new EDC_JaNeinAbbrechenHinweis
			{
				Title = i_edcLokalisierungsDienst.FUN_strText("13_81"),
				PRO_strText = i_edcLokalisierungsDienst.FUN_strText("13_82"),
				PRO_strJaText = i_edcLokalisierungsDienst.FUN_strText("13_80"),
				PRO_strNeinText = i_edcLokalisierungsDienst.FUN_strText("13_79"),
				PRO_strAbbrechenText = i_edcLokalisierungsDienst.FUN_strText("1_15")
			};
		}
	}
}
