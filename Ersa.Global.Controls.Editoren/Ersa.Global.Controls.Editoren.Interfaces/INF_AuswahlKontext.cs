using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Werkzeuge;
using System.Collections.Generic;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_AuswahlKontext
	{
		void SUB_AuswahlGeaendert(IEnumerable<EDC_EditorElement> i_enuElemente);

		void SUB_EntferneElemente(IEnumerable<EDC_EditorElement> i_enuElemente);

		void SUB_BewegeElemente(IEnumerable<EDC_EditorElement> elements, ENUM_BewegungsRichtung direction);
	}
}
