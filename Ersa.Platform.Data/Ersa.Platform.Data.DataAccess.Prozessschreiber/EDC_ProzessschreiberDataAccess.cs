using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Prozessschreiber;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Prozessschreiber;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Prozessschreiber
{
	public class EDC_ProzessschreiberDataAccess : EDC_DataAccess, INF_ProzessschreiberDataAccess, INF_DataAccess
	{
		private readonly Dictionary<string, Tuple<string, string>> m_dicRegistrierteVariablen = new Dictionary<string, Tuple<string, string>>();

		private readonly Dictionary<string, Tuple<string, string>> m_dicSpaltenZuordnung = new Dictionary<string, Tuple<string, string>>();

		public EDC_ProzessschreiberDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcRegistriereProzessschreiberVariablenAsync(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen)
		{
			List<EDC_SchreiberVariablenData> lstAlleVariablen = i_lstVariablen.ToList();
			if (lstAlleVariablen.Any())
			{
				long i64MaschinenId = lstAlleVariablen.First().PRO_i64MaschinenId;
				List<EDC_SchreiberVariablenData> lstAlleRegistrietenVariablen = (await FUN_fdcHoleAlleRegistriertenVariablenFuerMaschineAsync(i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				List<EDC_SchreiberVariablenData> lstNichtRegistrierteVariablen = lstAlleVariablen.Except(lstAlleRegistrietenVariablen).ToList();
				if (lstNichtRegistrierteVariablen.Any())
				{
					await FUN_edcLegeDieNeuenVariablenAnAsync(i64MaschinenId, lstNichtRegistrierteVariablen).ConfigureAwait(continueOnCapturedContext: false);
					lstAlleRegistrietenVariablen.AddRange(lstNichtRegistrierteVariablen);
					SUB_ErweitereDieDatenTabelle(i64MaschinenId, lstAlleRegistrietenVariablen, lstNichtRegistrierteVariablen);
				}
				await FUN_edcLadeRegistrierteVariablenAsync(i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleAlleRegistriertenVariablenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_SchreiberVariablenData.FUN_strMaschinenVariablenWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_SchreiberVariablenData(i_strWhereStatement));
		}

		public async Task FUN_fdcSpeichereProzessschreiberVariablenAsync(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen)
		{
			List<EDC_SchreiberVariablenData> list = i_lstVariablen.ToList();
			if (list.Any())
			{
				long pRO_i64MaschinenId = list.First().PRO_i64MaschinenId;
				if (!m_dicRegistrierteVariablen.Any())
				{
					throw new ArgumentNullException("i_lstVariablen", "The variables must first be registered before they can be written to the database");
				}
				SUB_ErgaenzeDieDynamischenSpaltenNamen(list);
				EDC_SchreiberData eDC_SchreiberData = FUN_edcErstelleModellobjektAusVariablen(pRO_i64MaschinenId, list);
				eDC_SchreiberData.PRO_dtmTimeStamp = list.First().PRO_dtmTimeStamp;
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_SchreiberData).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleProzessschreiberDatenAsync(long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IEnumerable<EDC_SchreiberVariablenData> i_lstSuchVariablen)
		{
			if (!m_dicRegistrierteVariablen.Any())
			{
				await FUN_edcLadeRegistrierteVariablenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
				if (!m_dicRegistrierteVariablen.Any())
				{
					return new List<EDC_SchreiberVariablenData>();
				}
			}
			List<EDC_SchreiberVariablenData> list = SUB_EntferneNichtRegistrierteVariablen(i_lstSuchVariablen.ToList()).ToList();
			SUB_ErgaenzeDieDynamischenSpaltenNamen(list);
			EDC_SchreiberData eDC_SchreiberData = FUN_edcErstelleModellobjektAusVariablen(i_i64MaschinenId, list);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			eDC_SchreiberData.PRO_strWhereStatement = EDC_SchreiberData.FUN_strAufgetretenWhereStatementErstellenMitParameter(i_sttVon, i_sttBis, dictionary);
			return FUN_lstKonvertiereInSchreiberVariablenData(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(eDC_SchreiberData, null, dictionary).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleProzessschreiberDatenAsync(long i_i64MaschinenId, DateTime i_sttVon, int i_i32AnzahlDatensaetze, IEnumerable<EDC_SchreiberVariablenData> i_lstSuchVariablen)
		{
			if (!m_dicRegistrierteVariablen.Any())
			{
				await FUN_edcLadeRegistrierteVariablenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
				if (!m_dicRegistrierteVariablen.Any())
				{
					return new List<EDC_SchreiberVariablenData>();
				}
			}
			Stopwatch fdcStop = Stopwatch.StartNew();
			List<EDC_SchreiberVariablenData> list = SUB_EntferneNichtRegistrierteVariablen(i_lstSuchVariablen.ToList()).ToList();
			SUB_ErgaenzeDieDynamischenSpaltenNamen(list);
			EDC_SchreiberData eDC_SchreiberData = FUN_edcErstelleModellobjektAusVariablen(i_i64MaschinenId, list);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			eDC_SchreiberData.PRO_strWhereStatement = EDC_SchreiberData.FUN_strAufgetretenVorWhereStatementErstellenMitParameter(i_sttVon, dictionary);
			IEnumerable<EDC_SchreiberData> i_lstSchreiberData = await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(eDC_SchreiberData, null, dictionary, i_i32AnzahlDatensaetze).ConfigureAwait(continueOnCapturedContext: false);
			fdcStop.Stop();
			return FUN_lstKonvertiereInSchreiberVariablenData(i_lstSchreiberData);
		}

		public async Task<long> FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion)
		{
			try
			{
				IEnumerable<string> source = m_edcDatenbankAdapter.FUN_enuHoleListeAllerTabellen();
				string strTabellenName = EDC_SchreiberData.FUN_strBestimmeTabellenName(i_i64MaschinenId);
				if (!source.Any((string i_strTabelle) => i_strTabelle.Equals(strTabellenName, StringComparison.InvariantCultureIgnoreCase)))
				{
					return 0L;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				string i_strSql = EDC_SchreiberData.FUN_strSelectCountVorStartdatumStatement(i_fdcStartzeitpunkt, i_i64MaschinenId, dictionary);
				object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
				if (obj == null || obj == DBNull.Value)
				{
					return 0L;
				}
				return Convert.ToInt64(obj);
			}
			catch (Exception)
			{
				return 0L;
			}
		}

		public async Task FUN_fdcLoescheAlleProzessschreiberDatenVorDatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion)
		{
			if (await FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(i_i64MaschinenId, i_fdcStartzeitpunkt, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != 0L)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				string i_strSql = EDC_SchreiberData.FUN_strLoescheSchreiberdatenVorDatumStatement(i_fdcStartzeitpunkt, i_i64MaschinenId, dictionary);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task FUN_edcLadeRegistrierteVariablenAsync(long i_i64MaschinenId)
		{
			List<EDC_SchreiberVariablenData> list = (await FUN_fdcHoleAlleRegistriertenVariablenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			m_dicRegistrierteVariablen.Clear();
			m_dicSpaltenZuordnung.Clear();
			foreach (EDC_SchreiberVariablenData item2 in list)
			{
				string item = item2.PRO_strEinheitKey ?? string.Empty;
				m_dicRegistrierteVariablen.Add(item2.PRO_strVariable, new Tuple<string, string>(item2.PRO_strSpaltenName, item));
				m_dicSpaltenZuordnung.Add(item2.PRO_strSpaltenName, new Tuple<string, string>(item2.PRO_strVariable, item));
			}
		}

		private void SUB_ErgaenzeDieDynamischenSpaltenNamen(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen)
		{
			foreach (EDC_SchreiberVariablenData item in i_lstVariablen)
			{
				if (m_dicRegistrierteVariablen.TryGetValue(item.PRO_strVariable, out Tuple<string, string> value))
				{
					item.PRO_strSpaltenName = value.Item1;
					item.PRO_strEinheitKey = value.Item2;
				}
			}
		}

		private IEnumerable<EDC_SchreiberVariablenData> SUB_EntferneNichtRegistrierteVariablen(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen)
		{
			return (from i_edcVariable in i_lstVariablen
			where m_dicRegistrierteVariablen.ContainsKey(i_edcVariable.PRO_strVariable)
			select i_edcVariable).ToList();
		}

		private async Task FUN_edcLegeDieNeuenVariablenAnAsync(long i_i64MaschinenId, IEnumerable<EDC_SchreiberVariablenData> i_lstNeueVariablen)
		{
			long num = await FUN_i64HoleDieHoechsteVariablenNummerAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false) + 1;
			List<EDC_SchreiberVariablenData> list = i_lstNeueVariablen.ToList();
			foreach (EDC_SchreiberVariablenData item in list)
			{
				item.PRO_i64MaschinenId = i_i64MaschinenId;
				item.PRO_strSpaltenName = string.Format("c{0}", num.ToString("D3"));
				num++;
			}
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(list).ConfigureAwait(continueOnCapturedContext: false);
		}

		private void SUB_ErweitereDieDatenTabelle(long i_i64MaschinenId, IEnumerable<EDC_SchreiberVariablenData> i_lstAlleVariablen, IEnumerable<EDC_SchreiberVariablenData> i_lstNeueVariablen)
		{
			EDC_SchreiberData eDC_SchreiberData = FUN_edcErstelleModellobjektAusVariablen(i_i64MaschinenId, i_lstAlleVariablen);
			if (!m_edcDatenbankAdapter.FUN_blnExistiertDieTabelle(eDC_SchreiberData.PRO_strTabellenname))
			{
				m_edcDatenbankAdapter.SUB_ErstelleEineZusatztabelle(eDC_SchreiberData);
				return;
			}
			List<EDC_DynamischeSpalte> i_lstSpalten = (from i_edcVariable in i_lstNeueVariablen
			select new EDC_DynamischeSpalte(i_edcVariable.PRO_strSpaltenName, i_edcVariable.PRO_strSpaltenDatenTyp, i_edcVariable.PRO_objWert)).ToList();
			m_edcDatenbankAdapter.SUB_FuegeTabellenSpaltenHinzu(eDC_SchreiberData.PRO_strTabellenname, i_lstSpalten);
		}

		private EDC_SchreiberData FUN_edcErstelleModellobjektAusVariablen(long i_i64MaschinenId, IEnumerable<EDC_SchreiberVariablenData> i_lstAlleVariablen)
		{
			EDC_SchreiberData eDC_SchreiberData = new EDC_SchreiberData(i_i64MaschinenId);
			List<EDC_DynamischeSpalte> list = (List<EDC_DynamischeSpalte>)(eDC_SchreiberData.PRO_lstDynamischeSpalten = (from i_edcVariable in i_lstAlleVariablen
			select new EDC_DynamischeSpalte(i_edcVariable.PRO_strSpaltenName, i_edcVariable.PRO_strSpaltenDatenTyp, i_edcVariable.PRO_objWert)).ToList());
			return eDC_SchreiberData;
		}

		private async Task<long> FUN_i64HoleDieHoechsteVariablenNummerAsync(long i_i64MaschinenId)
		{
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(EDC_SchreiberVariablenData.FUN_strSelectCountStatementAufMaschinenIdErstellen(i_i64MaschinenId)).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return 0L;
			}
			return Convert.ToInt64(obj);
		}

		private IEnumerable<EDC_SchreiberVariablenData> FUN_lstKonvertiereInSchreiberVariablenData(IEnumerable<EDC_SchreiberData> i_lstSchreiberData)
		{
			List<EDC_SchreiberVariablenData> list = new List<EDC_SchreiberVariablenData>();
			foreach (EDC_SchreiberData i_lstSchreiberDatum in i_lstSchreiberData)
			{
				foreach (EDC_DynamischeSpalte item in i_lstSchreiberDatum.PRO_lstDynamischeSpalten)
				{
					if (item.PRO_objWert != null)
					{
						EDC_SchreiberVariablenData eDC_SchreiberVariablenData = new EDC_SchreiberVariablenData
						{
							PRO_dtmTimeStamp = i_lstSchreiberDatum.PRO_dtmTimeStamp,
							PRO_objWert = item.PRO_objWert
						};
						if (m_dicSpaltenZuordnung.TryGetValue(item.PRO_strSpaltenName, out Tuple<string, string> value))
						{
							eDC_SchreiberVariablenData.PRO_strVariable = value.Item1;
							eDC_SchreiberVariablenData.PRO_strEinheitKey = value.Item2;
						}
						list.Add(eDC_SchreiberVariablenData);
					}
				}
			}
			return list;
		}
	}
}
