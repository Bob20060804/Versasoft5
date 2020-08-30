using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_LoetprogrammTransferDienst<TLoetprogramm> where TLoetprogramm : class
	{
		Task FUN_fdcLoetprogrammStartAnfordernAsync(ENUM_LpTransferStrategie i_enuLpTransferStrategie);

		Task<bool> FUN_fdcLoetprogrammTransferAbschliessenAsync(bool i_blnTransferNichtAbwarten);

		void SUB_LoetprogrammExistiertNichtSignalisieren();

		void SUB_ManuellEingegebenenCodeHinzufuegen(int i_i32GeraeteIndex, string i_strCode);

		void SUB_SetzeManuellEingegebenesProgramm(TLoetprogramm i_edcLoetprogramm);

		void SUB_SetzeLoetprogrammTransferModus(ENUM_LpTransferModus i_enmLpTransferModus);

		Task FUN_fdcVersucheLoetprogrammZuUebertragenAsync();

		Task FUN_fdcCodebetriebNeuStarten();

		INF_LpAnfoStrategie<TLoetprogramm> FUN_edcHoleLoetProgrammAnfoStrategie();
	}
}
