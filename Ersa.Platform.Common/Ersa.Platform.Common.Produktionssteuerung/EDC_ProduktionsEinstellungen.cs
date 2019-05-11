using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Produktionssteuerung
{
	public class EDC_ProduktionsEinstellungen
	{
		[JsonProperty("ProductionMethod")]
		public List<ENUM_Produktionsart> PRO_lstProduktionsart
		{
			get;
			set;
		}

		[JsonProperty("CodeReadErrorConfirmationPossibilities")]
		public List<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit> PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten
		{
			get;
			set;
		}

		[JsonProperty("CodeNotFoundConfirmationPossibilities")]
		public List<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit> PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten
		{
			get;
			set;
		}

		[JsonProperty("ProtocolActive")]
		public bool PRO_blnLoetprotokollAktiv
		{
			get;
			set;
		}

		[JsonProperty("MesActive")]
		public bool PRO_blnMesAktiv
		{
			get;
			set;
		}

		[JsonProperty("TestBoardActive")]
		public bool PRO_blnTestBoardAktiv
		{
			get;
			set;
		}

		[JsonProperty("TwoManProductionReleaseActive")]
		public bool PRO_blnVierAugenFreigabeAktiv
		{
			get;
			set;
		}
	}
}
