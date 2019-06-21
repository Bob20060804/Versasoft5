using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Platform.Common.Data.Aoi;
using Ersa.Platform.Common.Data.Bauteile;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Data.CodeCache;
using Ersa.Platform.Common.Data.Codetabelle;
using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Data.Loetprotokoll;
using Ersa.Platform.Common.Data.Maschinenkonfiguration;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Data.Nutzen;
using Ersa.Platform.Common.Data.Produktionssteuerung;
using Ersa.Platform.Common.Data.Prozessschreiber;
using Ersa.Platform.Common.Data.Sprachen;
using Ersa.Platform.Data.DataAccess.Benutzer;
using Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung;
using Ersa.Platform.Data.DataAccess.Meldungen;
using Ersa.Platform.Data.DatenModelle.Organisation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DatenbankModelle
{
	public class EDC_ErsasoftDatenbankModell : EDC_DatenbankModel
	{
		public override string PRO_strSequencesErstellungsString
		{
			get
			{
				if (base.PRO_edcDatenbankDialekt.PRO_blnHatSequences)
				{
					List<string> values = new List<string>
					{
						base.PRO_edcDatenbankDialekt.FUN_strErstelleSequenceAnweisung("ess5_sequnique"),
						base.PRO_edcDatenbankDialekt.FUN_strErstelleSequenceAnweisung("ess5_seprotokoll")
					};
					return string.Join(";", values);
				}
				return string.Format("  Insert into {0} ({1}, {2}) values ('{3}', 1);\r\n                        Insert into {0} ({1}, {2}) values ('{4}', 1)", "Sequences", "Name", "Value", "ess5_sequnique", "ess5_seprotokoll");
			}
		}

		public override string PRO_strFunctionErstellungsString => string.Empty;

		public override string PRO_strTriggerErstellungsString => string.Empty;

		public override string PRO_strConstraintsErstellungsString => string.Empty;

		private string PRO_strBenutzerTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BenutzerData));

		private string PRO_strBenutzerMappingTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BenutzerMappingData));

		private string PRO_strBenutzerTrackTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BenutzerTrackData));

		private string PRO_strAktiveBenutzerTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_AktiverBenutzerData));

		private string PRO_strParameterTabellenString
		{
			get
			{
				string text = FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_Parameter));
				return string.Format("{0};\r\n                        Insert into {1} ({2}, {3}) values ('{4}', {5})", text, "Parameter", "Parameter", "Value", "database version", 82);
			}
		}

		private string PRO_strSequenceTabellenString
		{
			get
			{
				if (!base.PRO_edcDatenbankDialekt.PRO_blnHatSequences)
				{
					return FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_Sequence));
				}
				return string.Empty;
			}
		}

		private string PRO_strMeldungenDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MeldungData));

		private string PRO_strZyklischeMeldungenDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_ZyklischeMeldungData));

		private string PRO_strZyklischeMeldungenVorlageDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_ZyklischeMeldungVorlageData));

		private string PRO_strZyklischeMeldungenVorlageGruppeDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_ZyklischeMeldungVorlageGruppeData));

		private string PRO_strSprachenEintragTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_SprachenEintragData));

		private string PRO_strProzessschreiberVariablenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_SchreiberVariablenData));

		private string PRO_strMaschineTabelleString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschineData));

		private string PRO_strMaschineGruppenTabelleString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenGruppeData));

		private string PRO_strMaschineGruppenMitgliedTabelleString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenGruppenMitgliedData));

		private string PRO_strLoetprogrammTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammData));

		private string PRO_strLoetprogrammBibliothekTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammBibliothekData));

		private string PRO_strLoetprogrammBildTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammBildData));

		private string PRO_strLoetprogrammNutzenDatenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammNutzenData));

		private string PRO_strLoetprogrammParameterTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammParameterData));

		private string PRO_strLoetprogrammSatzDatenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammSatzDatenData));

		private string PRO_strLoetprogrammVersionTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammVersionData));

		private string PRO_strCodetabellenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodetabelleData));

		private string PRO_strCodetabellenEintragTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodetabelleneintragData));

		private string PRO_strMaschinenkonfigurationTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenkonfigurationData));

		private string PRO_strDuesenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenData));

		private string PRO_strEcp3DatenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammEcp3DatenData));

		private string PRO_strLoetprotokollVariablenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprotokollVariablenData));

		private string PRO_strProduktionssteuerungDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_ProduktionssteuerungData));

		private string PRO_strCodeCacheTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodeCacheEintragData));

		private string PRO_strBetriebsdatenKopfTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenBetriebsDatenKopfData));

		private string PRO_strBetriebsdatenWertTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenBetriebsDatenWerteData));

		private string PRO_strMaschinenEinstellungenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MaschinenEinstellungenData));

		private string PRO_strDuesenBetriebsdatenVorgebaTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenbetriebVorgabenData));

		private string PRO_strDuesenBetriebsdatenWerteTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenbetriebWerteData));

		private string PRO_strDuesenBetriebWechselTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenbetriebWechselData));

		private string PRO_strCodeKonfigurationTabellenString => $"{FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodeKonfigData))};{FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodeKonfigTrackData))};{FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodePipelineData))};{FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CodePipelineTrackData))}";

		private string PRO_strCadVerboteneBereicheTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadVerbotenerBereichData));

		private string PRO_strCadBewegungsschrittTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadAblaufSchrittData));

		private string PRO_strCadBilddatenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadBildData));

		private string PRO_strCadBewegungsgruppenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadBewegungsGruppenData));

		private string PRO_strCadEinstellungenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadEinstellungenData));

		private string PRO_strCadCncSchrittTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadCncSchrittData));

		private string PRO_strCadRoutingTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadRoutenData));

		private string PRO_strCadRoutingSchrittTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_CadRoutenSchrittData));

		private string PRO_strLoetprogrammVersionValideTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LoetprogrammVersionValideData));

		private string PRO_strAoiProgrammTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_AoiProgramData));

		private string PRO_strAoiStepDatenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_AoiSchrittData));

		private string PRO_strAoiErgebnisTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_AoiErgebnisData));

		private string PRO_strLoescheAoiStepDatenTabellenString => string.Format("Drop Table {0}", "AoiStepData");

		private string PRO_strLoescheAoiErgebnisTabellenString => string.Format("Drop Table {0}", "AoiResults");

		private string PRO_strDuesenGeometrieTabellenString
		{
			get
			{
				string arg = FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenGeometrienData));
				IEnumerable<string> values = EDC_DuesenGeometrienData.FUN_enuHoleDefaultGeometrieDatenErstellungsStatementListe();
				return string.Format("{0};{1}", arg, string.Join<string>(";", values));
			}
		}

		private string PRO_strDuesenSollwerteTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_DuesenSollwerteData));

		private string PRO_strBauteilTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BauteilData));

		private string PRO_strBauteilMakroTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BauteilMakroData));

		private string PRO_strNutzenDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_NutzenData));

		private string PRO_strBetriebsmittelDataTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_BetriebsmittelData));

		private string PRO_strMeldungenContextTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_MeldungContextData));

		private string PRO_strLinkCacheTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_LinkCacheData));

		private string PRO_strRuestkomponentenTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_RuestkomponentenData));

		private string PRO_strRuestwerkzeugeTabellenString => FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_RuestwerkzeugeData));

		private string PRO_strLoescheCodeCacheTabellenString => string.Format("Drop Table {0}", "CodeCache");

		private string PRO_strFuegeSpalteMaschinenIdCodeCacheHinzuString
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("MachineId", typeof(long).Name, 0);
				return string.Format("Alter Table {0} {1}", "CodeCache", arg);
			}
		}

		private string PRO_strLoescheBetriebsdatenWertTabellenString => string.Format("Drop Table {0}", "MachineOperatingDataValue");

		private string PRO_strLoescheDuesenGeometrieTabellenString => string.Format("Drop Table {0}", "NozzleGeometries");

		private string PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzu3String
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Metainfo6", typeof(string).Name, 50);
				return string.Format("Alter Table {0} {1}", "ProgramImages", arg);
			}
		}

		private string PRO_strFuegeSpalteAlbAusElbZuCodeConfigurationHinzuString
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("AlbFromElb", typeof(bool).Name, 0);
				return string.Format("Alter Table {0} {1}", "CodeConfigurations", arg);
			}
		}

		private string PRO_strBilderDatenDateinamenSpalte => string.Format("Alter Table {0} {1}", "ProgramImages", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("FileName", typeof(string).Name, 250));

		private string PRO_strEcp3DatenBildspaltenDroppen => string.Format("ALTER TABLE {0} DROP COLUMN FileContent;\r\n                        ALTER TABLE {0} DROP COLUMN FileName;", "ProgramEcp3Data");

		private string PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzuString
		{
			get
			{
				string text = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Metainfo1", typeof(string).Name, 50, i_blnIsErsteSpalte: true);
				string text2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Metainfo2", typeof(string).Name, 50, i_blnIsErsteSpalte: false);
				string text3 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Metainfo3", typeof(string).Name, 50, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}, {3}", "ProgramImages", text, text2, text3);
			}
		}

		private string PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzu2String
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Metainfo4", typeof(string).Name, 50, i_blnIsErsteSpalte: true);
				string arg2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Metainfo5", typeof(string).Name, 50, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}", "ProgramImages", arg, arg2);
			}
		}

		private string PRO_strFuegeSpaltenZuBenutzerTabelleHinzuString => string.Format("Alter Table {0} {1}", "Users", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("IsActiveAfterAutoLogout", typeof(bool).Name, 0));

		private string PRO_strFuegeMemoSpaltenZuEinstellungenTabelleHinzuString => string.Format("Alter Table {0} {1}", "MachineSettings", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("MemoValue", typeof(string).Name, 0));

		private string PRO_strFuegeSpaltenZuLoetprogrammVersionenDataTabellenHinzuString
		{
			get
			{
				string text = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Status", typeof(int).Name, 0, i_blnIsErsteSpalte: true);
				string text2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("SetNumber", typeof(int).Name, 0, i_blnIsErsteSpalte: false);
				string text3 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("ChangeDate", typeof(DateTime).Name, 0, i_blnIsErsteSpalte: false);
				string text4 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("ChangeUser", typeof(long).Name, 0, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}, {3}, {4}", "ProgramHistory", text, text2, text3, text4);
			}
		}

		private string PRO_strFuegeSpaltenZuCodesCacheTabelleHinzuString
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("ArrayIndex", typeof(long).Name, 0, i_blnIsErsteSpalte: true);
				string arg2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Meaning", typeof(string).Name, 100, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}", "CodeCache", arg, arg2);
			}
		}

		private string PRO_strFuegeAoiSpaltenZuLoetprogramTabelleHinzuString
		{
			get
			{
				string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("InfeedInspection", typeof(string).Name, 200, i_blnIsErsteSpalte: true);
				string arg2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("OutfeedInspection", typeof(string).Name, 200, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}", "SolderingPrograms", arg, arg2);
			}
		}

		private string PRO_strFuegeSpaltenZuMeldungenTabelleHinzuString
		{
			get
			{
				string text = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("System", typeof(int).Name, 0, i_blnIsErsteSpalte: true);
				string text2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Code", typeof(int).Name, 0, i_blnIsErsteSpalte: false);
				string text3 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("MessageReset", typeof(DateTime).Name, 0, i_blnIsErsteSpalte: false);
				string text4 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("PossibleActions", typeof(string).Name, 30, i_blnIsErsteSpalte: false);
				string text5 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("RequestedActions", typeof(string).Name, 30, i_blnIsErsteSpalte: false);
				return string.Format("Alter Table {0} {1}, {2}, {3}, {4}, {5}", "Messages", text, text2, text3, text4, text5);
			}
		}

		public EDC_ErsasoftDatenbankModell(ENUM_DatenbankTyp i_enmDatenbankTyp, string i_strDatenbankName, string i_strDatenverzeichnis)
			: base(i_enmDatenbankTyp, i_strDatenbankName, i_strDatenverzeichnis, 82)
		{
			SUB_ErstelleTabellenErstellungsliste();
			SUB_ErstelleMigrationsliste();
			SUB_ErstelleTablespaceErstellungsliste();
		}

		private void SUB_ErstelleTablespaceErstellungsliste()
		{
			base.PRO_dicTablespacesErstellungsliste.Clear();
			if (base.PRO_edcDatenbankDialekt.PRO_blnHatTablespaces)
			{
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_indizes", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_indizes", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_images", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_images", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_messages", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_messages", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_programs", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_programs", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_protocol", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_protocol", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_recorder", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_recorder", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_production", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_production", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_cad", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_cad", base.PRO_strDatenbankDatenVerzeichnis));
				base.PRO_dicTablespacesErstellungsliste.Add("ess5_components", base.PRO_edcDatenbankDialekt.FUN_strHoleTablespaceErstellungsString("ess5_components", base.PRO_strDatenbankDatenVerzeichnis));
			}
		}

		private void SUB_ErstelleTabellenErstellungsliste()
		{
			base.PRO_lstTabellenErstellungsliste.Clear();
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strParameterTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBenutzerTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBenutzerMappingTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBenutzerTrackTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strAktiveBenutzerTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMeldungenDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strZyklischeMeldungenDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strZyklischeMeldungenVorlageDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strZyklischeMeldungenVorlageGruppeDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strSprachenEintragTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMaschineTabelleString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMaschineGruppenTabelleString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMaschineGruppenMitgliedTabelleString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strProzessschreiberVariablenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammBibliothekTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammBildTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammNutzenDatenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammParameterTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammSatzDatenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammVersionTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCodetabellenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCodetabellenEintragTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strSequenceTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMaschinenkonfigurationTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strEcp3DatenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprotokollVariablenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strProduktionssteuerungDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCodeCacheTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBetriebsdatenKopfTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBetriebsdatenWertTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenGeometrieTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMaschinenEinstellungenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCodeKonfigurationTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenBetriebsdatenVorgebaTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenBetriebsdatenWerteTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenBetriebWechselTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadVerboteneBereicheTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadBewegungsschrittTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadBilddatenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadBewegungsgruppenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadEinstellungenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadCncSchrittTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadRoutingTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strCadRoutingSchrittTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLoetprogrammVersionValideTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strDuesenSollwerteTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBauteilTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBauteilMakroTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strNutzenDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strBetriebsmittelDataTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strAoiProgrammTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strAoiStepDatenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strAoiErgebnisTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strMeldungenContextTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strLinkCacheTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strRuestkomponentenTabellenString);
			base.PRO_lstTabellenErstellungsliste.Add(PRO_strRuestwerkzeugeTabellenString);
		}

		private string FUN_strSkriptFuerVersion22()
		{
			return PRO_strBilderDatenDateinamenSpalte;
		}

		private string FUN_strSkriptFuerVersion23()
		{
			return PRO_strEcp3DatenBildspaltenDroppen;
		}

		private string FUN_strSkriptFuerVersion24()
		{
			return FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_ProduktionssteuerungData));
		}

		private string FUN_strSkriptFuerVersion25()
		{
			return PRO_strCodeCacheTabellenString;
		}

		private string FUN_strSkriptFuerVersion28()
		{
			return PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzuString;
		}

		private string FUN_strSkriptFuerVersion29()
		{
			return PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzu2String;
		}

		private string FUN_strSkriptFuerVersion30()
		{
			return $"{PRO_strLoescheCodeCacheTabellenString}; {PRO_strCodeCacheTabellenString}";
		}

		private string FUN_strSkriptFuerVersion31()
		{
			return $"{PRO_strBetriebsdatenKopfTabellenString}; {PRO_strBetriebsdatenWertTabellenString}";
		}

		private string FUN_strSkriptFuerVersion32()
		{
			return $"{PRO_strLoescheBetriebsdatenWertTabellenString}; {PRO_strBetriebsdatenWertTabellenString}";
		}

		private string FUN_strSkriptErstelleDuesenGeometrienNeu()
		{
			return $"{PRO_strLoescheDuesenGeometrieTabellenString}; {PRO_strDuesenGeometrieTabellenString}";
		}

		private string FUN_strSkriptFuerVersion38()
		{
			string text = string.Format("Alter Table {0} {1}", "Nozzles", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("GeomertyId", typeof(long).Name, 0));
			return $"{PRO_strDuesenBetriebsdatenVorgebaTabellenString}; {PRO_strDuesenBetriebsdatenWerteTabellenString}; {PRO_strDuesenBetriebWechselTabellenString}; {text}";
		}

		private string FUN_strSkriptFuerVersion51()
		{
			return string.Format("Drop Table cadprojecthistory; Drop Table cadprojects; Drop Table cadprojecttrackings;Drop Table cadprojectsettings;Drop Table {0};Drop Table {1};Drop Table {2};Drop Table {3};Drop Table {4};", "CadFlowSteps", "CadMotionGroups", "CadImages", "CadCncSteps", "CadForbiddenAreas");
		}

		private string FUN_strSkriptFuerVersion52()
		{
			return string.Format("Drop Table {0};", "CadRoutingData") + string.Format("Drop Table {0}", "CadRoutingSteps");
		}

		private string FUN_strSkriptFuerVersion53()
		{
			return $"{PRO_strCadVerboteneBereicheTabellenString}; {PRO_strCadBewegungsschrittTabellenString}; {PRO_strCadBilddatenTabellenString}; {PRO_strCadBewegungsgruppenTabellenString}; {PRO_strCadEinstellungenTabellenString}; {PRO_strCadCncSchrittTabellenString}; {PRO_strCadRoutingSchrittTabellenString}; {PRO_strCadRoutingTabellenString}";
		}

		private string FUN_strSkriptFuerVersion58()
		{
			List<string> list = new List<string>();
			IEnumerable<string> enumerable = from i_strTabellenName in base.PRO_lstTabellenNamen
			where i_strTabellenName.ToLower().StartsWith(EDC_LoetprotokollKopfData.PRO_strTabellennameMitMaschinenPrefix)
			select i_strTabellenName;
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Draft", typeof(bool).Name, 0);
			foreach (string item in enumerable)
			{
				list.Add($"Alter Table {item} {arg}");
			}
			return string.Join(";", list);
		}

		private string FUN_strSkriptFuerVersion61()
		{
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Pitch", typeof(float).Name, 0);
			return string.Format("Alter Table {0} {1}", "Packages", arg);
		}

		private string FUN_strSkriptFuerVersion62()
		{
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("DefaultProgramId", typeof(long).Name, 0);
			return string.Format("Alter Table {0} {1}", "Machines", arg);
		}

		private string FUN_strSkriptFuerVersion65()
		{
			return string.Format("Alter Table {0} {1}", "SolderingPrograms", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Version", typeof(int).Name, 0));
		}

		private string FUN_strSkriptFuerVersion67()
		{
			return string.Format("Alter Table {0} {1}", "CadSettings", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Data", typeof(string).Name, 0));
		}

		private string FUN_strSkriptFuerVersion69()
		{
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("IsExtern", typeof(bool).Name, 0, i_blnIsErsteSpalte: true);
			string arg2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("code", typeof(string).Name, 200, i_blnIsErsteSpalte: false);
			string arg3 = string.Format("Alter Table {0} {1}, {2}", "Users", arg, arg2);
			return $"{arg3}; {PRO_strBenutzerMappingTabellenString}";
		}

		private string FUN_strSkriptFuerVersion71()
		{
			return $"{PRO_strAoiStepDatenTabellenString}; {PRO_strAoiProgrammTabellenString}; {PRO_strAoiErgebnisTabellenString}";
		}

		private string FUN_strSkriptFuerVersion72()
		{
			return $"{PRO_strLoescheAoiStepDatenTabellenString}; {PRO_strAoiStepDatenTabellenString}; {PRO_strLoescheAoiErgebnisTabellenString}; {PRO_strAoiErgebnisTabellenString}";
		}

		private string FUN_strSkriptFuerVersion73()
		{
			return string.Format("Alter Table {0} {1}", "OperatingMaterial", base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Deleted", typeof(bool).Name, 0));
		}

		private string FUN_strSkriptFuerVersion74()
		{
			return $"{PRO_strFuegeSpaltenZuMeldungenTabelleHinzuString}; {PRO_strMeldungenContextTabellenString};";
		}

		private string FUN_strSkriptFuerVersion76()
		{
			return $"{PRO_strRuestkomponentenTabellenString}; {PRO_strRuestwerkzeugeTabellenString};";
		}

		private string FUN_strSkriptFuerVersion77()
		{
			string text = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Parameter", typeof(string).Name, 200, i_blnIsErsteSpalte: true);
			string text2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("OldValue", typeof(string).Name, 200, i_blnIsErsteSpalte: false);
			string text3 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("NewValue", typeof(string).Name, 200, i_blnIsErsteSpalte: false);
			return string.Format("Alter Table {0} {1}, {2}, {3}", "UserTrackings", text, text2, text3);
		}

		private string FUN_strSkriptFuerVersion78()
		{
			string text = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Sm1Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: true);
			string text2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Fm1Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: false);
			string text3 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Fm2Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: false);
			string text4 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Lm1Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: false);
			string text5 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Lm2Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: false);
			string text6 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("Lm3Active", typeof(bool).Name, 0, i_blnIsErsteSpalte: false);
			string text7 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("OffsetX", typeof(float).Name, 0, i_blnIsErsteSpalte: false);
			string text8 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("OffsetY", typeof(float).Name, 0, i_blnIsErsteSpalte: false);
			string text9 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("OffsetZ", typeof(float).Name, 0, i_blnIsErsteSpalte: false);
			string text10 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("RotationDeg", typeof(float).Name, 0, i_blnIsErsteSpalte: false);
			return string.Format("Alter Table {0} {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", "ProgramPanelParameter", text, text2, text3, text4, text5, text6, text7, text8, text9, text10);
		}

		private string FUN_strSkriptFuerVersion79()
		{
			List<string> list = new List<string>();
			IEnumerable<string> enumerable = from i_strTabellenName in base.PRO_lstTabellenNamen
			where i_strTabellenName.ToLower().StartsWith(EDC_LoetprotokollKopfData.PRO_strTabellennameMitMaschinenPrefix)
			select i_strTabellenName;
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement("Mode", typeof(long).Name, 0);
			foreach (string item in enumerable)
			{
				list.Add($"Alter Table {item} {arg}");
			}
			return string.Join(";", list);
		}

		private string FUN_strSkriptFuerVersion80()
		{
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("ReleaseState", typeof(int).Name, 0, i_blnIsErsteSpalte: true);
			string arg2 = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement("ReleaseNotes", typeof(string).Name, 0, i_blnIsErsteSpalte: false);
			return string.Format("Alter Table {0} {1}, {2}", "ProgramHistory", arg, arg2);
		}

		private string FUN_strSkriptFuerVersion81()
		{
			string arg = base.PRO_edcDatenbankDialekt.FUN_strHoleAlterTableChangeCharacterColumnLength("Username", 50);
			return string.Format("Alter Table {0} {1}", "Users", arg);
		}

		private string FUN_strSkriptFuerVersion82()
		{
			string text = string.Format("Drop Table {0}", "AoiResults");
			string text2 = FUN_strHoleStandardErstellungsStringFuerModell(typeof(EDC_AoiErgebnisData));
			return string.Join(";", text, text2);
		}

		public override void SUB_ErstelleDatenbankUpdateliste()
		{
			base.PRO_lstDatenbankUpdateliste.Clear();
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(22, PRO_strBilderDatenDateinamenSpalte));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(23, PRO_strEcp3DatenBildspaltenDroppen));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(24, PRO_strProduktionssteuerungDataTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(25, PRO_strCodeCacheTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(26, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(27, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(28, PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzuString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(29, PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzu2String));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(30, FUN_strSkriptFuerVersion30()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(31, FUN_strSkriptFuerVersion31()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(32, FUN_strSkriptFuerVersion32()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(33, PRO_strDuesenGeometrieTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(34, PRO_strMaschinenEinstellungenTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(35, FUN_strSkriptErstelleDuesenGeometrienNeu()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(36, PRO_strCodeKonfigurationTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(37, PRO_strFuegeSpaltenZuBenutzerTabelleHinzuString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(38, FUN_strSkriptFuerVersion38()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(39, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(40, FUN_strSkriptErstelleDuesenGeometrienNeu()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(41, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(42, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(43, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(44, PRO_strLoetprogrammNutzenDatenTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(45, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(46, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(47, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(48, PRO_strFuegeMemoSpaltenZuEinstellungenTabelleHinzuString, 35));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(49, PRO_strFuegeSpaltenZuLoetprogrammVersionenDataTabellenHinzuString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(50, PRO_strLoetprogrammVersionValideTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(51, FUN_strSkriptFuerVersion51(), 41));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(52, FUN_strSkriptFuerVersion52(), 48));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(53, FUN_strSkriptFuerVersion53()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(54, PRO_strDuesenSollwerteTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(55, PRO_strFuegeSpaltenZuLoetprogrammBildDataTabellenHinzu3String));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(56, PRO_strFuegeSpalteAlbAusElbZuCodeConfigurationHinzuString, 36));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(57, PRO_strBauteilTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(58, FUN_strSkriptFuerVersion58()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(59, PRO_strFuegeSpaltenZuCodesCacheTabelleHinzuString, 31));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(60, PRO_strFuegeAoiSpaltenZuLoetprogramTabelleHinzuString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(61, FUN_strSkriptFuerVersion61(), 58));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(62, FUN_strSkriptFuerVersion62()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(63, PRO_strBauteilMakroTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(64, PRO_strNutzenDataTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(65, FUN_strSkriptFuerVersion65()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(66, string.Empty));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(67, FUN_strSkriptFuerVersion67(), 53));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(68, PRO_strBetriebsmittelDataTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(69, FUN_strSkriptFuerVersion69()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(70, PRO_strFuegeSpalteMaschinenIdCodeCacheHinzuString, 31));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(71, FUN_strSkriptFuerVersion71()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(72, FUN_strSkriptFuerVersion72()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(73, FUN_strSkriptFuerVersion73(), 68));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(74, FUN_strSkriptFuerVersion74()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(75, PRO_strLinkCacheTabellenString));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(76, FUN_strSkriptFuerVersion76()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(77, FUN_strSkriptFuerVersion77()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(78, FUN_strSkriptFuerVersion78(), 45));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(79, FUN_strSkriptFuerVersion79()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(80, FUN_strSkriptFuerVersion80()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(81, FUN_strSkriptFuerVersion81()));
			base.PRO_lstDatenbankUpdateliste.Add(new EDC_UpdateInformation(82, FUN_strSkriptFuerVersion82()));
		}

		private void SUB_ErstelleMigrationsliste()
		{
			base.PRO_dicDatenbankMigrationliste.Clear();
			base.PRO_dicDatenbankMigrationliste.Add(22, FUN_fdcMigriereEcp3BilddatenInBildertabelleAsync);
			base.PRO_dicDatenbankMigrationliste.Add(38, FUN_fdcMigrationFuerVersion38Async);
			base.PRO_dicDatenbankMigrationliste.Add(49, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcMigriereVersionenTabelleAsync(i_edcAdapter));
			base.PRO_dicDatenbankMigrationliste.Add(69, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcMigriereBenutzerMappingTabellenAsync(i_edcAdapter));
			base.PRO_dicDatenbankMigrationliste.Add(74, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcSetzeDefaultWerteFuerExterneMeldungenAsync(i_edcAdapter));
			base.PRO_dicDatenbankMigrationliste.Add(75, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcSetzeProgrammVersionenDefaultWerteAsync(i_edcAdapter));
			base.PRO_dicDatenbankMigrationliste.Add(79, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcSetzeModusWerteInKopfdatenAsync(i_edcAdapter));
			base.PRO_dicDatenbankMigrationliste.Add(80, (INF_DatenbankAdapter i_edcAdapter) => FUN_fdcSetzeReleaseStateWertInLoetprogrammVersionAsync(i_edcAdapter));
		}

		private Task FUN_fdcMigrationFuerVersion38Async(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return Task.Factory.StartNew(delegate
			{
				string i_strSql = EDC_DuesenData.FUN_strErstelleDefaultGeometrieIdUpdateStatement(0L);
				i_edcDatenbankAdapter.SUB_ExecuteStatement(i_strSql);
			});
		}

		private Task FUN_fdcMigriereEcp3BilddatenInBildertabelleAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return Task.Factory.StartNew(delegate
			{
				string i_strSql = string.Format("select distinct h1.{4}, e1.{2}, e1.{3} from {0} e1 inner join {1} h1 on e1.{5} = h1.{6} where (not (e1.{2} is null)) and h1.{7} = (select max(h2.{7}) from {0} e2 inner join {1} h2 on e2.{5} = h2.{6} where h1.{4} = h2.{4})", "ProgramEcp3Data", "ProgramHistory", "FileContent", "FileName", "ProgramId", "HistoryId", "HistoryId", "Version");
				DataTable dataTable = i_edcDatenbankAdapter.FUN_fdcLeseInDataTable(i_strSql, "BildAbfrage");
				if (dataTable != null && dataTable.Rows.Count != 0)
				{
					IDbTransaction dbTransaction = i_edcDatenbankAdapter.FUN_fdcStarteTransaktion();
					try
					{
						foreach (DataRow row in dataTable.Rows)
						{
							EDC_LoetprogrammBildData eDC_LoetprogrammBildData = new EDC_LoetprogrammBildData
							{
								PRO_i32Verwendung = 3
							};
							if (row["FileContent"] != DBNull.Value)
							{
								eDC_LoetprogrammBildData.PRO_bytBild = (byte[])row["FileContent"];
							}
							if (row["ProgramId"] != DBNull.Value)
							{
								eDC_LoetprogrammBildData.PRO_i64ProgrammId = (long)row["ProgramId"];
							}
							if (row["FileName"] != DBNull.Value)
							{
								eDC_LoetprogrammBildData.PRO_strDateiname = (string)row["FileName"];
							}
							i_edcDatenbankAdapter.SUB_SpeichereObjekt(eDC_LoetprogrammBildData, dbTransaction);
						}
						i_edcDatenbankAdapter.SUB_CommitTransaktion(dbTransaction);
					}
					catch (Exception)
					{
						if (dbTransaction != null)
						{
							i_edcDatenbankAdapter.SUB_RollbackTransaktion(dbTransaction);
						}
						throw;
					}
				}
			});
		}

		private Task FUN_fdcMigriereVersionenTabelleAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return new EDC_LoetprogrammVersionDataAccess(i_edcDatenbankAdapter).FUN_fdcFuehreDatenMigrationDurchAsync();
		}

		private Task FUN_fdcMigriereBenutzerMappingTabellenAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return new EDC_BenutzerVerwaltungDataAccess(i_edcDatenbankAdapter).FUN_fdcFuehreTabellenMappingMigrationDurchAsync();
		}

		private Task FUN_fdcSetzeProgrammVersionenDefaultWerteAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return new EDC_LoetprogrammProgrammDataAccess(i_edcDatenbankAdapter).FUN_fdcSetzeDefaultProgrammVersionsNummerAsync();
		}

		private Task FUN_fdcSetzeDefaultWerteFuerExterneMeldungenAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return new EDC_MeldungenDataAccess(i_edcDatenbankAdapter).FUN_fdcMigriereDefaultWerteFuerExterneMeldungenAsync();
		}

		private async Task FUN_fdcSetzeModusWerteInKopfdatenAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			IEnumerable<string> enumerable = from i_strTabellenName in PRO_lstTabellenNamen
			where i_strTabellenName.ToLower().StartsWith(EDC_LoetprotokollKopfData.PRO_strTabellennameMitMaschinenPrefix)
			select i_strTabellenName;
			foreach (string item in enumerable)
			{
				await i_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(string.Format("Update {0} set {1} = 0", item, "Mode")).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private Task FUN_fdcSetzeReleaseStateWertInLoetprogrammVersionAsync(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			return new EDC_LoetprogrammVersionDataAccess(i_edcDatenbankAdapter).FUN_fdcFuehreReleaseStateDatenMigrationDurchAsync();
		}
	}
}
