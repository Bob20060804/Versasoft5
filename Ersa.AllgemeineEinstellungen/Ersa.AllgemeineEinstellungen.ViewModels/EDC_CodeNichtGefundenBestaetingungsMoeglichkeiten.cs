using Ersa.Platform.Common.Produktionssteuerung;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten
	{
		public bool PRO_blnAktiv
		{
			get;
			set;
		}

		public ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit PRO_enmCodeNichtGefundenBestaetingungsMoeglichkeit
		{
			get;
			set;
		}

		public string PRO_strLocKey
		{
			get;
			set;
		}
	}
}
