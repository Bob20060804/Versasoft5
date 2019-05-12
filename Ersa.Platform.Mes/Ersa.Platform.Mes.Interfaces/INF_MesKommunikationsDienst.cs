using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Modell;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesKommunikationsDienst
	{
		ENUM_MesTyp PRO_enmMesTyp
		{
			get;
		}

		bool PRO_blnIstVerbunden
		{
			get;
		}

		IEnumerable<INF_MesFunktion> PRO_enuFunktionen
		{
			get;
		}

		Task<EDC_MesInitialisierungsRueckgabe> FUN_fdcInitialisiereAsync();

		Task<EDC_MesInitialisierungsRueckgabe> FUN_fdcDeinitialisiereAsync();

		Task<EDC_MesTypEinstellung> FUN_fdcHoleDienstEinstellungenAsync<T>(ENUM_MesTyp i_enmMesTyp, EDC_MesTypEinstellung i_edcDefaultEinstellungen);

		Task FUN_fdcSetzeDienstEinstellungenAsync<T>(ENUM_MesTyp i_enmMesTyp, EDC_MesTypEinstellung i_edcEinstellungen);

		Task<EDC_MesKommunikationsRueckgabe> FUN_fdcNachrichtSendenAsync(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten);
	}
}
