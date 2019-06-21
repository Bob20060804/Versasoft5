using Ersa.Global.Controls.Editoren.EditorElemente;
using System.Collections.Generic;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_RechteckErstellenKontext
	{
		bool FUN_blnValidiereRechteckPunkt(IReadOnlyList<Point> i_lstPunkte, Point i_sttNeuePunkte);

		EDC_RechteckElement FUN_edcErzeugeRechteck();

		void SUB_UebernehmeRechteck(EDC_RechteckElement i_edcRechteck);
	}
}
