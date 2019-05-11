using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Konstanten
{
	public static class EDC_DataKonstanten
	{
		public const int gC_i32ErforderlicheErsasoftDatenbankVersion = 82;

		public const string gC_strUniqueSequence = "ess5_sequnique";

		public const string gC_strProtokollSequence = "ess5_seprotokoll";

		public const string gC_strIndizesTablespace = "ess5_indizes";

		public const string gC_strBilderTablespace = "ess5_images";

		public const string gC_strMeldungenTablespace = "ess5_messages";

		public const string gC_strProgrammeTablespace = "ess5_programs";

		public const string gC_strProtokolleTablespace = "ess5_protocol";

		public const string gC_strProzessschreiberTablespace = "ess5_recorder";

		public const string gC_strProduktionTablespace = "ess5_production";

		public const string gC_strCadTablespace = "ess5_cad";

		public const string gC_strBauteileTablespace = "ess5_components";

		public const string gC_strZeitKonvertierung = "Zeit";

		public const string gC_strDatenKonvertierungReal = "DatenReal";

		public const string gC_strDatenKonvertierungInt64 = "DatenInt64";

		public const string gC_strDatenKonvertierungZaehlerElement = "DatenEditInt64";

		public static readonly Dictionary<int, string> gs_dicDatenbankVersionen = new Dictionary<int, string>
		{
			{
				36,
				"08.07.2016 Neue Tabellen für die Konfiguration des Codelesens und der Code-Pipeline-Elemente. Löschen der bisher bestehenden Scanner-Konfigurationstabelle"
			},
			{
				37,
				"06.09.2016 Neue Spalte für automatische Abmeldung in der Benutzertabelle"
			},
			{
				38,
				"14.09.2016 Neue Tabellen für Düsenbetriebszeitenzähler, Erweiterung bestehende Düsentabelle um GeometrieId"
			},
			{
				39,
				"22.09.2016 Neue Tabellen für CAD4-Verwaltung"
			},
			{
				40,
				"11.10.2016 Neue Tabelle für Düsengeometrien"
			},
			{
				41,
				"27.10.2016 Änderung der Tabelle CadProjektHistory -> Umstellung auf HistroryIds aus Programmverwaltung"
			},
			{
				42,
				"07.11.2016 Erweiterung EDC_CadBildData Tabelle für Matrixtransformationen (für Update aus alten CAD3 Daten)"
			},
			{
				43,
				"01.12.2016 Neue Spalte Modus in der Tabelle CadAblauf"
			},
			{
				44,
				"14.12.2016 Refactoring Nutzentabellen für Codezurodnung auf Nutzen"
			},
			{
				45,
				"14.12.2016 Neue Tabelle EDC_CadRoutenSchrittData und neue Tabelle CadRoutenData"
			},
			{
				46,
				"21.12.2016 Neue Spalte SynchronisationsPosition in CadRoutenData"
			},
			{
				47,
				"11.01.2017 Tabelle CAD-Einstellungen muss neu erstellt werden"
			},
			{
				48,
				"11.01.2017 Neue Spalte in der Maschineneinstellung-Tabelle"
			},
			{
				49,
				"11.01.2017 Neue Spalten in der LötprogrammVersionen Tabelle (neue Spalten: Status und SetNumber)"
			},
			{
				50,
				"19.01.2017 Neue Tabelle 'solderingversionvalid'"
			},
			{
				51,
				"08.02.2017 Konsolidierung der Cad-Tabellen"
			},
			{
				54,
				"07.03.2017 Neue Tabelle 'NozzleSetValues'"
			},
			{
				55,
				"17.03.2017 Anpassungen an der Lötprogramm-Bildtabelle (3)"
			},
			{
				56,
				"12.04.2017 Anpassungen an der Codekonfigurations-Tabelle (neue Spalte AlbFromElb)"
			},
			{
				57,
				"08.05.2017 Neue Tabelle 'Packages' und neuer Tablespace 'ess5_components'"
			},
			{
				58,
				"09.05.2017 Neue Spalte in LötprotokollKopfData-Tabelle hinzugefügt (Spalte Arbeitsversion)"
			},
			{
				59,
				"01.06.2017 Neue Spalten in CodeCacheEintragData-Tabelle hinzugefügt (Spalte Bedeutung und ArrayIndex)"
			},
			{
				60,
				"01.06.2017 Neue Spalten in SolderingPrograms hinzugeüft (Spalte InfeedInspection und OutfeedInspection)"
			},
			{
				61,
				"25.07.2017 Neue Spalten für Pitch in der Bauteiltabelle"
			},
			{
				62,
				"25.07.2017 Neue Spalten für das Default-Lötprogramm pro Maschine in der Maschinentabelle"
			},
			{
				63,
				"25.07.2017 Neue Tabelle für die Bauteilmakros"
			},
			{
				64,
				"22.08.2017 Neue Tabelle NutzenData"
			},
			{
				65,
				"19.09.2017 Erweiterung Lötprogrammtabelle um Spalte 'Version'"
			},
			{
				66,
				"19.09.2017 Neue Tabelle NozzleAssessmentParameter (Düsen-Beurteilung Parameter für BV) angelegt"
			},
			{
				67,
				"21.09.2017 Erweiterung Cad-Einstellungen Tabelle um Spalte 'Data'"
			},
			{
				68,
				"21.11.2017 Neue Tabelle BetriebsmittelData angelegt"
			},
			{
				69,
				"21.09.2017 Neue Splitting-Tabelle UserMachineMapping für die Benutzerverwaltung (Datum vor 68, da in eigenen Branch entwickelt)"
			},
			{
				70,
				"09.01.2018 Neue Spalte MachineId in CodeCache-Tabelle }"
			},
			{
				71,
				"09.01.2018 Neue Tabellen für die AOI-Programm Verwaltung }"
			},
			{
				72,
				"24.01.2018 Tabelle AoiStepData und AoiResults löschen und neu anlegen, da sich der PK für AoiStepData geändert hat}"
			},
			{
				73,
				"08.02.2018 Erweiterung Tabelle BetriebsmittelData um Spalte 'Deleted'}"
			},
			{
				74,
				"08.03.2018 Neue Spalten in Meldungen-Tabelle und neue Tabelle Meldungen-Context}"
			},
			{
				75,
				"11.04.2018 Neue Tabelle LinkCache}"
			},
			{
				76,
				"24.04.2018 Neue Tabelle Equipment und EquipmentTools"
			},
			{
				77,
				"02.05.2018 Erweiterung Tabelle UserTracking um die Spalten Parameter, OldValue, NewValue"
			},
			{
				78,
				"18.06.2018 Erweiterung Tabelle ProgramPanelParameter um Nutzendaten"
			},
			{
				79,
				"26.07.2018 Neue Spalte Modus in LötprotokollKopfData-Tabelle"
			},
			{
				80,
				"08.08.2018 Neue Spalten in LoetprogrammVersionData-Tabelle hinzugefügt (FreigabeStatus, FreigabeKommentar))"
			},
			{
				81,
				"18.10.2018 Spalte Username der Tabelle users auf 50 Zeichen erweitert"
			},
			{
				82,
				"26.10.2018 Spalte Tabelle AoiResult geändert, deshalb droppen und neu anlegen (bisher nicht verwendet)"
			}
		};
	}
}
