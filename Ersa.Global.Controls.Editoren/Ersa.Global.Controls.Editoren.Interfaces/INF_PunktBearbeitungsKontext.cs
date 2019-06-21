using Ersa.Global.Controls.Editoren.Interfaces.Intern;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_PunktBearbeitungsKontext
	{
		bool FUN_blnVeraenderungValidieren(EDC_PunktVerschiebungsDaten i_edcDaten);

		void SUB_EditorPunkteGeaendert(EDC_PunktVerschiebungsDaten i_edcDaten);
	}
}
