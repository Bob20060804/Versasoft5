using Ersa.Platform.Common.Produktionssteuerung;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_ProduktionsUnterart : EDC_ProduktionsArtElement
	{
		public ENUM_Produktionsart PRO_enmProduktionsart
		{
			get;
		}

		public EDC_ProduktionsUnterart(string i_strLocKey, ENUM_Produktionsart i_enmProduktionsart)
			: base(i_strLocKey)
		{
			PRO_enmProduktionsart = i_enmProduktionsart;
		}
	}
}
