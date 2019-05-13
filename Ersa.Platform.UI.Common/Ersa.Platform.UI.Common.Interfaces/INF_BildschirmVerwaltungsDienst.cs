using System.Collections.Generic;
using System.Windows;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_BildschirmVerwaltungsDienst
	{
		EDC_Bildschirm PRO_edcPrimaerBildschirm
		{
			get;
		}

		IEnumerable<EDC_Bildschirm> FUN_enuAlleBildschirmeErmitteln();

		EDC_Bildschirm FUN_edcErmittleBildschirm(Window i_fdcFenster);

		EDC_Bildschirm FUN_edcErmittleBildschirm(Point i_fdcPunkt);

		DependencyObject FUN_fdcErmittleObjektAnPosition(Point i_fdcPosition);

		DependencyObject FUN_fdcErmittleObjektImAnwendungsMittelpunkt();
	}
}
