using Ersa.Global.Controls.Editoren.EditorElemente;
using System.Collections.Generic;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_LinieErstellenKontext
	{
		bool FUN_blnValidiereLinienPunkt(IReadOnlyList<Point> i_lstPunkte, Point i_sttNeuePunkte);

		EDC_LinienElement FUN_edcErzeugeLinie();

		void SUB_UebernehmeLinie(EDC_LinienElement i_edcLinie);
	}
}
