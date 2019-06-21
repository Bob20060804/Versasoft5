using Ersa.Global.Controls.Editoren.EditorElemente;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_PositionsKontext
	{
		void SUB_EditorPositionenGeaendert(params EDC_EditorElement[] ia_edcElemente);

		bool FUN_blnDarfElementeVerschieben(params EDC_EditorElement[] ia_edcElemente);

		void SUB_BehandleKlickAnPositionOhneElement(Point i_sttPoint);
	}
}
