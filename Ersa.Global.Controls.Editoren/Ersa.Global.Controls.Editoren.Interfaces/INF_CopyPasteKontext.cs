using System;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces
{
	public interface INF_CopyPasteKontext
	{
		bool FUN_blnKannEinfuegen(Func<Point, Point> i_delPunktVerschiebung);

		void SUB_Einfuegen(Func<Point, Point> i_delPunktVerschiebung);
	}
}
