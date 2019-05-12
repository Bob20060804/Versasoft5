using Ersa.Platform.Common.Mes;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesDienst : INF_MeldungProzessor
	{
		Task<EDC_MesInitialisierungsRueckgabe> FUN_fdcInitialisiereAsync();

		Task FUN_fdcDeinitialisiereAsync();

		Task<bool> FUN_fdcIstMesSystemVerbundenAsync();

		Task<bool> FUN_fdcIstFunktionAktivAsync(ENUM_MesFunktionen i_enuFunktion);

		Task<bool> FUN_fdcIstFunktionUndMesAktivAsync(ENUM_MesFunktionen i_enuFunktion);

		Task<bool> FUN_fdcLoetprotokollEinzelnSendenAsync();

		Task<EDC_MesKommunikationsRueckgabe> FUN_fdcFunktionAufrufenAsync(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten);
	}
}
