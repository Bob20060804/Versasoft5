using System;

namespace Ersa.Platform.Logging
{
	[Flags]
	public enum ENUM_LogLevel : long
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
		PcbDurchlauf = 0x20000,
		LoetProgramm = 0x40000,
		Uebersicht = 0x80000,
		Basisklasse = 0x100000,
		Leiterkarte = 0x200000,
		Ruecktransportmodul = 0x400000,
		Heizung = 0x800000,
		Fluxer = 0x1000000,
		Loeteinheit = 0x2000000,
		Cnc = 0x4000000,
		enmSpsKommunkation = 0x8000000,
		enmShell = 0x10000000,
		enmMaschinenModel = 0x20000000,
		enmBenutzerverwaltung = 0x40000000,
		enmUserControls = 0x80000000,
		enmHilfsklassen = 0x100000000,
		enmMefKontext = 0x200000000,
		enmCad = 0x400000000,
		enmPrism = 0x800000000,
		enmViewModel = 0x1000000000,
		enmMeldungen = 0x2000000000,
		enmEventSourcing = 0x4000000000,
		enmSetup = 0x8000000000,
		AlleErweitert = 0x77FFFFFFF8,
		Alle = 0x77FFFFFFFF
	}
}
