using Ersa.Global.Dienste.Interfaces;

namespace Ersa.Platform.UI.Programm
{
	public static class EDC_ProgrammBasisAngabenHelfer
	{
		public static bool FUN_blnBibUndPrgNameFuerNeuesProgrammPruefen(string i_strBibName, string i_strLpName, EDC_ProgrammDialogHelfer i_edcDialogHelfer, INF_IODienst i_edcIoDienst)
		{
			if (string.IsNullOrEmpty(i_strBibName))
			{
				i_edcDialogHelfer.SUB_KeineBibliothekAusgewaehltHinweisAnzeigen();
				return false;
			}
			if (!i_edcIoDienst.FUN_blnIstValiderOrdnerName(i_strBibName) || i_strBibName.Length > 31)
			{
				i_edcDialogHelfer.SUB_KeinGueltigerBiblothekNameHinweisAnzeigen();
				return false;
			}
			if (string.IsNullOrEmpty(i_strLpName))
			{
				i_edcDialogHelfer.SUB_ProgrammNameDarfNichtLeerSeinHinweisAnzeigen();
				return false;
			}
			if (!i_edcIoDienst.FUN_blnIstValiderOrdnerName(i_strLpName) || i_strLpName.Length > 31)
			{
				i_edcDialogHelfer.SUB_KeinGueltigerProgrammNameHinweisAnzeigen();
				return false;
			}
			return true;
		}
	}
}
