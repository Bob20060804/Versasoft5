using System;

namespace Ersa.Logging
{
	[Flags]
	public enum ENUM_LoggingLevels : ulong
	{
		Kein = 0x0,
		Fehler = 0x1,
		Warnung = 0x2,
		Info = 0x4,
		AlleBasis = 0x7,
		Traceability = 0x8,
		Einlauf = 0x10,
		Einlaufmodul = 0x20,
		Fluxermodul = 0x40,
		Vorheizmodul = 0x80,
		Loetmodul = 0x100,
		Auslaufmodul = 0x200,
		Auslauf = 0x400,
		Protokoll = 0x800,
		Codebetrieb = 0x1000,
		Konfiguration = 0x2000,
		LoetProgrammEditor = 0x4000,
		Datensicherung = 0x8000,
		ProzessSchreiber = 0x10000,
		PCBDurchlauf = 0x20000,
		LoetProgramm = 0x40000,
		Uebersicht = 0x80000,
		Basisklasse = 0x100000,
		Leiterkarte = 0x200000,
		Ruecktransportmodul = 0x400000,
		Heizung = 0x800000,
		Fluxer = 0x1000000,
		Loeteinheit = 0x2000000,
		CNC = 0x4000000,
		SpsKommunkation = 0x8000000,
		Shell = 0x10000000,
		MaschinenModel = 0x20000000,
		Benutzerverwaltung = 0x40000000,
		UserControls = 0x80000000,
		Hilfsklassen = 0x100000000,
		MefKontext = 0x200000000,
		Cad = 0x400000000,
		Prism = 0x800000000,
		ViewModel = 0x1000000000,
		Meldungen = 0x2000000000,
		EventSourcing = 0x4000000000,
		Setup = 0x8000000000,
		AlleErweitert = 0xFFFFFFFFF8,
		Alle = 0xFFFFFFFFFF
	}
}
