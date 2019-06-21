using Ersa.Global.Controls.Editoren.EditorElemente;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces.Intern
{
	internal interface INF_EditorKontext
	{
		IEnumerable<EDC_EditorElement> FUN_enuHoleAlleElemente();

		EDC_EditorElement FUN_edcHoleElementAnPosition(Point i_sttPosition);

		IDisposable FUN_fdcFuegeElementTemporaerHinzu(EDC_EditorElement i_edcElement);

		void SUB_AendereBildausschnitt(Rect i_sttBereich);
	}
}
