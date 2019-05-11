using System.Collections.Generic;

namespace Ersa.Platform.Common.Loetprotokoll
{
	public class EDC_LoetprotokollKopfElemente
	{
		public EDC_LoetprotokollElement PRO_edcEinlaufZeitpunkt
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcAuslaufZeitpunkt
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcDurchlaufzeit
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcTaktzeit
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcTransportbreiteIst
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcLaufendeNummer
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcBenutzer
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcBibliothekName
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcProgrammName
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcProgrammVersionsId
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcFehler
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcFertigGeloetet
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcAusgelaufen
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcMaschineIdentifier
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcArbeitsVersion
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcModus
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcBearbeitenExtern
		{
			get;
			set;
		}

		public EDC_LoetprotokollElement PRO_edcNutzenPosition
		{
			get;
			set;
		}

		public List<EDC_LoetprotokollElement> PRO_lstScannerCodes
		{
			get;
			set;
		}

		public void SUB_Init()
		{
			PRO_edcEinlaufZeitpunkt = new EDC_LoetprotokollElement();
			PRO_edcAuslaufZeitpunkt = new EDC_LoetprotokollElement();
			PRO_edcDurchlaufzeit = new EDC_LoetprotokollElement();
			PRO_edcTaktzeit = new EDC_LoetprotokollElement();
			PRO_edcTransportbreiteIst = new EDC_LoetprotokollElement();
			PRO_edcLaufendeNummer = new EDC_LoetprotokollElement();
			PRO_edcBenutzer = new EDC_LoetprotokollElement();
			PRO_edcFehler = new EDC_LoetprotokollElement();
			PRO_edcMaschineIdentifier = new EDC_LoetprotokollElement();
			PRO_edcBibliothekName = new EDC_LoetprotokollElement();
			PRO_edcProgrammName = new EDC_LoetprotokollElement();
			PRO_edcProgrammVersionsId = new EDC_LoetprotokollElement();
			PRO_edcArbeitsVersion = new EDC_LoetprotokollElement();
			PRO_edcModus = new EDC_LoetprotokollElement();
			PRO_lstScannerCodes = new List<EDC_LoetprotokollElement>();
		}
	}
}
