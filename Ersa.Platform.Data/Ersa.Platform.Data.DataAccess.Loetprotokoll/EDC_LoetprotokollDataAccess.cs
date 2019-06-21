using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Filter;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Data.Loetprotokoll;
using Ersa.Platform.Common.Exceptions;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprotokoll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprotokoll
{
	public class EDC_LoetprotokollDataAccess : EDC_DataAccess, INF_LoetprotokollDataAccess, INF_DataAccess
	{
		private const string mC_strParameterTyp = "P";

		private const string mC_strCodeTyp = "C";

		private Dictionary<string, Tuple<string, string>> m_dicRegistrierteVariablen = new Dictionary<string, Tuple<string, string>>();

		private Dictionary<string, Tuple<string, string>> m_dicSpaltenZuordnung = new Dictionary<string, Tuple<string, string>>();

		public EDC_LoetprotokollDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcRegistriereProtokollVariablenAsync(IEnumerable<EDC_LoetprotokollVariablenData> i_lstVariablen)
		{
			List<EDC_LoetprotokollVariablenData> lstAlleVariablen = i_lstVariablen.ToList();
			if (lstAlleVariablen.Any())
			{
				long i64MaschinenId = lstAlleVariablen.First().PRO_i64MaschinenId;
				string i_strWhereStatement = EDC_LoetprotokollVariablenData.FUN_strMaschinenVariablenWhereStatementErstellen(i64MaschinenId);
				List<EDC_LoetprotokollVariablenData> lstAlleRegistrietenVariablen = (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprotokollVariablenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				List<EDC_LoetprotokollVariablenData> lstNichtRegistrierteVariablen = lstAlleVariablen.Except(lstAlleRegistrietenVariablen).ToList();
				if (lstNichtRegistrierteVariablen.Any())
				{
					await FUN_edcLegeDieNeuenVariablenAnAsync(i64MaschinenId, lstNichtRegistrierteVariablen).ConfigureAwait(continueOnCapturedContext: false);
					lstAlleRegistrietenVariablen.AddRange(lstNichtRegistrierteVariablen);
					SUB_ErweitereDieDatenTabelle(i64MaschinenId, lstAlleRegistrietenVariablen, lstNichtRegistrierteVariablen);
					SUB_ErstelleDieKopfTabelle(i64MaschinenId);
					SUB_ErstelleDieParameterTabelle(i64MaschinenId);
				}
				await FUN_edcLadeRegistrierteVariablenAsync(i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcFuegeProtokollEintragHinzuAsync(EDC_LoetprotokollKopfData i_edcProtokollKopf, IEnumerable<EDC_LoetprotokollVariablenData> i_enuVariablen, Dictionary<string, string> i_dicCodes = null, Dictionary<string, string> i_dicParameter = null)
		{
			uint i64ProtokollId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			i_edcProtokollKopf.PRO_i64ProtokollId = i64ProtokollId;
			List<EDC_LoetprotokollVariablenData> lstVariablen = i_enuVariablen.ToList();
			EDC_LoetprotokollData edcModell = await FUN_edcErstelleProtokollModellAsync(i64ProtokollId, lstVariablen).ConfigureAwait(continueOnCapturedContext: false);
			long i64MaschinenId = lstVariablen.First().PRO_i64MaschinenId;
			if (i64MaschinenId == 0L)
			{
				throw new ArgumentException("The machineId must be set for each registered parameter", "i_enuVariablen");
			}
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcProtokollKopf, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcModell, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcFuegeProtokollParameterUndCodesHinzuAsync(i64ProtokollId, i64MaschinenId, i_dicParameter, i_dicCodes, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
			return i64ProtokollId;
		}

		public async Task<IEnumerable<STRUCT_FilterBasisDefinition>> FUN_enuHoleMoeglicheFilterFuerLoetprotokollAbfragenAsync(long i_i64MaschinenId)
		{
			Task<Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>>> task = FUN_edcLadeRegistrierteProtokollVariablenAsync(new List<long>
			{
				i_i64MaschinenId
			});
			Dictionary<PropertyInfo, EDC_FilterInformationen> source = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleFilterInformationen(typeof(EDC_LoetprotokollAbfrageData));
			List<STRUCT_FilterBasisDefinition> lstSttFilterBasisDefinitionen = source.Select(delegate(KeyValuePair<PropertyInfo, EDC_FilterInformationen> i_fdcPaar)
			{
				STRUCT_FilterBasisDefinition result2 = default(STRUCT_FilterBasisDefinition);
				result2.PRO_strSpaltenName = i_fdcPaar.Value.PRO_strSpaltenName;
				result2.PRO_strDatentyp = i_fdcPaar.Key.PropertyType.Name;
				result2.PRO_lstFilterOperationen = EDC_FilterErstellungsHelfer.FUN_enuKonvertiereStringOperatoernZuEnumOperatoren(i_fdcPaar.Value.PRO_strFilterOperationen);
				result2.PRO_lstFilterVerknuepfungen = EDC_FilterErstellungsHelfer.FUN_enuKonvertiereStringVerknuepfungenZuEnumVerknuepfungen(i_fdcPaar.Value.PRO_strFilterVerknuepfungen);
				result2.PRO_lstWerteListe = i_fdcPaar.Value.PRO_lstWerteListe;
				result2.PRO_objMinimalWert = i_fdcPaar.Value.PRO_objMinimalWert;
				result2.PRO_objMaximalWert = i_fdcPaar.Value.PRO_objMaximalWert;
				result2.PRO_strAnzeigeNameKey = i_fdcPaar.Value.PRO_strFilterAnzeigeNameKey;
				result2.PRO_strKategorieNameKey = i_fdcPaar.Value.PRO_strFilterKategorieNameKey;
				result2.PRO_strTabellenname = (("Users".Equals(i_fdcPaar.Value.PRO_strTabellenName) || "SolderingPrograms".Equals(i_fdcPaar.Value.PRO_strTabellenName) || "SolderingLibraries".Equals(i_fdcPaar.Value.PRO_strTabellenName)) ? i_fdcPaar.Value.PRO_strTabellenName : $"{i_fdcPaar.Value.PRO_strTabellenName}_MA{i_i64MaschinenId}");
				return result2;
			}).ToList();
			if ((await task.ConfigureAwait(continueOnCapturedContext: false)).TryGetValue(i_i64MaschinenId, out IEnumerable<EDC_LoetprotokollVariablenData> value))
			{
				lstSttFilterBasisDefinitionen.AddRange(value.Select(delegate(EDC_LoetprotokollVariablenData i_edcVariable)
				{
					STRUCT_FilterBasisDefinition result = default(STRUCT_FilterBasisDefinition);
					result.PRO_strKategorieNameKey = "Parameter";
					result.PRO_strAnzeigeNameKey = (i_edcVariable.PRO_strVariable ?? string.Empty);
					result.PRO_strDatentyp = (i_edcVariable.PRO_strDatentyp ?? string.Empty);
					result.PRO_strSpaltenName = (i_edcVariable.PRO_strSpaltenName ?? string.Empty);
					result.PRO_strTabellenname = string.Format("{0}_MA{1}", "ProtocolData", i_i64MaschinenId);
					result.PRO_lstFilterVerknuepfungen = EDC_FilterErstellungsHelfer.FUN_enuKonvertiereStringVerknuepfungenZuEnumVerknuepfungen(i_edcVariable.PRO_strDatentyp ?? string.Empty);
					result.PRO_lstFilterOperationen = EDC_FilterErstellungsHelfer.FUN_enuErstelleOperatorenListe("or|and");
					return result;
				}));
			}
			return lstSttFilterBasisDefinitionen;
		}

		public async Task<IEnumerable<EDC_LoetprotokollAbfrageErgebnis>> FUN_fdcFuehreLoetprotokollAbfrageAusAsync(IEnumerable<STRUCT_FilterKonkret> i_enuAbfrageFilter, IEnumerable<STRUCT_FilterBasisDefinition> i_enuVariablen, long i_i64MaschinenId, long i_i64MaschinenGruppenId, INF_LoetParameterIstSollConverter i_edcIstSollConverter)
		{
			List<EDC_DynamischeSpalte> i_blnEdcDynamischeSpalten = FUN_lstErzeugeDynamischeSpalten(i_enuVariablen);
			Stopwatch fdcStop = Stopwatch.StartNew();
			Task<Dictionary<long, Dictionary<string, EDC_LoetprotokollVariablenData>>> task = FUN_edcLadeRegistrierteProtokollVariablenInDictionaryAsync(new long[1]
			{
				i_i64MaschinenId
			});
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strWhereStatement = EDC_LoetprotokollAbfrageData.FUN_strWhereUndOrderByStatementMitListeFilterKonkretErstellen(i_enuAbfrageFilter, dictionary);
			Task<IEnumerable<EDC_LoetprotokollAbfrageData>> fdcAbfrageTask = m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprotokollAbfrageData(i_i64MaschinenId, i_blnEdcDynamischeSpalten, i_strWhereStatement), null, dictionary);
			Dictionary<string, EDC_LoetprotokollVariablenData> dicRegistrierteVariablen = (await task.ConfigureAwait(continueOnCapturedContext: false))[i_i64MaschinenId];
			IEnumerable<EDC_LoetprotokollAbfrageData> enuAbfrageData;
			try
			{
				enuAbfrageData = await fdcAbfrageTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				EDC_LoetprotokollKopfData eDC_LoetprotokollKopfData = new EDC_LoetprotokollKopfData(i_i64MaschinenId);
				if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_LoetprotokollKopfData.PRO_strTabellenname))
				{
					throw new EDC_KeineLoetprotokollTabellenException(ex.Message);
				}
				throw;
			}
			finally
			{
				fdcStop.Stop();
			}
			return await FUN_fdcFuegeParameterUndVersionsInfoHinzuAsync(i_i64MaschinenId, i_i64MaschinenGruppenId, enuAbfrageData, dicRegistrierteVariablen, i_edcIstSollConverter).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_LoetprotokollAbfrageErgebnis> FUN_fdcHoleLoetprotokollMitProtokollIdAsync(long i_i64ProtokollId, long i_i64MaschinenId, long i_i64MaschinenGruppenId, INF_LoetParameterIstSollConverter i_edcIstSollConverter)
		{
			Stopwatch fdcStop = Stopwatch.StartNew();
			Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>> obj = await FUN_edcLadeRegistrierteProtokollVariablenAsync(new List<long>
			{
				i_i64MaschinenId
			}).ConfigureAwait(continueOnCapturedContext: false);
			List<STRUCT_FilterBasisDefinition> list = new List<STRUCT_FilterBasisDefinition>();
			if (obj.TryGetValue(i_i64MaschinenId, out IEnumerable<EDC_LoetprotokollVariablenData> value))
			{
				list.AddRange(value.Select(delegate(EDC_LoetprotokollVariablenData i_edcVariable)
				{
					STRUCT_FilterBasisDefinition result = default(STRUCT_FilterBasisDefinition);
					result.PRO_strKategorieNameKey = "Parameter";
					result.PRO_strAnzeigeNameKey = (i_edcVariable.PRO_strVariable ?? string.Empty);
					result.PRO_strDatentyp = (i_edcVariable.PRO_strDatentyp ?? string.Empty);
					result.PRO_strSpaltenName = (i_edcVariable.PRO_strSpaltenName ?? string.Empty);
					result.PRO_strTabellenname = string.Format("{0}_MA{1}", "ProtocolData", i_i64MaschinenId);
					result.PRO_lstFilterVerknuepfungen = EDC_FilterErstellungsHelfer.FUN_enuKonvertiereStringVerknuepfungenZuEnumVerknuepfungen(i_edcVariable.PRO_strDatentyp ?? string.Empty);
					result.PRO_lstFilterOperationen = EDC_FilterErstellungsHelfer.FUN_enuErstelleOperatorenListe("or|and");
					return result;
				}));
			}
			List<EDC_DynamischeSpalte> i_blnEdcDynamischeSpalten = FUN_lstErzeugeDynamischeSpalten(list);
			Task<Dictionary<long, Dictionary<string, EDC_LoetprotokollVariablenData>>> task = FUN_edcLadeRegistrierteProtokollVariablenInDictionaryAsync(new long[1]
			{
				i_i64MaschinenId
			});
			string i_strWhereStatement = EDC_LoetprotokollAbfrageData.FUN_strProtokollIdWhereStatementErstellen(i_i64ProtokollId, i_i64MaschinenId);
			Task<IEnumerable<EDC_LoetprotokollAbfrageData>> fdcAbfrageTask = m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprotokollAbfrageData(i_i64MaschinenId, i_blnEdcDynamischeSpalten, i_strWhereStatement));
			Dictionary<string, EDC_LoetprotokollVariablenData> dicRegistrierteVariablen = (await task.ConfigureAwait(continueOnCapturedContext: false))[i_i64MaschinenId];
			IEnumerable<EDC_LoetprotokollAbfrageData> enuAbfrageData;
			try
			{
				enuAbfrageData = await fdcAbfrageTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				EDC_LoetprotokollKopfData eDC_LoetprotokollKopfData = new EDC_LoetprotokollKopfData(i_i64MaschinenId);
				if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_LoetprotokollKopfData.PRO_strTabellenname))
				{
					throw new EDC_KeineLoetprotokollTabellenException(ex.Message);
				}
				throw;
			}
			finally
			{
				fdcStop.Stop();
			}
			List<EDC_LoetprotokollAbfrageErgebnis> source = (await FUN_fdcFuegeParameterUndVersionsInfoHinzuAsync(i_i64MaschinenId, i_i64MaschinenGruppenId, enuAbfrageData, dicRegistrierteVariablen, i_edcIstSollConverter).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			if (source.Any())
			{
				return source.First();
			}
			return null;
		}

		public async Task<Dictionary<long, IEnumerable<EDC_LoetprogrammParameterData>>> FUN_fdcLadeProgrammParameterSollwerteZuVersionenAsync(IEnumerable<long> i_lst64VersionsId)
		{
			Dictionary<long, IEnumerable<EDC_LoetprogrammParameterData>> dicProgrammData = new Dictionary<long, IEnumerable<EDC_LoetprogrammParameterData>>();
			long[] a_i64VersionsId = (i_lst64VersionsId as long[]) ?? i_lst64VersionsId.ToArray();
			string i_strWhereStatement = EDC_LoetprogrammParameterData.FUN_strVersionsIdWhereStatementErstellen(a_i64VersionsId);
			List<EDC_LoetprogrammParameterData> source = (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammParameterData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			long[] array = a_i64VersionsId;
			foreach (long i64Id in array)
			{
				List<EDC_LoetprogrammParameterData> value = (from i_edcParameter in source
				where i_edcParameter.PRO_i64VersionsId == i64Id
				select i_edcParameter).ToList();
				dicProgrammData.Add(i64Id, value);
			}
			return dicProgrammData;
		}

		public async Task<Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>>> FUN_edcLadeRegistrierteProtokollVariablenAsync(IEnumerable<long> i_lst64MaschinenId)
		{
			Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>> dicRegistrieteVariablen = new Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>>();
			long[] a_i64MachinenId = (i_lst64MaschinenId as long[]) ?? i_lst64MaschinenId.ToArray();
			string i_strWhereStatement = EDC_LoetprotokollVariablenData.FUN_strMaschinenVariablenWhereStatementErstellen(a_i64MachinenId);
			List<EDC_LoetprotokollVariablenData> source = (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprotokollVariablenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			long[] array = a_i64MachinenId;
			foreach (long i64Id in array)
			{
				List<EDC_LoetprotokollVariablenData> value = (from i_edcVariable in source
				where i_edcVariable.PRO_i64MaschinenId == i64Id
				select i_edcVariable).ToList();
				dicRegistrieteVariablen.Add(i64Id, value);
			}
			return dicRegistrieteVariablen;
		}

		private static void SUB_BehandleParameterUndCodeDaten(EDC_LoetprotokollAbfrageData i_edcAbfrageData, Dictionary<string, string> i_dicProtokollParameter, Dictionary<string, List<string>> i_dicProtokollCodes)
		{
			string pRO_strParameter = i_edcAbfrageData.PRO_strParameter;
			string pRO_strTyp = i_edcAbfrageData.PRO_strTyp;
			if (!(pRO_strTyp == "P"))
			{
				if (pRO_strTyp == "C" && !string.IsNullOrEmpty(pRO_strParameter) && i_dicProtokollCodes != null)
				{
					List<string> value = string.IsNullOrEmpty(i_edcAbfrageData.PRO_strInhalt) ? new List<string>() : i_edcAbfrageData.PRO_strInhalt.Split(new char[1]
					{
						'|'
					}, StringSplitOptions.RemoveEmptyEntries).ToList();
					i_dicProtokollCodes.Add(pRO_strParameter, value);
				}
			}
			else if (!string.IsNullOrEmpty(pRO_strParameter))
			{
				i_dicProtokollParameter?.Add(pRO_strParameter, i_edcAbfrageData.PRO_strInhalt);
			}
		}

		private static List<EDC_DynamischeSpalte> FUN_lstErzeugeDynamischeSpalten(IEnumerable<STRUCT_FilterBasisDefinition> i_enuVariablen)
		{
			return (from i_sttVariable in i_enuVariablen
			select new EDC_DynamischeSpalte
			{
				PRO_strSpaltenName = i_sttVariable.PRO_strSpaltenName,
				PRO_strDatenTyp = i_sttVariable.PRO_strDatentyp
			}).ToList();
		}

		private static void SUB_FuelleLoetprotokollAbfrageErgebnis(long i_i64MaschinenId, EDC_LoetprotokollAbfrageErgebnis i_edcAbfrageErgebnis, EDC_LoetprotokollAbfrageData i_edcAbfrageData, Dictionary<string, EDC_LoetprotokollVariablenData> i_lstRegistrierteVariablen)
		{
			i_edcAbfrageErgebnis.PRO_i64ProtokollId = i_edcAbfrageData.PRO_i64ProtokollId;
			i_edcAbfrageErgebnis.PRO_i64LoetprogrammVersionsId = i_edcAbfrageData.PRO_i64LoetprogrammVersionsId;
			i_edcAbfrageErgebnis.PRO_blnArbeitsversion = i_edcAbfrageData.PRO_blnArbeitsversion;
			i_edcAbfrageErgebnis.PRO_dtmEingangszeit = i_edcAbfrageData.PRO_dtmEingangszeit;
			i_edcAbfrageErgebnis.PRO_dtmAusgangszeit = i_edcAbfrageData.PRO_dtmAusgangszeit;
			i_edcAbfrageErgebnis.PRO_strBenutzerName = i_edcAbfrageData.PRO_strBenutzername;
			i_edcAbfrageErgebnis.PRO_i64LaufendeNummer = i_edcAbfrageData.PRO_i64LaufendeNummer;
			i_edcAbfrageErgebnis.PRO_blnLoetgutSchlecht = i_edcAbfrageData.PRO_blnLoetgutSchlecht;
			i_edcAbfrageErgebnis.PRO_strBibliotheksName = i_edcAbfrageData.PRO_strBibliothekName;
			i_edcAbfrageErgebnis.PRO_strLoetprogrammName = i_edcAbfrageData.PRO_strProgrammName;
			i_edcAbfrageErgebnis.PRO_enmModus = (ENUM_LoetprogrammModus)i_edcAbfrageData.PRO_i64Modus;
			List<EDC_LoetprotokollSollIstVariable> list = new List<EDC_LoetprotokollSollIstVariable>();
			foreach (EDC_DynamischeSpalte item in i_edcAbfrageData.PRO_lstDynamischeSpalten)
			{
				if (item.PRO_objWert != null)
				{
					EDC_LoetprotokollSollIstVariable eDC_LoetprotokollSollIstVariable = new EDC_LoetprotokollSollIstVariable
					{
						PRO_strProtokollSpaltenName = item.PRO_strSpaltenName,
						PRO_strDatentyp = item.PRO_strDatenTyp,
						PRO_objIstwert = item.PRO_objWert,
						PRO_i64MaschinenId = i_i64MaschinenId
					};
					i_lstRegistrierteVariablen.TryGetValue(item.PRO_strSpaltenName, out EDC_LoetprotokollVariablenData value);
					if (value != null)
					{
						eDC_LoetprotokollSollIstVariable.PRO_strIstVariable = value.PRO_strVariable;
						eDC_LoetprotokollSollIstVariable.PRO_strEinheitenKey = value.PRO_strEinheitKey;
						eDC_LoetprotokollSollIstVariable.PRO_strNameKey = value.PRO_strNameKeyArray;
					}
					list.Add(eDC_LoetprotokollSollIstVariable);
				}
			}
			i_edcAbfrageErgebnis.PRO_enuVariablenListe = list;
		}

		private async Task FUN_edcLadeRegistrierteVariablenAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_LoetprotokollVariablenData.FUN_strMaschinenVariablenWhereStatementErstellen(i_i64MaschinenId);
			List<EDC_LoetprotokollVariablenData> i_lstVariablen = (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprotokollVariablenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			SUB_ErstelleVariablenDictionaries(i_lstVariablen, out m_dicRegistrierteVariablen, out m_dicSpaltenZuordnung);
		}

		private void SUB_ErstelleVariablenDictionaries(IEnumerable<EDC_LoetprotokollVariablenData> i_lstVariablen, out Dictionary<string, Tuple<string, string>> i_dicRegistrierteVariablen, out Dictionary<string, Tuple<string, string>> i_dicSpaltenzuordnung)
		{
			i_dicRegistrierteVariablen = new Dictionary<string, Tuple<string, string>>();
			i_dicSpaltenzuordnung = new Dictionary<string, Tuple<string, string>>();
			foreach (EDC_LoetprotokollVariablenData item2 in i_lstVariablen)
			{
				string item = item2.PRO_strEinheitKey ?? string.Empty;
				i_dicRegistrierteVariablen.Add(item2.PRO_strVariable, new Tuple<string, string>(item2.PRO_strSpaltenName, item));
				i_dicSpaltenzuordnung.Add(item2.PRO_strSpaltenName, new Tuple<string, string>(item2.PRO_strVariable, item));
			}
		}

		private void SUB_ErgaenzeDieDynamischenSpaltenNamen(IEnumerable<EDC_LoetprotokollVariablenData> i_lstVariablen)
		{
			foreach (EDC_LoetprotokollVariablenData item in i_lstVariablen)
			{
				if (m_dicRegistrierteVariablen.TryGetValue(item.PRO_strVariable, out Tuple<string, string> value))
				{
					item.PRO_strSpaltenName = value.Item1;
					item.PRO_strEinheitKey = value.Item2;
				}
			}
		}

		private async Task FUN_edcLegeDieNeuenVariablenAnAsync(long i_i64MaschinenId, IEnumerable<EDC_LoetprotokollVariablenData> i_lstNeueVariablen)
		{
			long num = await FUN_i64HoleDieHoechsteVariablenNummerAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false) + 1;
			List<EDC_LoetprotokollVariablenData> list = i_lstNeueVariablen.ToList();
			foreach (EDC_LoetprotokollVariablenData item in list)
			{
				item.PRO_i64MaschinenId = i_i64MaschinenId;
				item.PRO_strSpaltenName = string.Format("c{0}", num.ToString("D3"));
				num++;
			}
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(list).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcFuegeProtokollParameterUndCodesHinzuAsync(long i_i64ProtokollVersion, long i_i64MaschinenId, Dictionary<string, string> i_dicParameter, Dictionary<string, string> i_dicCodes, IDbTransaction i_fdcTransaktion)
		{
			IEnumerable<EDC_LoetprotokollParameterData> first = (i_dicParameter != null) ? (from i_fdcKeyValuePair in i_dicParameter
			select new EDC_LoetprotokollParameterData
			{
				PRO_i64ProtokollId = i_i64ProtokollVersion,
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_strParameter = i_fdcKeyValuePair.Key,
				PRO_strInhalt = i_fdcKeyValuePair.Value,
				PRO_strTyp = "P"
			}) : Enumerable.Empty<EDC_LoetprotokollParameterData>();
			IEnumerable<EDC_LoetprotokollParameterData> second = (i_dicCodes != null) ? (from i_fdcKeyValuePair in i_dicCodes
			select new EDC_LoetprotokollParameterData
			{
				PRO_i64ProtokollId = i_i64ProtokollVersion,
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_strParameter = i_fdcKeyValuePair.Key,
				PRO_strInhalt = i_fdcKeyValuePair.Value,
				PRO_strTyp = "C"
			}) : Enumerable.Empty<EDC_LoetprotokollParameterData>();
			List<EDC_LoetprotokollParameterData> list = first.Union(second).ToList();
			if (list.Any())
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(list, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task<EDC_LoetprotokollData> FUN_edcErstelleProtokollModellAsync(long i_i64ProtokollId, IEnumerable<EDC_LoetprotokollVariablenData> i_lstVariablen)
		{
			List<EDC_LoetprotokollVariablenData> lstVariablen = i_lstVariablen.ToList();
			if (!lstVariablen.Any())
			{
				return null;
			}
			long i64MaschinenId = lstVariablen.First().PRO_i64MaschinenId;
			if (!m_dicRegistrierteVariablen.Any())
			{
				await FUN_edcLadeRegistrierteVariablenAsync(i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
				if (!m_dicRegistrierteVariablen.Any())
				{
					throw new ArgumentNullException("i_lstVariablen", "The variables must first be registered before they can be handeld with the database");
				}
			}
			SUB_ErgaenzeDieDynamischenSpaltenNamen(lstVariablen);
			EDC_LoetprotokollData eDC_LoetprotokollData = FUN_edcErstelleModellobjektFuerDatenTabelleAusVariablen(i64MaschinenId, lstVariablen);
			eDC_LoetprotokollData.PRO_i64ProtokollId = i_i64ProtokollId;
			return eDC_LoetprotokollData;
		}

		private void SUB_ErstelleDieKopfTabelle(long i_i64MaschinenId)
		{
			EDC_LoetprotokollKopfData eDC_LoetprotokollKopfData = FUN_edcErstelleModellobjektFuerKopfTabelle(i_i64MaschinenId);
			if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_LoetprotokollKopfData.PRO_strTabellenname))
			{
				m_edcDatenbankAdapter.SUB_ErstelleEineZusatztabelle(eDC_LoetprotokollKopfData);
			}
		}

		private void SUB_ErstelleDieParameterTabelle(long i_i64MaschinenId)
		{
			EDC_LoetprotokollParameterData eDC_LoetprotokollParameterData = FUN_edcErstelleModellobjektFuerParameterTabelle(i_i64MaschinenId);
			if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_LoetprotokollParameterData.PRO_strTabellenname))
			{
				m_edcDatenbankAdapter.SUB_ErstelleEineZusatztabelle(eDC_LoetprotokollParameterData);
			}
		}

		private void SUB_ErweitereDieDatenTabelle(long i_i64MaschinenId, IEnumerable<EDC_LoetprotokollVariablenData> i_lstAlleVariablen, IEnumerable<EDC_LoetprotokollVariablenData> i_lstNeueVariablen)
		{
			EDC_LoetprotokollData eDC_LoetprotokollData = FUN_edcErstelleModellobjektFuerDatenTabelleAusVariablen(i_i64MaschinenId, i_lstAlleVariablen);
			if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_LoetprotokollData.PRO_strTabellenname))
			{
				m_edcDatenbankAdapter.SUB_ErstelleEineZusatztabelle(eDC_LoetprotokollData);
				return;
			}
			List<EDC_DynamischeSpalte> i_lstSpalten = (from i_edcVariable in i_lstNeueVariablen
			select new EDC_DynamischeSpalte(i_edcVariable.PRO_strSpaltenName, i_edcVariable.PRO_strDatentyp, i_edcVariable.PRO_objWert)).ToList();
			m_edcDatenbankAdapter.SUB_FuegeTabellenSpaltenHinzu(eDC_LoetprotokollData.PRO_strTabellenname, i_lstSpalten);
		}

		private EDC_LoetprotokollKopfData FUN_edcErstelleModellobjektFuerKopfTabelle(long i_i64MaschinenId)
		{
			return new EDC_LoetprotokollKopfData(i_i64MaschinenId);
		}

		private EDC_LoetprotokollParameterData FUN_edcErstelleModellobjektFuerParameterTabelle(long i_i64MaschinenId)
		{
			return new EDC_LoetprotokollParameterData(i_i64MaschinenId);
		}

		private EDC_LoetprotokollData FUN_edcErstelleModellobjektFuerDatenTabelleAusVariablen(long i_i64MaschinenId, IEnumerable<EDC_LoetprotokollVariablenData> i_lstAlleVariablen)
		{
			EDC_LoetprotokollData eDC_LoetprotokollData = new EDC_LoetprotokollData(i_i64MaschinenId);
			List<EDC_DynamischeSpalte> list = (List<EDC_DynamischeSpalte>)(eDC_LoetprotokollData.PRO_lstDynamischeSpalten = (from i_edcVariable in i_lstAlleVariablen
			select new EDC_DynamischeSpalte(i_edcVariable.PRO_strSpaltenName, i_edcVariable.PRO_strDatentyp, i_edcVariable.PRO_objWert)).ToList());
			return eDC_LoetprotokollData;
		}

		private async Task<long> FUN_i64HoleDieHoechsteVariablenNummerAsync(long i_i64MaschinenId)
		{
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(EDC_LoetprotokollVariablenData.FUN_strSelectMaxSpaltennummerFuerMaschinenIdErstellen(i_i64MaschinenId)).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return 0L;
			}
			return Convert.ToInt64(obj);
		}

		private async Task<IEnumerable<EDC_LoetprotokollAbfrageErgebnis>> FUN_fdcFuegeParameterUndVersionsInfoHinzuAsync(long i_i64MaschinenId, long i_i64MaschinenGruppenId, IEnumerable<EDC_LoetprotokollAbfrageData> i_enuAbfrageData, Dictionary<string, EDC_LoetprotokollVariablenData> i_dicRegistrierteVariablen, INF_LoetParameterIstSollConverter i_edcIstSollConverter)
		{
			Stopwatch fdcStop = Stopwatch.StartNew();
			List<EDC_LoetprotokollAbfrageErgebnis> edcAbfrageErgebnisse = new List<EDC_LoetprotokollAbfrageErgebnis>();
			List<long> list = new List<long>();
			EDC_LoetprotokollAbfrageData[] array = (i_enuAbfrageData as EDC_LoetprotokollAbfrageData[]) ?? i_enuAbfrageData.ToArray();
			if (array.Any())
			{
				EDC_LoetprotokollAbfrageData[] array2 = array;
				foreach (EDC_LoetprotokollAbfrageData edcAbfrageData in array2)
				{
					List<EDC_LoetprotokollAbfrageErgebnis> source = (from i_edcErgebnis in edcAbfrageErgebnisse
					where i_edcErgebnis.PRO_i64ProtokollId == edcAbfrageData.PRO_i64ProtokollId
					select i_edcErgebnis).ToList();
					EDC_LoetprotokollAbfrageErgebnis eDC_LoetprotokollAbfrageErgebnis;
					if (!source.Any())
					{
						eDC_LoetprotokollAbfrageErgebnis = new EDC_LoetprotokollAbfrageErgebnis
						{
							PRO_strProtokollParameter = new Dictionary<string, string>(),
							PRO_strProtokollCodes = new Dictionary<string, List<string>>()
						};
						edcAbfrageErgebnisse.Add(eDC_LoetprotokollAbfrageErgebnis);
						SUB_FuelleLoetprotokollAbfrageErgebnis(i_i64MaschinenId, eDC_LoetprotokollAbfrageErgebnis, edcAbfrageData, i_dicRegistrierteVariablen);
					}
					else
					{
						eDC_LoetprotokollAbfrageErgebnis = source.First();
					}
					SUB_BehandleParameterUndCodeDaten(edcAbfrageData, eDC_LoetprotokollAbfrageErgebnis.PRO_strProtokollParameter, eDC_LoetprotokollAbfrageErgebnis.PRO_strProtokollCodes);
					if (!list.Contains(edcAbfrageData.PRO_i64LoetprogrammVersionsId))
					{
						list.Add(edcAbfrageData.PRO_i64LoetprogrammVersionsId);
					}
				}
				await FUN_fdcFuegeSollWertInformationenHinzuAsync(i_i64MaschinenGruppenId, list, edcAbfrageErgebnisse, i_edcIstSollConverter).ConfigureAwait(continueOnCapturedContext: false);
			}
			fdcStop.Stop();
			return edcAbfrageErgebnisse.AsEnumerable();
		}

		private async Task FUN_fdcFuegeSollWertInformationenHinzuAsync(long i_i64MaschinenGruppenId, IList<long> i_lstVersionId, List<EDC_LoetprotokollAbfrageErgebnis> i_edcAbfrageErgebnisse, INF_LoetParameterIstSollConverter i_edcIstSollConverter)
		{
			Dictionary<long, Dictionary<string, EDC_LoetprogrammParameterData>> dicProgrammParameterSollWerte = await FUN_fdcLadeProgrammParameterSollwerteZuVersionenInDictionaryAsync(i_lstVersionId).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<long, string> i_dicRuestGruppenDaten = await FUN_fdcHoleRuestGruppenDaten(i_i64MaschinenGruppenId);
			if (i_edcIstSollConverter != null)
			{
				i_edcIstSollConverter.SUB_SetzteLoetprogrammSollwerte(dicProgrammParameterSollWerte, i_edcAbfrageErgebnisse, i_dicRuestGruppenDaten);
				return;
			}
			throw new EDC_KeinLoetParameterIstSollConverterRegistriert();
		}

		private async Task<Dictionary<long, string>> FUN_fdcHoleRuestGruppenDaten(long i_i64MaschinenGruppenId)
		{
			Dictionary<long, string> dicRuestGruppenDaten = new Dictionary<long, string>();
			foreach (object value in Enum.GetValues(typeof(ENUM_RuestkomponentenTyp)))
			{
				string i_strWhereStatement = EDC_RuestkomponentenData.FUN_strHoleAuchGeloeschteRuestkomponentenFuerTypWhereStatement(i_i64MaschinenGruppenId, (int)value);
				foreach (EDC_RuestkomponentenData item in (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_RuestkomponentenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList())
				{
					dicRuestGruppenDaten.Add(item.PRO_i64RuestkomponentenId, item.PRO_strName);
				}
			}
			return dicRuestGruppenDaten;
		}

		private async Task<Dictionary<long, Dictionary<string, EDC_LoetprogrammParameterData>>> FUN_fdcLadeProgrammParameterSollwerteZuVersionenInDictionaryAsync(IEnumerable<long> i_lst64VersionsId)
		{
			Dictionary<long, Dictionary<string, EDC_LoetprogrammParameterData>> dicVersionIdDictionary = new Dictionary<long, Dictionary<string, EDC_LoetprogrammParameterData>>();
			Dictionary<long, IEnumerable<EDC_LoetprogrammParameterData>> dictionary = await FUN_fdcLadeProgrammParameterSollwerteZuVersionenAsync(i_lst64VersionsId).ConfigureAwait(continueOnCapturedContext: false);
			foreach (long key in dictionary.Keys)
			{
				Dictionary<string, EDC_LoetprogrammParameterData> dictionary2 = new Dictionary<string, EDC_LoetprogrammParameterData>();
				dictionary.TryGetValue(key, out IEnumerable<EDC_LoetprogrammParameterData> value);
				if (value != null)
				{
					foreach (EDC_LoetprogrammParameterData item in value)
					{
						dictionary2.Add(item.PRO_strVariable, item);
					}
				}
				dicVersionIdDictionary.Add(key, dictionary2);
			}
			return dicVersionIdDictionary;
		}

		private async Task<Dictionary<long, Dictionary<string, EDC_LoetprotokollVariablenData>>> FUN_edcLadeRegistrierteProtokollVariablenInDictionaryAsync(IEnumerable<long> i_lst64MaschinenId)
		{
			Dictionary<long, Dictionary<string, EDC_LoetprotokollVariablenData>> dicMaschinenIdDictionary = new Dictionary<long, Dictionary<string, EDC_LoetprotokollVariablenData>>();
			Dictionary<string, EDC_LoetprotokollVariablenData> dicSpaltenNameVariablenData = new Dictionary<string, EDC_LoetprotokollVariablenData>();
			Dictionary<long, IEnumerable<EDC_LoetprotokollVariablenData>> dictionary = await FUN_edcLadeRegistrierteProtokollVariablenAsync(i_lst64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			foreach (long key in dictionary.Keys)
			{
				dictionary.TryGetValue(key, out IEnumerable<EDC_LoetprotokollVariablenData> value);
				if (value != null)
				{
					foreach (EDC_LoetprotokollVariablenData item in value)
					{
						dicSpaltenNameVariablenData.Add(item.PRO_strSpaltenName, item);
					}
				}
				dicMaschinenIdDictionary.Add(key, dicSpaltenNameVariablenData);
			}
			return dicMaschinenIdDictionary;
		}
	}
}
