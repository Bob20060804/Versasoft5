using System.Collections.Generic;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_ProduktionsArtGruppe : EDC_ProduktionsArtElement
	{
		public IEnumerable<EDC_ProduktionsArten> PRO_enuProduktionsArten
		{
			get;
		}

		public EDC_ProduktionsArtGruppe(string i_strLocKey, params EDC_ProduktionsArten[] ia_edcArten)
			: base(i_strLocKey)
		{
			PRO_enuProduktionsArten = ia_edcArten;
		}
	}
}
