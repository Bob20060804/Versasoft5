using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Selektiv;

namespace Ersa.Platform.Common.Extensions
{
	public static class EDC_DuesenbetriebsDatenExtensions
	{
		public static EDC_DuesenBetriebWerte FUN_edcKonvertiereZuBetriebsWerte(this EDC_DuesenbetriebWerteData i_edcDusenData, long i_i64DuesenId = 0L)
		{
			return new EDC_DuesenBetriebWerte
			{
				PRO_i64DuesenId = i_i64DuesenId,
				PRO_i64WelleEinZeit = i_edcDusenData.PRO_i64WelleEinZeit,
				PRO_i64WelleAusZeit = i_edcDusenData.PRO_i64WelleAusZeit,
				PRO_i64GesamtZeit = i_edcDusenData.PRO_i64GesamtZeit,
				PRO_i64AnzahlAktivierungen = i_edcDusenData.PRO_i64AnzahlAktivierungen,
				PRO_enmTiegel = i_edcDusenData.PRO_enmTiegel
			};
		}
	}
}
