using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_LpAnfoStrategie<TLoetprogramm> where TLoetprogramm : class
	{
		ENUM_LpTransferStrategie PRO_enmLpTransferStrategie
		{
			get;
		}

		Task<EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm>> FUN_fdcAufLoetprogrammAnforderungReagierenAsync(TLoetprogramm i_edcLoetprogrammZuVerwenden, bool i_blnManuellEingebenesProgrammVerwenden, ENUM_LpTransferModus i_enmLpTransferModus);

		void SUB_ManuellEingegebenenCodeHinzufuegen(int i_i32GeraeteIndex, string i_strCode);

		void SUB_ManuellEingegebeneCodesLeeren();
	}
}
