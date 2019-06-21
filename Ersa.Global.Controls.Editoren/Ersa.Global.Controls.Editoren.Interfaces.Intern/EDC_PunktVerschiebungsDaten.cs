using Ersa.Global.Controls.Editoren.EditorElemente;
using System.Collections.Generic;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Interfaces.Intern
{
	public class EDC_PunktVerschiebungsDaten
	{
		public EDC_EditorElementMitPunkten PRO_edcElement
		{
			get;
		}

		public object PRO_objPunktReferenz
		{
			get;
			set;
		}

		public IReadOnlyList<Point> PRO_lstOriginalPunkte
		{
			get;
		}

		public EDC_PunktVerschiebungsDaten(EDC_EditorElementMitPunkten i_edcElement, object i_objPunktReferenz, IReadOnlyList<Point> i_lstOriginalPunkte)
		{
			PRO_edcElement = i_edcElement;
			PRO_objPunktReferenz = i_objPunktReferenz;
			PRO_lstOriginalPunkte = i_lstOriginalPunkte;
		}
	}
}
