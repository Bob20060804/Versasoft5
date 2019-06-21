using Ersa.Platform.Common.Produktionssteuerung;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten
	{
		public bool PRO_blnAktiv
		{
			get;
			set;
		}

		public ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit PRO_enmCodeLeseFehlerBestaetingungsMoeglichkeit
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
