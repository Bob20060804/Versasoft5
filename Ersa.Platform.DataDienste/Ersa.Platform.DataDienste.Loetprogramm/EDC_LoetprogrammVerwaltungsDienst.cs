using Ersa.Global.Common.Helper;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Aoi;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataContracts.Aoi;
using Ersa.Platform.DataContracts.Benutzer;
using Ersa.Platform.DataContracts.Cad;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataDienste.Loetprogramm.Exceptions;
using Ersa.Platform.DataDienste.Loetprogramm.Helfer;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Loetprogramm
{
	[Export(typeof(INF_LoetprogrammVerwaltungsDienst))]
	public class EDC_LoetprogrammVerwaltungsDienst : EDC_DataDienst, INF_LoetprogrammVerwaltungsDienst
	{
		private const string mC_strProgrammAenderungsQuelle = "EDC_LoetprogrammVerwaltungsDienst";

		private readonly Lazy<INF_LoetprogrammBibliothekDataAccess> m_edcLoetprogrammBibliothekDataAccess;

		private readonly Lazy<INF_LoetprogrammProgrammDataAccess> m_edcLoetprogrammProgrammDataDataAccess;

		private readonly Lazy<INF_LoetprogrammValideDataAccess> m_edcLoetprogrammValideDataDataAccess;

		private readonly Lazy<INF_LoetprogrammVersionDataAccess> m_edcLoetprogrammVersionDataAccess;

		private readonly Lazy<INF_LoetprogrammParameterDataAccess> m_edcLoetprogrammParameterDataAccess;

		private readonly Lazy<INF_LoetprogrammSatzDatenDataAccess> m_edcLoetprogrammSatzDatenDataAccess;

		private readonly Lazy<INF_LoetprogrammNutzenDatenDataAccess> m_edcLoetprogrammNutzenDatenDataAccess;

		private readonly Lazy<INF_LoetprogrammEcp3DatenDataAccess> m_edcLoetprogrammEcp3DatenDataAccess;

		private readonly Lazy<INF_LoetprogrammBildDataAccess> m_edcLoetprogrammBildDataAccess;

		private readonly Lazy<INF_CadBildDataAccess> m_edcCadBildDataAccess;

		private readonly Lazy<INF_CadDatenDataAccess> m_edcCadDatenDataAccess;

		private readonly Lazy<INF_CadEinstellungenDataAccess> m_edcCadEinstellungenDataAccess;

		private readonly Lazy<INF_AoiDataAccess> m_edcAoiDataAccess;

		private readonly Lazy<INF_MaschinenDataAccess> m_edcMaschinenDataAccess;

		private readonly Lazy<INF_BenutzerVerwaltungDataAccess> m_edcBenutzerVerwaltungDataAccess;

		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly INF_IODienst m_edcIoDienst;

		private readonly IDictionary<ENUM_BildVerwendung, Tuple<int, int>> m_dicBildGroessen = new Dictionary<ENUM_BildVerwendung, Tuple<int, int>>
		{
			{
				ENUM_BildVerwendung.enmThumbnail,
				new Tuple<int, int>(56, 42)
			},
			{
				ENUM_BildVerwendung.enmVorschau,
				new Tuple<int, int>(320, 240)
			},
			{
				ENUM_BildVerwendung.enmVollbild,
				new Tuple<int, int>(480, 360)
			}
		};

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
		{
			get;
			set;
		}

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_LoetprogrammVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, IEventAggregator i_fdcEventAggregator, INF_IODienst i_edcIoDienst)
			: base(i_edcCapabilityProvider)
		{
			m_edcLoetprogrammBibliothekDataAccess = new Lazy<INF_LoetprogrammBibliothekDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammBibliothekDataAccess>);
			m_edcLoetprogrammProgrammDataDataAccess = new Lazy<INF_LoetprogrammProgrammDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammProgrammDataAccess>);
			m_edcLoetprogrammValideDataDataAccess = new Lazy<INF_LoetprogrammValideDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammValideDataAccess>);
			m_edcLoetprogrammVersionDataAccess = new Lazy<INF_LoetprogrammVersionDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammVersionDataAccess>);
			m_edcLoetprogrammParameterDataAccess = new Lazy<INF_LoetprogrammParameterDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammParameterDataAccess>);
			m_edcLoetprogrammSatzDatenDataAccess = new Lazy<INF_LoetprogrammSatzDatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammSatzDatenDataAccess>);
			m_edcLoetprogrammNutzenDatenDataAccess = new Lazy<INF_LoetprogrammNutzenDatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammNutzenDatenDataAccess>);
			m_edcLoetprogrammEcp3DatenDataAccess = new Lazy<INF_LoetprogrammEcp3DatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammEcp3DatenDataAccess>);
			m_edcLoetprogrammBildDataAccess = new Lazy<INF_LoetprogrammBildDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammBildDataAccess>);
			m_edcCadBildDataAccess = new Lazy<INF_CadBildDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CadBildDataAccess>);
			m_edcCadDatenDataAccess = new Lazy<INF_CadDatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CadDatenDataAccess>);
			m_edcCadEinstellungenDataAccess = new Lazy<INF_CadEinstellungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CadEinstellungenDataAccess>);
			m_edcAoiDataAccess = new Lazy<INF_AoiDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_AoiDataAccess>);
			m_edcMaschinenDataAccess = new Lazy<INF_MaschinenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenDataAccess>);
			m_edcBenutzerVerwaltungDataAccess = new Lazy<INF_BenutzerVerwaltungDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_BenutzerVerwaltungDataAccess>);
			m_edcIoDienst = i_edcIoDienst;
			m_fdcEventAggregator = i_fdcEventAggregator;
		}

		public async Task<IEnumerable<EDC_BibliothekInfo>> FUN_fdcBibliothekenAuslesenAsync(string i_strSuchbegriff = null)
		{
			List<EDC_BibliothekInfo> lstBibs = new List<EDC_BibliothekInfo>();
			Stopwatch fdcStop = Stopwatch.StartNew();
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_LoetprogrammBibliothekData> list = (await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcHoleAlleNichtGeloeschtenBibliothekenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			foreach (EDC_LoetprogrammBibliothekData edcBibliothekData in list)
			{
				try
				{
					EDC_BibliothekInfo eDC_BibliothekInfo = await FUN_edcBibliothekMitProgrammenLadenAsync(edcBibliothekData, i_strSuchbegriff).ConfigureAwait(continueOnCapturedContext: false);
					if (eDC_BibliothekInfo.PRO_lstProgramme.Any() || string.IsNullOrEmpty(i_strSuchbegriff))
					{
						lstBibs.Add(eDC_BibliothekInfo);
					}
				}
				catch (Exception i_fdcEx)
				{
					string i_strMessage = $"Error loading a soldering library: Library with Id {edcBibliothekData.PRO_i64BibliothekId} cannot be loaded.";
					SUB_FehlerLoggen(i_fdcEx, MethodBase.GetCurrentMethod().Name, i_strMessage);
				}
			}
			fdcStop.Stop();
			return lstBibs;
		}

		public async Task<EDC_BibliothekInfo> FUN_fdcBibliothekAuslesenAsync(long i_i64BibliotheksId, string i_strSuchbegriff = null)
		{
			EDC_LoetprogrammBibliothekData edcBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(i_i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false);
			if (edcBibliothekData != null)
			{
				EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = edcBibliothekData;
				eDC_LoetprogrammBibliothekData.PRO_i64ProgramAnzahl = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleProgrammAnzahlMitBibliotheksIdAsync(edcBibliothekData.PRO_i64BibliothekId).ConfigureAwait(continueOnCapturedContext: false);
			}
			return await FUN_edcBibliothekMitProgrammenLadenAsync(edcBibliothekData, i_strSuchbegriff).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IList<string>> FUN_fdcBibliothekNamenAuslesenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (from edcBib in (await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcHoleAlleNichtGeloeschtenBibliothekenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToList()
			orderby edcBib.PRO_strName
			select edcBib.PRO_strName).ToList();
		}

		public async Task<IEnumerable<KeyValuePair<long, string>>> FUN_fdcProgrammIdUndNamenInBibliothekErmittelnAsync(string i_strBibliotheksName)
		{
			return from i_edcPrg in (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleAlleProgrammeMitBibliothekNamenAsync(i_strBibliotheksName).ConfigureAwait(continueOnCapturedContext: false)).ToList()
			select new KeyValuePair<long, string>(i_edcPrg.PRO_i64ProgrammId, i_edcPrg.PRO_strName);
		}

		public async Task<IEnumerable<KeyValuePair<long, string>>> FUN_fdcProgrammIdUndNamenInBibliothekErmittelnAsync(long i_i64BibliotheksId)
		{
			return from i_edcPrg in (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleAlleProgrammeMitBibliothekIdAsync(i_i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false)).ToList()
			select new KeyValuePair<long, string>(i_edcPrg.PRO_i64ProgrammId, i_edcPrg.PRO_strName);
		}

		public async Task<IEnumerable<EDC_VersionsInfo>> FUN_fdcLoetprogrammVersionsStapelHolenAsync(long i_i64ProgrammId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IEnumerable<EDC_LoetprogrammVersionAbfrageData> enumerable = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcSichtbareVersionenLadenAsync(i_i64ProgrammId, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_VersionsInfo> lstVersionsInfos = new List<EDC_VersionsInfo>();
			foreach (EDC_LoetprogrammVersionAbfrageData item in enumerable)
			{
				lstVersionsInfos.Add(await FUN_edcVersionKonvertierenAsync(item).ConfigureAwait(continueOnCapturedContext: false));
			}
			return lstVersionsInfos;
		}

		public async Task<EDC_VersionsInfo> FUN_fdcLoetprogrammVersionHolenAsync(long i_i64VersionsId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await FUN_edcVersionKonvertierenAsync(await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionLadenAsync(i_i64VersionsId, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcLoetprogrammVerschiebenAsync(long i_i64ProgrammId, long i_i64BibliotheksId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await FUN_fdcPruefeObPrgNameInBibBereitsExistiertAsync(i_i64BibliotheksId, (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_strName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammVerschiebenAsync(i_i64ProgrammId, i_i64BibliotheksId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcLoetprogrammDuplizierenAsync(long i_i64ProgrammId, string i_strNeuerName, long i_i64BibliotheksId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await FUN_fdcPruefeObPrgNameInBibBereitsExistiertAsync(i_i64BibliotheksId, i_strNeuerName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = (await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)) ?? (await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcFreigegebeneVersionLadenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false));
				if (eDC_LoetprogrammVersionData == null)
				{
					throw new EDC_KeinEindeutigesLoetprogrammException();
				}
				long result = await FUN_fdcLoetprogrammDuplizierenAsync(i_i64ProgrammId, i_i64BibliotheksId, i_strNeuerName, eDC_LoetprogrammVersionData.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcLoetprogrammDuplizierenAsync(long i_i64ProgrammId, long i_i64UrsprungsVerionsId, string i_strNeuerName, long i_i64BibliotheksId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await FUN_fdcPruefeObPrgNameInBibBereitsExistiertAsync(i_i64BibliotheksId, i_strNeuerName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionLadenAsync(i_i64UrsprungsVerionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammVersionData == null)
				{
					throw new EDC_KeinEindeutigesLoetprogrammException();
				}
				long result = await FUN_fdcLoetprogrammDuplizierenAsync(i_i64ProgrammId, i_i64BibliotheksId, i_strNeuerName, eDC_LoetprogrammVersionData.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcLoetprogrammLoeschenAsync(long i_i64ProgrammId)
		{
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammGeloeschtSetzenAsync(i_i64ProgrammId, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcLoetprogrammUmbenennenAsync(long i_i64ProgrammId, string i_strNeuerName)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await FUN_fdcPruefeObPrgNameInBibBereitsExistiertAsync((await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_i64BibliotheksId, i_strNeuerName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammUmbenennenAsync(i_i64ProgrammId, i_strNeuerName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcBibliothekErstellenAsync(string i_strBibliotheksName)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitNamenAsync(i_strBibliotheksName, i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammBibliothekData != null && !eDC_LoetprogrammBibliothekData.PRO_blnGeloescht)
				{
					throw new EDC_BibliothekExistiertBereitsException(i_strBibliotheksName);
				}
				long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				long i_i64GruppenId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				EDC_LoetprogrammBibliothekData obj = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcBibliothekErstellenAsync(i_strBibliotheksName, i64BenutzerId, i_i64GruppenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return obj?.PRO_i64BibliothekId ?? 0;
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcBibliothekLoeschenAsync(long i_i64BibliotheksId)
		{
			if (i_i64BibliotheksId != 0L)
			{
				long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					EDC_LoetprogrammBibliothekData edcBibliothek = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(i_i64BibliotheksId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (edcBibliothek != null)
					{
						await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcBibliothekGeloeschtSetzenAsync(edcBibliothek.PRO_i64BibliothekId, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
						await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcAlleLoetprogrammEinerBibliothekGeloeschtSetzenAsync(edcBibliothek.PRO_i64BibliothekId, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				catch
				{
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
					throw;
				}
			}
		}

		public async Task FUN_fdcBibliothekUmbenennenAsync(long i_i64BibliotheksId, string i_strNeuerName)
		{
			if (i_i64BibliotheksId != 0L)
			{
				long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitNamenAsync(i_strNeuerName, i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (eDC_LoetprogrammBibliothekData != null && !eDC_LoetprogrammBibliothekData.PRO_blnGeloescht)
					{
						throw new EDC_BibliothekExistiertBereitsException(i_strNeuerName);
					}
					await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcBibliothekUmbenennenAsync(i_i64BibliotheksId, i_strNeuerName, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				catch (Exception)
				{
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
					throw;
				}
			}
		}

		public async Task<EDC_ProgrammInfo> FUN_fdcDefaultProgrammInfoLesenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammData eDC_LoetprogrammData = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleDefaultLoetprogrammDataFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammData == null)
			{
				return null;
			}
			return await FUN_fdcProgrammInfoLesenAsync(eDC_LoetprogrammData.PRO_i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoAusVersionsIdLesenAsync(long i_i64VersionsId)
		{
			if (i_i64VersionsId == 0L)
			{
				return null;
			}
			long i_i64MaschineId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_LoetprogrammInfoDataAbfrage> i_enuAbfrage = (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprgInfoAbfrageVonVersionsIdAsync(i_i64VersionsId, i_i64MaschineId).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			return FUN_edcErstelleProgrammInfoAusInfoAbfrage(i_enuAbfrage);
		}

		public async Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoFuerAktuelleFreigegebeneVersionAusVersionsIdLesenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64VersionsId == 0L)
			{
				return null;
			}
			long i_i64MaschineId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammInfoDataAbfrage eDC_LoetprogrammInfoDataAbfrage = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleAktuelleFreigegebeneLoetprgInfoAbfrageVonVersionsIdAsync(i_i64VersionsId, i_i64MaschineId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammInfoDataAbfrage == null)
			{
				return null;
			}
			return EDC_ProgrammInfoKonvertierungsHelfer.FUN_edcProgrammInfoKonvertieren(eDC_LoetprogrammInfoDataAbfrage, new List<ENUM_LoetprogrammStatus>
			{
				eDC_LoetprogrammInfoDataAbfrage.PRO_enmProgrammStatus
			}, new List<ENUM_LoetprogrammFreigabeStatus>
			{
				eDC_LoetprogrammInfoDataAbfrage.PRO_enmFreigabeStatus
			}, new List<bool>
			{
				eDC_LoetprogrammInfoDataAbfrage.PRO_blnValide
			});
		}

		public async Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoLesenAsync(long i_i64ProgrammId)
		{
			if (i_i64ProgrammId == 0L)
			{
				return null;
			}
			long i_i64MaschineId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_LoetprogrammInfoDataAbfrage> i_enuAbfrage = (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprgInfoAbfrageVonProgrammIdAsync(i_i64ProgrammId, i_i64MaschineId).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			return FUN_edcErstelleProgrammInfoAusInfoAbfrage(i_enuAbfrage);
		}

		[Export("Ersa.Platform.Dienste.Loetprogramm.EDC_LoetprogrammVerwaltungsDienst.FUN_fdcProgInfoAlsFehlerhaftSpeichernAsync")]
		public async Task FUN_fdcProgmmVersionAlsFehlerhaftSpeichernAsync(long i_i64ProgramId, long i_i64VersionsId)
		{
			if (i_i64VersionsId != 0L)
			{
				long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammValideDataDataAccess.Value.FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(i_i64VersionsId, i_i64MaschinenId, i_blnValide: false).ConfigureAwait(continueOnCapturedContext: false);
				m_fdcEventAggregator.GetEvent<EDC_ProgrammeGeandertEvent>().Publish(new EDC_ProgrammeGeandertEventPayload("EDC_LoetprogrammVerwaltungsDienst")
				{
					PRO_enuGeaenderteProgramme = new long[1]
					{
						i_i64ProgramId
					}
				});
			}
		}

		public async Task<long> FUN_fdcImportBibliothekAsync(string i_strBibliothekspfad, string i_strNeuerBibliothekName)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strBibliothekspfad))
			{
				return 0L;
			}
			IEnumerable<string> enuImportDateien = m_edcIoDienst.FUN_enuDateienImVerzeichnisErmitteln(i_strBibliothekspfad);
			IDbTransaction fdcDbTransaction = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitNamenAsync(i_strNeuerBibliothekName, i64MaschinenId, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false) != null)
				{
					throw new InvalidDataException("library already exists");
				}
				long i64NeuGruppenId = await FUN_i64BestimmeGruppenIdAsync(i64MaschinenId, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				if (i64NeuGruppenId == -1)
				{
					throw new InvalidDataException("group id mismatch");
				}
				EDC_LoetprogrammBibliothekData edcNeueBibliothek = null;
				foreach (string item in enuImportDateien)
				{
					DataSet fdcDataSet = await EDC_XmlZuDataSetReader.FUN_fdcLeseXmlDateiInDatasetAsync(item).ConfigureAwait(continueOnCapturedContext: false);
					if (edcNeueBibliothek == null)
					{
						DataTable i_fdcTable = fdcDataSet.Tables["SolderingLibraries"];
						edcNeueBibliothek = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcImportiereBibliothekAsync(i_fdcTable, i_strNeuerBibliothekName, i64BenutzerId, i64NeuGruppenId, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
						if (!(await FUN_blnPasstLoetprogrammZuMaschinenTypAsync(fdcDataSet, edcNeueBibliothek.PRO_i64BibliothekId, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false)))
						{
							throw new InvalidDataException("wrong or missing machine type");
						}
					}
					await FUN_fdcImportiereLoetprogrammAsync(fdcDataSet, edcNeueBibliothek.PRO_i64BibliothekId, i64BenutzerId, string.Empty, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_edcLoetprogrammBibliothekDataAccess.Value.SUB_CommitTransaktion(fdcDbTransaction);
				return edcNeueBibliothek?.PRO_i64BibliothekId ?? 0;
			}
			catch
			{
				m_edcLoetprogrammBibliothekDataAccess.Value.SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task<long> FUN_fdcImportiereLoetprogrammAsync(long i_i64ZielBibliotheksId, string i_strProgrammDatei, string i_strNeuerProgrammName)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (!m_edcIoDienst.FUN_blnDateiExistiert(i_strProgrammDatei))
			{
				return 0L;
			}
			DataSet fdcDataSet = await EDC_XmlZuDataSetReader.FUN_fdcLeseXmlDateiInDatasetAsync(i_strProgrammDatei).ConfigureAwait(continueOnCapturedContext: false);
			if (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_strNeuerProgrammName, i_i64ZielBibliotheksId).ConfigureAwait(continueOnCapturedContext: false) != null)
			{
				return 0L;
			}
			IDbTransaction fdcDbTransaction = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!(await FUN_blnPasstLoetprogrammZuMaschinenTypAsync(fdcDataSet, i_i64ZielBibliotheksId, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false)))
				{
					throw new InvalidDataException("wrong or missing machine type");
				}
				long result = await FUN_fdcImportiereLoetprogrammAsync(fdcDataSet, i_i64ZielBibliotheksId, i64BenutzerId, i_strNeuerProgrammName, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammBibliothekDataAccess.Value.SUB_CommitTransaktion(fdcDbTransaction);
				return result;
			}
			catch (Exception)
			{
				m_edcLoetprogrammBibliothekDataAccess.Value.SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task<bool> FUN_fdcExportBibliothekAsync(long i_i64ZielBibliotheksId, string i_strZielpfad)
		{
			EDC_LoetprogrammBibliothekData edcBibliothek = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(i_i64ZielBibliotheksId).ConfigureAwait(continueOnCapturedContext: false);
			List<long> list = (from i_edcLoetprogramm in (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleAlleProgrammeMitBibliothekIdAsync(i_i64ZielBibliotheksId).ConfigureAwait(continueOnCapturedContext: false)).ToList()
			select i_edcLoetprogramm.PRO_i64ProgrammId).ToList();
			string strBibliothekPfad = Path.Combine(i_strZielpfad, edcBibliothek.PRO_strName);
			SUB_ErstelleExportVerzeichnis(strBibliothekPfad);
			foreach (long item in list)
			{
				if (!(await FUN_blnExportProgrammAsync(await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(item).ConfigureAwait(continueOnCapturedContext: false), edcBibliothek, strBibliothekPfad).ConfigureAwait(continueOnCapturedContext: false)))
				{
					return false;
				}
			}
			return true;
		}

		public async Task<bool> FUN_fdcExportProgrammAsync(long i_i64ProgrammId, string i_strZielpfad)
		{
			SUB_ErstelleExportVerzeichnis(i_strZielpfad);
			EDC_LoetprogrammData edcLoetprogrammData = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
			return await FUN_blnExportProgrammAsync(edcLoetprogrammData, await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(edcLoetprogrammData.PRO_i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false), i_strZielpfad).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_fdcBibliothekDuplizierenAsync(long i_i64BibliotheksId, string i_strNeuerName)
		{
			if (i_i64BibliotheksId == 0L)
			{
				return 0L;
			}
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammBibliothekData edcBibliothek = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitNamenAsync(i_strNeuerName, i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (edcBibliothek != null && !edcBibliothek.PRO_blnGeloescht)
				{
					throw new EDC_BibliothekExistiertBereitsException(i_strNeuerName);
				}
				EDC_LoetprogrammBibliothekData edcBibDuplikat = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcBibliothekDuplizierenAsync(i_i64BibliotheksId, i_strNeuerName, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				List<EDC_LoetprogrammData> list = (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleAlleProgrammeMitBibliothekIdAsync(i_i64BibliotheksId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				bool blnKopierenerfolgt = false;
				foreach (EDC_LoetprogrammData edcLoetprogramm in list)
				{
					EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcFreigegebeneVersionLadenAsync(edcLoetprogramm.PRO_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (eDC_LoetprogrammVersionData != null)
					{
						await FUN_fdcLoetprogrammDuplizierenAsync(edcLoetprogramm.PRO_i64ProgrammId, edcBibDuplikat.PRO_i64BibliothekId, edcLoetprogramm.PRO_strName, eDC_LoetprogrammVersionData.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
						blnKopierenerfolgt = true;
					}
				}
				if (!blnKopierenerfolgt)
				{
					throw new EDC_BibliothekOhneGueltigenInhaltException(edcBibliothek?.PRO_strName ?? i_strNeuerName);
				}
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return edcBibDuplikat.PRO_i64BibliothekId;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcArbeitsversionAnlegenAsync(long i_i64ProgrammId, long i_i64UrsprungVersionsId, string i_strKommentar)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != null)
				{
					throw new InvalidDataException("work in progress already exist");
				}
				if (await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionLadenAsync(i_i64UrsprungVersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) == null)
				{
					throw new InvalidDataException("no source version found");
				}
				EDC_LoetprogrammData eDC_LoetprogrammData = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammData == null || eDC_LoetprogrammData.PRO_blnGeloescht)
				{
					throw new InvalidDataException("no valid source program");
				}
				EDC_LoetprogrammStand edcProgrammUrsprung = await FUN_fdcLoetprogrammLadenAsync(i_i64ProgrammId, i_i64UrsprungVersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				edcProgrammUrsprung.PRO_strKommentar = i_strKommentar;
				EDC_LoetprogrammVersionData edcNeueArbeitsversion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionErstellenAsync(i_i64ProgrammId, i64BenutzerId, i_strKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcLoetprogrammSpeichernAsync(edcProgrammUrsprung, null, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return edcNeueArbeitsversion.PRO_i64VersionsId;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcArbeitsversionLoeschenAsync(long i_i64ProgrammId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<EDC_LoetprogrammVersionAbfrageData> lstVersionen = (await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcHoleVersionenStapelAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				EDC_LoetprogrammVersionAbfrageData edcArbeitsversion = lstVersionen.FirstOrDefault((EDC_LoetprogrammVersionAbfrageData i_edcItem) => ENUM_LoetprogrammStatus.Arbeitsversion.Equals(i_edcItem.PRO_enmProgrammStatus));
				if (edcArbeitsversion == null)
				{
					throw new InvalidDataException("work in progress does not exist");
				}
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionLoeschenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcParameterVersionLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammSatzDatenDataAccess.Value.FUN_fdcSatzDatenVersionLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammEcp3DatenDataAccess.Value.FUN_fdcEcp3DatenVersionLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammNutzenDatenDataAccess.Value.FUN_fdcNutzenDatenVersionLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (lstVersionen.Count == 1)
				{
					await m_edcCadBildDataAccess.Value.FUN_fdcBilderLoeschenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				}
				await m_edcCadEinstellungenDataAccess.Value.FUN_fdcCadDatenLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleAblaufSchritteLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleVerboteneBereicheLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleCncSchritteLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleRoutenSchritteLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleRoutenDatenLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAlleBewegungsGruppenLoeschenAsync(edcArbeitsversion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcVersionFreigebenAsync(long i_i64ProgrammId, long i_i64FreigabeVersionsId, string i_strKommentar)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionFreigebenAsync(i_i64ProgrammId, i_i64FreigabeVersionsId, i64BenutzerId, i_strKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcFreigabeWegnehmenAsync(long i_i64ProgrammId)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcFreigeabeWegnehmenAsync(i_i64ProgrammId, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcKommentarFuerArbeitsversionSetzenAsync(long i_i64ProgrammId, string i_strKommentar)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionAktualisierenAsync(i_i64ProgrammId, i64BenutzerId, i_strKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcVersionFreigebeStatusUndNotizenSetzenAsync(long i_i64ProgrammId, long i_i64VersionsId, ENUM_LoetprogrammFreigabeStatus i_enmFreigabeStatus, ENUM_LoetprogrammFreigabeArt i_enmFreigabeArt = ENUM_LoetprogrammFreigabeArt.Einstufig, string i_strFreigabeKommentar = null)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionLadenAsync(i_i64VersionsId, fdcTransaktion);
				if (eDC_LoetprogrammVersionData == null)
				{
					throw new InvalidDataException($"no valid test version for version id = {i_i64VersionsId}");
				}
				if (eDC_LoetprogrammVersionData.PRO_i64ProgrammId != i_i64ProgrammId)
				{
					throw new InvalidDataException($"no valid program id = {i_i64ProgrammId}");
				}
				EDC_FreigabeSchritt item = new EDC_FreigabeSchritt
				{
					PRO_strKommentar = i_strFreigabeKommentar,
					PRO_i64BenutzerId = i64BenutzerId,
					PRO_fdcDatum = DateTime.Now,
					PRO_enmNachReleaseStatus = i_enmFreigabeStatus,
					PRO_enmVonReleaseState = ((eDC_LoetprogrammVersionData.PRO_enmFreigabeStatus != ENUM_LoetprogrammFreigabeStatus.InFreigabe) ? ENUM_LoetprogrammFreigabeStatus.InFreigabe : ENUM_LoetprogrammFreigabeStatus.Undefiniert)
				};
				EDC_FreigabeNotizen eDC_FreigabeNotizen = string.IsNullOrEmpty(eDC_LoetprogrammVersionData.PRO_strFreigabeNotizen) ? new EDC_FreigabeNotizen() : PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_FreigabeNotizen>(eDC_LoetprogrammVersionData.PRO_strFreigabeNotizen);
				eDC_FreigabeNotizen.PRO_enmFreigabeart = i_enmFreigabeArt;
				if (eDC_FreigabeNotizen.PRO_lstFreigabeSchritte == null)
				{
					eDC_FreigabeNotizen.PRO_lstFreigabeSchritte = new List<EDC_FreigabeSchritt>();
				}
				eDC_FreigabeNotizen.PRO_lstFreigabeSchritte.Add(item);
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionFreigebeStatusUndNotizenSetzenAsync(i_i64ProgrammId, i_i64VersionsId, i64BenutzerId, i_enmFreigabeStatus, PRO_edcSerialisierer.FUN_strSerialisieren(eDC_FreigabeNotizen), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				m_edcLoetprogrammVersionDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammVersionDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcVersionSichtbarkeitEntfernenAsync(long i_i64ProgrammId, long i_i64VersionsId)
		{
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionSichtbarkeitEntfernenAsync(i_i64ProgrammId, i_i64VersionsId, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<IEnumerable<EDC_ProgrammAenderung>> FUN_fdcErmittleParameterAenderungenZwischenZweiVersionenAsync(long i_i64AlteVersionsId, long i_i64NeueVersionsId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<EDC_ProgrammAenderung> lstUnterschiede = new List<EDC_ProgrammAenderung>();
				List<EDC_LoetprogrammParameterDiffData> lstAbweichungen = (await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcErmittleParameterAenderungenZwischenZweiVersionenAsync(i_i64AlteVersionsId, i_i64NeueVersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				if (lstAbweichungen.Any())
				{
					IEnumerable<string> i_enuVariablen = (from i_edcItem in lstAbweichungen
					select i_edcItem.PRO_strVariable).Distinct();
					_003C_003Ec__DisplayClass60_0 _003C_003Ec__DisplayClass60_;
					List<EDC_LoetprogrammParameterDiffData> lstNeueWerte2 = _003C_003Ec__DisplayClass60_.lstNeueWerte;
					List<EDC_LoetprogrammParameterDiffData> lstNeueWerte = (await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcErmittleParameterWerteEinerVersionenAsync(i_i64NeueVersionsId, i_enuVariablen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
					lstUnterschiede = (from edcVariable in lstAbweichungen
					let edcNeueVariable = lstNeueWerte.FirstOrDefault((EDC_LoetprogrammParameterDiffData i_edcItem) => i_edcItem.PRO_strVariable.Equals(edcVariable.PRO_strVariable))
					select new EDC_ProgrammAenderung
					{
						PRO_strVariable = edcVariable.PRO_strVariable,
						PRO_strAlterWert = edcVariable.PRO_strWert,
						PRO_strNeuerWert = ((edcNeueVariable == null) ? string.Empty : edcNeueVariable.PRO_strWert)
					}).ToList();
				}
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return lstUnterschiede;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		public Task<long> FUN_fdcErmittleProgrammIdAusNamenAsync(string i_strBibliotheksName, string i_strProgrammName, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcErmittleProgrammIdAusNamenAsync(i_strBibliotheksName, i_strProgrammName, i_fdcTransaktion);
		}

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		public async Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammLadenAsync(string i_strBibliotheksName, string i_strProgrammName, long i_i64VersionsId)
		{
			long num = await FUN_fdcErmittleProgrammIdAusNamenAsync(i_strBibliotheksName, i_strProgrammName).ConfigureAwait(continueOnCapturedContext: false);
			return (num != 0L) ? (await FUN_fdcLoetprogrammLadenAsync(num, i_i64VersionsId).ConfigureAwait(continueOnCapturedContext: false)) : null;
		}

		public async Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammArbeitsversionLadenAsync(long i_i64ProgrammId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammVersionData == null)
				{
					throw new InvalidDataException($"no valid test version for program id = {i_i64ProgrammId}");
				}
				EDC_LoetprogrammStand result = await FUN_fdcLoetprogrammLadenAsync(i_i64ProgrammId, eDC_LoetprogrammVersionData.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammProduktionsFreigabeVersionLadenAsync(long i_i64ProgrammId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcFreigegebeneVersionLadenAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammVersionData == null)
				{
					throw new InvalidDataException($"no production version for program id = {i_i64ProgrammId}");
				}
				EDC_LoetprogrammStand result = await FUN_fdcLoetprogrammLadenAsync(i_i64ProgrammId, eDC_LoetprogrammVersionData.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammLadenAsync(long i_i64ProgrammId, long i_i64VersionsId)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammStand result = await FUN_fdcLoetprogrammLadenAsync(i_i64ProgrammId, i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcLoetprogrammBasisDatenSpeichernAsync(EDC_LoetprogrammData i_edcLoetprogramm, string i_strVersionsKommentar, string i_strBildImportPfad, IDictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten> i_dicFiducialBilder = null)
		{
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_LoetprogrammVersionData edcVersion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i_edcLoetprogramm.PRO_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: true);
				if (edcVersion == null)
				{
					throw new InvalidDataException($"no update possible: version is not work in progress program = {i_edcLoetprogramm.PRO_strName} ");
				}
				long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammDataAktualisierenAsync(i_edcLoetprogramm.PRO_i64ProgrammId, i64BenutzerId, i_edcLoetprogramm.PRO_strNotizen, i_edcLoetprogramm.PRO_strEingangskontrolle, i_edcLoetprogramm.PRO_strAusgangskontrolle, i_edcLoetprogramm.PRO_i32Version, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionAktualisierenAsync(i_edcLoetprogramm.PRO_i64ProgrammId, i64BenutzerId, i_strVersionsKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammValideDataDataAccess.Value.FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(edcVersion.PRO_i64VersionsId, i64MaschinenId, i_edcLoetprogramm.PRO_blnIstFuerMaschineValide, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_strBildImportPfad != null)
				{
					await FUN_fdcProfilBilderAktualisierenAsync(i_edcLoetprogramm.PRO_i64ProgrammId, i_strBildImportPfad, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				await FUN_fdcFiducialBilderSpeichernAsync(i_edcLoetprogramm.PRO_i64ProgrammId, i_dicFiducialBilder, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcLoetprogrammSpeichernAsync(EDC_LoetprogrammStand i_edcLoetprogrammStand, string i_strBildImportPfad, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_edcLoetprogrammStand == null)
			{
				throw new ArgumentNullException("i_edcLoetprogrammStand");
			}
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64BibliotheksId == 0L)
				{
					EDC_LoetprogrammBibliothekData obj = await FUN_fdcHoleDefaultBibliothekAsync(i64MaschinenId, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64BibliotheksId = obj.PRO_i64BibliothekId;
				}
				EDC_LoetprogrammVersionData edcVersion;
				if (i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId == 0L)
				{
					EDC_LoetprogrammData pRO_edcProgrammInfo = i_edcLoetprogrammStand.PRO_edcProgrammInfo;
					pRO_edcProgrammInfo.PRO_i64ProgrammId = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleNaechstenSequenzWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
					edcVersion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionErstellenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i64BenutzerId, i_edcLoetprogrammStand.PRO_strKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammNeuAnlegenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId == 0L || edcVersion.PRO_i64VersionsId == 0L)
					{
						throw new InvalidDataException("Saving soldering program failed: Id is 0");
					}
				}
				else
				{
					edcVersion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (edcVersion == null)
					{
						throw new InvalidDataException("Saving soldering program failed: version is not work in progress");
					}
					await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsversionAktualisierenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i64BenutzerId, i_edcLoetprogrammStand.PRO_strKommentar, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammDataAktualisierenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i64BenutzerId, i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_strNotizen, i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_strEingangskontrolle, i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_strAusgangskontrolle, i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i32Version, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				await m_edcLoetprogrammValideDataDataAccess.Value.FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(edcVersion.PRO_i64VersionsId, i64MaschinenId, i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_blnIstFuerMaschineValide, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcParameterListeSpeichernAsync(i_edcLoetprogrammStand.PRO_enuParameter.ToList(), edcVersion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammSatzDatenDataAccess.Value.FUN_fdcSatzDatenListeHinzufuegenAsync(i_edcLoetprogrammStand.PRO_enuSatzdaten.ToList(), edcVersion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammNutzenDatenDataAccess.Value.FUN_fdcSpeichereNutzenDatenAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuNutzenParameter, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcLoetprogrammEcp3DatenDataAccess.Value.FUN_fdcEcp3DatenListeHinzufuegenAsync(i_edcLoetprogrammStand.PRO_enuEcp3Daten.ToList(), edcVersion.PRO_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcEcp3BilderSpeichernAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_edcLoetprogrammStand.PRO_edcEcp3Bilddaten, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcFiducialBilderSpeichernAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_edcLoetprogrammStand.PRO_dicFiducialBilder, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_strBildImportPfad != null)
				{
					await FUN_fdcProfilBilderAktualisierenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_strBildImportPfad, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				await m_edcCadBildDataAccess.Value.FUN_fdcBilderSpeichernAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_edcLoetprogrammStand.PRO_enuCadBilder, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadEinstellungenDataAccess.Value.FUN_fdcSpeichereCadDatenAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_strCadDaten, i_edcLoetprogrammStand.PRO_strCadEinstellungen).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcVerboteneBereicheSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadVerboteneBereiche, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcAblaufSchritteSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadAblaufSchritte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcCncSchritteSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadCncSchritte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcRoutenSchritteSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadRoutenSchritte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcRoutenDatenSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadRoutenDaten, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcCadDatenDataAccess.Value.FUN_fdcBewegungsGruppenSpeichernAsync(edcVersion.PRO_i64VersionsId, i_edcLoetprogrammStand.PRO_enuCadBewegungsGruppen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_edcLoetprogrammStand.PRO_strAoiDaten != null || i_edcLoetprogrammStand.PRO_strAoiEinstellungen != null)
				{
					await m_edcAoiDataAccess.Value.FUN_fdcSpeichereAoiProgrammAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_edcLoetprogrammStand.PRO_strAoiDaten, i_edcLoetprogrammStand.PRO_strAoiEinstellungen).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_edcLoetprogrammStand.PRO_enuAoiSchritte != null)
				{
					await m_edcAoiDataAccess.Value.FUN_fdcSpeichereAoiSchrittDatenAsync(i_edcLoetprogrammStand.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_edcLoetprogrammStand.PRO_enuAoiSchritte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				return edcVersion.PRO_i64VersionsId;
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		private static string FUN_strBestimmeImportMaschinenTyp(DataSet i_fdcDataSet)
		{
			string result = null;
			long num = Convert.ToInt64(i_fdcDataSet.Tables["SolderingLibraries"].Rows[0]["GroupId"]);
			foreach (DataRow row in i_fdcDataSet.Tables["MaschineGroups"].Rows)
			{
				if (Convert.ToInt64(row["GroupId"]) == num)
				{
					return Convert.ToString(row["MachineType"]);
				}
			}
			return result;
		}

		private async Task<EDC_LoetprogrammBibliothekData> FUN_fdcHoleDefaultBibliothekAsync(long i_i64MaschinenId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion)
		{
			EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleDefaultBibliothekFuerMaschineAsync(i_i64MaschinenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammBibliothekData == null)
			{
				long i_i64GruppenId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				eDC_LoetprogrammBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcBibliothekErstellenAsync("[defaultLibrary]", i_i64BenutzerId, i_i64GruppenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			return eDC_LoetprogrammBibliothekData;
		}

		private async Task<long> FUN_i64BestimmeGruppenIdAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion)
		{
			long i64NeuGruppenId = -1L;
			List<EDC_MaschinenGruppeData> list = (await m_edcMaschinenDataAccess.Value.FUN_fdcHoleMaschinenGruppeZugehoerigkeitenAsync(i_i64MaschinenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			if (list.Any())
			{
				i64NeuGruppenId = list[0].PRO_i64GruppenId;
			}
			return i64NeuGruppenId;
		}

		private async Task<bool> FUN_blnPasstLoetprogrammZuMaschinenTypAsync(DataSet i_fdcDataSet, long i_i64ZielBibliotheksId, IDbTransaction i_fdcTransaktion)
		{
			string strImportMaschinenTyp = FUN_strBestimmeImportMaschinenTyp(i_fdcDataSet);
			if (strImportMaschinenTyp == null)
			{
				return false;
			}
			EDC_LoetprogrammBibliothekData eDC_LoetprogrammBibliothekData = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(i_i64ZielBibliotheksId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return (await m_edcMaschinenDataAccess.Value.FUN_fdcHoleMaschinenGruppeDataAsync(eDC_LoetprogrammBibliothekData.PRO_i64GruppenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_strMaschinenTyp.Equals(strImportMaschinenTyp);
		}

		private async Task<long> FUN_fdcImportiereLoetprogrammAsync(DataSet i_fdcDataSet, long i_i64ZielBibliotheksId, long i_i64BenutzerId, string i_strNeuerProgrammName, IDbTransaction i_fdcTransaktion)
		{
			if (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_strNeuerProgrammName, i_i64ZielBibliotheksId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != null)
			{
				throw new InvalidDataException("program already exists in the target library");
			}
			long i64NeueProgrammId = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleNaechstenSequenzWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			long i64NeueVersionsId = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleNaechstenSequenzWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcLoetprogrammImportierenAsync(i_fdcDataSet, i64NeueProgrammId, i_i64ZielBibliotheksId, i_i64BenutzerId, i_strNeuerProgrammName, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionImportierenAsync(i_fdcDataSet, i64NeueProgrammId, i64NeueVersionsId, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBilddatenImportierenAsync(i_fdcDataSet, i64NeueProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcParameterDatenImportierenAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammEcp3DatenDataAccess.Value.FUN_fdcEcp3DatenImportierenAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammSatzDatenDataAccess.Value.FUN_fdcSatzDatenImportierenAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammNutzenDatenDataAccess.Value.FUN_fdcNutzenDatenImportierenAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcCadBildDataAccess.Value.FUN_fdcImportiereCadBildDatenAusLoetprogrammAsync(i_fdcDataSet, i64NeueProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcCadDatenDataAccess.Value.FUN_fdcImportiereAusLoetprogrammAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcCadEinstellungenDataAccess.Value.FUN_fdcImportiereCadDatenAsync(i_fdcDataSet, i64NeueVersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcAoiDataAccess.Value.FUN_fdcImportiereAoiProgrammAsync(i_fdcDataSet, i64NeueProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcAoiDataAccess.Value.FUN_fdcImportiereAoiSchrittDatenAsync(i_fdcDataSet, i64NeueProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i64NeueProgrammId;
		}

		private void SUB_ErstelleExportVerzeichnis(string i_strZielpfad)
		{
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strZielpfad))
			{
				m_edcIoDienst.SUB_VerzeichnisErstellen(i_strZielpfad);
			}
		}

		private async Task<bool> FUN_blnExportProgrammAsync(EDC_LoetprogrammData i_edcLoetprogrammData, EDC_LoetprogrammBibliothekData i_edcLoetprogrammBibliothekData, string i_strZielpfad)
		{
			long i64ProgrammId = i_edcLoetprogrammData.PRO_i64ProgrammId;
			long i64BibliothekId = i_edcLoetprogrammBibliothekData.PRO_i64BibliothekId;
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcFreigegebeneVersionLadenAsync(i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData == null)
			{
				eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammVersionData == null)
				{
					PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Warnung, $"no valid export version found (released or testversion) for soldering program id = {i64ProgrammId}");
					return true;
				}
			}
			long i64VersionsId = eDC_LoetprogrammVersionData.PRO_i64VersionsId;
			DataSet fdcDataSet = new DataSet();
			IDbTransaction fdcTransaktion = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				DataTable dataTable = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleProgrammInDataTableAsync(i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable != null)
				{
					fdcDataSet.Tables.Add(dataTable);
				}
				DataTable dataTable2 = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_fdcHoleBibliothekInDataTableAsync(i64BibliothekId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable2 != null)
				{
					fdcDataSet.Tables.Add(dataTable2);
				}
				DataTable dataTable3 = await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcHoleBildDataInDataTableAsync(i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable3 != null)
				{
					fdcDataSet.Tables.Add(dataTable3);
				}
				DataTable dataTable4 = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcHoleVersionInDataTableAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable4 != null)
				{
					fdcDataSet.Tables.Add(dataTable4);
				}
				DataTable dataTable5 = await m_edcLoetprogrammEcp3DatenDataAccess.Value.FUN_fdcHoleEcp3DatenInDataTableAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable5 != null)
				{
					fdcDataSet.Tables.Add(dataTable5);
				}
				DataTable dataTable6 = await m_edcLoetprogrammParameterDataAccess.Value.FUN_fdcHoleParameterInDataTableAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable6 != null)
				{
					fdcDataSet.Tables.Add(dataTable6);
				}
				DataTable dataTable7 = await m_edcLoetprogrammSatzDatenDataAccess.Value.FUN_fdcHoleSatzdatenInDataTableAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable7 != null)
				{
					fdcDataSet.Tables.Add(dataTable7);
				}
				DataTable dataTable8 = await m_edcLoetprogrammNutzenDatenDataAccess.Value.FUN_fdcHoleNutzendatenInDataTableAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable8 != null)
				{
					fdcDataSet.Tables.Add(dataTable8);
				}
				DataTable dataTable9 = await m_edcMaschinenDataAccess.Value.FUN_fdcHoleMaschinenGruppeInDataTableAsync(i_edcLoetprogrammBibliothekData.PRO_i64GruppenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable9 != null)
				{
					fdcDataSet.Tables.Add(dataTable9);
				}
				DataTable dataTable10 = await m_edcCadBildDataAccess.Value.FUN_fdcHoleBilddatenDataTableAsync(i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable10 != null)
				{
					fdcDataSet.Tables.Add(dataTable10);
				}
				DataTable dataTable11 = await m_edcCadEinstellungenDataAccess.Value.FUN_fdcExportiereCadDatenAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable11 != null)
				{
					fdcDataSet.Tables.Add(dataTable11);
				}
				DataSet dataSet = await m_edcCadDatenDataAccess.Value.FUN_fdcExportiereProjektDatenVersionAsync(i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataSet != null)
				{
					fdcDataSet.Merge(dataSet);
				}
				DataTable dataTable12 = await m_edcAoiDataAccess.Value.FUN_fdcExportiereAoiProgrammAsync(i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable12 != null)
				{
					fdcDataSet.Merge(dataTable12);
				}
				DataTable dataTable13 = await m_edcAoiDataAccess.Value.FUN_fdcExportiereAoiSchrittDatenAsync(i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (dataTable13 != null)
				{
					fdcDataSet.Merge(dataTable13);
				}
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcLoetprogrammProgrammDataDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
			}
			string arg = m_edcIoDienst.FUN_strValidiereUndBereinigeDateiNamen(i_edcLoetprogrammData.PRO_strName);
			string text = Path.Combine(i_strZielpfad, $"{arg}.xml");
			if (!m_edcIoDienst.FUN_blnDateiExistiert(text))
			{
				fdcDataSet.WriteXml(text, XmlWriteMode.WriteSchema);
				return true;
			}
			return false;
		}

		private void SUB_FehlerLoggen(Exception i_fdcEx, string i_strMethodenName, string i_strMessage)
		{
			PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strMessage, "Ersa.Platform.Dienste.Loetprogramm", "EDC_LoetprogrammVerwaltungsDienst", i_strMethodenName, i_fdcEx);
		}

		private async Task<EDC_Ecp3Bilddaten> FUN_fdcEcp3BilddatenLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion)
		{
			EDC_Ecp3Bilddaten edcEpc3Daten = new EDC_Ecp3Bilddaten();
			EDC_LoetprogrammBildData eDC_LoetprogrammBildData = await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildDatensatzLadenAsync(i_i64ProgrammId, ENUM_BildVerwendung.enmEcp3Bild, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammBildData != null)
			{
				edcEpc3Daten.PRO_bytEcp3BildDaten = eDC_LoetprogrammBildData.PRO_bytBild;
				edcEpc3Daten.PRO_strEcp3BildDateiName = eDC_LoetprogrammBildData.PRO_strDateiname;
			}
			return edcEpc3Daten;
		}

		private async Task<IDictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten>> FUN_fdcFiducialBilddatenLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion)
		{
			List<ENUM_BildVerwendung> list = new List<ENUM_BildVerwendung>
			{
				ENUM_BildVerwendung.enmFiducialBild1Fm1,
				ENUM_BildVerwendung.enmFiducialBild2Fm1,
				ENUM_BildVerwendung.enmFiducialBild1Fm2,
				ENUM_BildVerwendung.enmFiducialBild2Fm2,
				ENUM_BildVerwendung.enmFiducialBild1Lm1,
				ENUM_BildVerwendung.enmFiducialBild2Lm1,
				ENUM_BildVerwendung.enmFiducialBild1Lm2,
				ENUM_BildVerwendung.enmFiducialBild2Lm2,
				ENUM_BildVerwendung.enmFiducialBild1Lm3,
				ENUM_BildVerwendung.enmFiducialBild2Lm3
			};
			Dictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten> dicBilder = new Dictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten>();
			foreach (ENUM_BildVerwendung enmVerwendung in list)
			{
				EDC_LoetprogrammBildData eDC_LoetprogrammBildData = await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildDatensatzLadenAsync(i_i64ProgrammId, enmVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammBildData != null)
				{
					double.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo1, NumberStyles.Any, CultureInfo.InvariantCulture, out double result);
					double.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo2, NumberStyles.Any, CultureInfo.InvariantCulture, out double result2);
					int.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo3, out int result3);
					double.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo4, NumberStyles.Any, CultureInfo.InvariantCulture, out double result4);
					double.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo5, NumberStyles.Any, CultureInfo.InvariantCulture, out double result5);
					double.TryParse(eDC_LoetprogrammBildData.PRO_strZusatzinfo6, NumberStyles.Any, CultureInfo.InvariantCulture, out double result6);
					EDC_FiducialBilddaten value = new EDC_FiducialBilddaten
					{
						PROa_bytBilddaten = eDC_LoetprogrammBildData.PRO_bytBild,
						PRO_dblPositionX = result,
						PRO_dblPositionY = result2,
						PRO_i32BelichtungszeitRelativ = result3,
						PRO_dblManuelleKorrekturX = result4,
						PRO_dblManuelleKorrekturY = result5,
						PRO_dblOffsetZZuTransport = result6
					};
					dicBilder.Add(enmVerwendung, value);
				}
			}
			return dicBilder;
		}

		private async Task FUN_fdcEcp3BilderSpeichernAsync(long i_i64ProgrammId, EDC_Ecp3Bilddaten i_edcEcp3Bilddaten, IDbTransaction i_fdcTransaktion)
		{
			if (i_edcEcp3Bilddaten != null && i_edcEcp3Bilddaten.PRO_blnEcp3BildWurdeGeaendert)
			{
				if (string.IsNullOrEmpty(i_edcEcp3Bilddaten.PRO_strEcp3BildDateiName))
				{
					await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildVerwendungEntfernenAsync(i_i64ProgrammId, ENUM_BildVerwendung.enmEcp3Bild, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					return;
				}
				EDC_LoetprogrammBildData i_edcBilddata = new EDC_LoetprogrammBildData
				{
					PRO_i64ProgrammId = i_i64ProgrammId,
					PRO_strDateiname = i_edcEcp3Bilddaten.PRO_strEcp3BildDateiName,
					PRO_i32Verwendung = 3,
					PRO_bytBild = i_edcEcp3Bilddaten.PRO_bytEcp3BildDaten
				};
				await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(i_edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task FUN_fdcFiducialBilderSpeichernAsync(long i_i64ProgrammId, IDictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten> i_dicFiducialBilder, IDbTransaction i_fdcTransaktion)
		{
			if (i_dicFiducialBilder != null && i_dicFiducialBilder.Any((KeyValuePair<ENUM_BildVerwendung, EDC_FiducialBilddaten> i_fdcKvp) => i_fdcKvp.Value.PRO_blnBildGeandert))
			{
				foreach (KeyValuePair<ENUM_BildVerwendung, EDC_FiducialBilddaten> fdcKvp in i_dicFiducialBilder)
				{
					ENUM_BildVerwendung enmVerwendung = fdcKvp.Key;
					EDC_FiducialBilddaten edcFiducialBilddaten = fdcKvp.Value;
					if (edcFiducialBilddaten.PRO_blnBildGeandert && edcFiducialBilddaten.PROa_bytBilddaten == null)
					{
						await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildVerwendungEntfernenAsync(i_i64ProgrammId, enmVerwendung, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
						edcFiducialBilddaten.PRO_blnBildGeandert = false;
					}
					else
					{
						EDC_LoetprogrammBildData i_edcBilddata = new EDC_LoetprogrammBildData
						{
							PRO_i64ProgrammId = i_i64ProgrammId,
							PRO_strDateiname = fdcKvp.Key.ToString(),
							PRO_bytBild = edcFiducialBilddaten.PROa_bytBilddaten,
							PRO_i32Verwendung = (int)enmVerwendung,
							PRO_strZusatzinfo1 = edcFiducialBilddaten.PRO_dblPositionX.ToString(CultureInfo.InvariantCulture),
							PRO_strZusatzinfo2 = edcFiducialBilddaten.PRO_dblPositionY.ToString(CultureInfo.InvariantCulture),
							PRO_strZusatzinfo3 = edcFiducialBilddaten.PRO_i32BelichtungszeitRelativ.ToString(),
							PRO_strZusatzinfo4 = edcFiducialBilddaten.PRO_dblManuelleKorrekturX.ToString(CultureInfo.InvariantCulture),
							PRO_strZusatzinfo5 = edcFiducialBilddaten.PRO_dblManuelleKorrekturY.ToString(CultureInfo.InvariantCulture),
							PRO_strZusatzinfo6 = edcFiducialBilddaten.PRO_dblOffsetZZuTransport.ToString(CultureInfo.InvariantCulture)
						};
						await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(i_edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
						edcFiducialBilddaten.PRO_blnBildGeandert = false;
					}
				}
			}
		}

		private async Task FUN_fdcProfilBilderAktualisierenAsync(long i_i64ProgrammId, string i_strBildImportPfad, IDbTransaction i_fdcTransaktion)
		{
			if (string.IsNullOrEmpty(i_strBildImportPfad))
			{
				List<ENUM_BildVerwendung> i_enuVerwendungen = new List<ENUM_BildVerwendung>
				{
					ENUM_BildVerwendung.enmThumbnail,
					ENUM_BildVerwendung.enmVollbild,
					ENUM_BildVerwendung.enmVorschau
				};
				await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcLpBilderEntfernenAsync(i_i64ProgrammId, i_enuVerwendungen, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				using (Image fdcBild = Image.FromFile(i_strBildImportPfad))
				{
					EDC_LoetprogrammBildData edcBilddata = new EDC_LoetprogrammBildData
					{
						PRO_i64ProgrammId = i_i64ProgrammId,
						PRO_strDateiname = i_strBildImportPfad
					};
					Bitmap i_fdcBild = new Bitmap(FUN_fdcBildGroesseAnpassen(fdcBild, ENUM_BildVerwendung.enmVollbild));
					edcBilddata.PRO_i32Verwendung = 2;
					edcBilddata.PRO_bytBild = EDC_BildConverterHelfer.FUNa_bytImageToByteArray(i_fdcBild);
					await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					Bitmap i_fdcBild2 = new Bitmap(FUN_fdcBildGroesseAnpassen(fdcBild, ENUM_BildVerwendung.enmVorschau));
					edcBilddata.PRO_i32Verwendung = 1;
					edcBilddata.PRO_bytBild = EDC_BildConverterHelfer.FUNa_bytImageToByteArray(i_fdcBild2);
					await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					Bitmap i_fdcBild3 = new Bitmap(FUN_fdcBildGroesseAnpassen(fdcBild, ENUM_BildVerwendung.enmThumbnail));
					edcBilddata.PRO_i32Verwendung = 0;
					edcBilddata.PRO_bytBild = EDC_BildConverterHelfer.FUNa_bytImageToByteArray(i_fdcBild3);
					await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(edcBilddata, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		private Image FUN_fdcBildGroesseAnpassen(Image i_fdcBild, ENUM_BildVerwendung i_enmBildverwendung)
		{
			Tuple<int, int> tuple = m_dicBildGroessen[i_enmBildverwendung];
			return EDC_BildConverterHelfer.FUN_fdcPasseBildAnMaximaleGroesseAn(i_fdcBild, tuple.Item1, tuple.Item2);
		}

		private async Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammLadenAsync(long i_i64ProgrammId, long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammVersionAbfrageData edcVersion = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcVersionLadenAsync(i_i64VersionsId, i_i64MaschinenId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (edcVersion == null)
			{
				throw new InvalidDataException($"no valid version for program id = {i_i64ProgrammId} and version id={i_i64VersionsId}");
			}
			EDC_LoetprogrammData eDC_LoetprogrammData = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammData != null)
			{
				eDC_LoetprogrammData.PRO_blnIstFuerMaschineValide = edcVersion.PRO_blnValide;
			}
			EDC_LoetprogrammStand eDC_LoetprogrammStand = new EDC_LoetprogrammStand
			{
				PRO_i32SetNummer = edcVersion.PRO_i32SetNummer,
				PRO_strKommentar = edcVersion.PRO_strKommentar,
				PRO_blnIstValide = edcVersion.PRO_blnValide,
				PRO_edcProgrammInfo = eDC_LoetprogrammData
			};
			EDC_LoetprogrammStand eDC_LoetprogrammStand2 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand2.PRO_enuParameter = await m_edcLoetprogrammParameterDataAccess.Value.FUN_enuAlleParameterZuVersionLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand3 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand3.PRO_enuSatzdaten = await m_edcLoetprogrammSatzDatenDataAccess.Value.FUN_enuAlleSatzDatenZuVersionLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand4 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand4.PRO_enuNutzenParameter = await m_edcLoetprogrammNutzenDatenDataAccess.Value.FUN_fdcLadeNutzenDatenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand5 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand5.PRO_enuEcp3Daten = await m_edcLoetprogrammEcp3DatenDataAccess.Value.FUN_enuAlleEcp3DatenZuVersionLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand6 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand6.PRO_edcEcp3Bilddaten = await FUN_fdcEcp3BilddatenLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand7 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand7.PRO_dicFiducialBilder = await FUN_fdcFiducialBilddatenLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand8 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand8.PRO_enuCadVerboteneBereiche = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleVerbotenenBereicheLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand9 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand9.PRO_enuCadAblaufSchritte = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleAblaufSchritteLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand10 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand10.PRO_enuCadCncSchritte = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleCncSchritteLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand11 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand11.PRO_enuCadRoutenSchritte = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleRoutenSchritteLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand12 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand12.PRO_enuCadRoutenDaten = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleRoutenDatenLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand13 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand13.PRO_enuCadBewegungsGruppen = await m_edcCadDatenDataAccess.Value.FUN_fdcAlleBewegungsGruppenLadenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand14 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand14.PRO_enuCadBilder = await m_edcCadBildDataAccess.Value.FUN_fdcAlleBilddatenLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand eDC_LoetprogrammStand15 = eDC_LoetprogrammStand;
			eDC_LoetprogrammStand15.PRO_enuAoiSchritte = await m_edcAoiDataAccess.Value.FUN_fdcHoleAoiSchrittDatenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand edcLoetprogrammStand = eDC_LoetprogrammStand;
			EDC_CadEinstellungenData eDC_CadEinstellungenData = await m_edcCadEinstellungenDataAccess.Value.FUN_fdcHoleCadDatenAsync(edcVersion.PRO_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			edcLoetprogrammStand.PRO_strCadEinstellungen = ((eDC_CadEinstellungenData != null) ? eDC_CadEinstellungenData.PRO_strEinstellungen : string.Empty);
			edcLoetprogrammStand.PRO_strCadDaten = ((eDC_CadEinstellungenData != null) ? eDC_CadEinstellungenData.PRO_strDaten : string.Empty);
			EDC_AoiProgramData eDC_AoiProgramData = await m_edcAoiDataAccess.Value.FUN_fdcHoleAoiProgrammAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			edcLoetprogrammStand.PRO_strAoiEinstellungen = ((eDC_AoiProgramData != null) ? eDC_AoiProgramData.PRO_strEinstellungen : string.Empty);
			edcLoetprogrammStand.PRO_strAoiDaten = ((eDC_AoiProgramData != null) ? eDC_AoiProgramData.PRO_strDaten : string.Empty);
			return edcLoetprogrammStand;
		}

		private async Task<long> FUN_fdcLoetprogrammDuplizierenAsync(long i_i64ProgrammId, long i_i64ZielBibliotheksId, string i_strNeuerProgrammName, long i_i64UrsprungsVerionsId, IDbTransaction i_fdcTransaktion)
		{
			if (i_i64UrsprungsVerionsId == 0L)
			{
				return 0L;
			}
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammStand edcKopieDesLetztenStandes = await FUN_fdcLoetprogrammLadenAsync(i_i64ProgrammId, i_i64UrsprungsVerionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (edcKopieDesLetztenStandes == null)
			{
				throw new InvalidDataException("Library has no released program versions. The process was rolled back!");
			}
			edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64ProgrammId = 0L;
			edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_strName = i_strNeuerProgrammName;
			edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64BibliotheksId = i_i64ZielBibliotheksId;
			edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64AngelegtVon = i64BenutzerId;
			edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64BearbeitetVon = i64BenutzerId;
			await FUN_fdcLoetprogrammSpeichernAsync(edcKopieDesLetztenStandes, null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await m_edcLoetprogrammVersionDataAccess.Value.FUN_fdcArbeitsVersionLadenAsync(edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcLoetprogrammValideDataDataAccess.Value.FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(eDC_LoetprogrammVersionData.PRO_i64VersionsId, i64MaschinenId, edcKopieDesLetztenStandes.PRO_blnIstValide, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			foreach (ENUM_BildVerwendung value in Enum.GetValues(typeof(ENUM_BildVerwendung)))
			{
				EDC_LoetprogrammBildData eDC_LoetprogrammBildData = await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildDatensatzLadenAsync(i_i64ProgrammId, value, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_LoetprogrammBildData != null)
				{
					eDC_LoetprogrammBildData.PRO_i64ProgrammId = edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64ProgrammId;
					await m_edcLoetprogrammBildDataAccess.Value.FUN_fdcBildAktualisierenAsync(eDC_LoetprogrammBildData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			return edcKopieDesLetztenStandes.PRO_edcProgrammInfo.PRO_i64ProgrammId;
		}

		private async Task<EDC_BibliothekInfo> FUN_edcBibliothekMitProgrammenLadenAsync(EDC_LoetprogrammBibliothekData i_edcBibliothekData, string i_strSuchbegriff = null)
		{
			IList<EDC_ProgrammInfo> i_enuElemente = await FUN_lstProgrammeLadenAsync(await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false), i_edcBibliothekData.PRO_i64BibliothekId, i_strSuchbegriff).ConfigureAwait(continueOnCapturedContext: false);
			EDC_BibliothekInfo eDC_BibliothekInfo = new EDC_BibliothekInfo(i_edcBibliothekData.PRO_strName, i_edcBibliothekData.PRO_i64BibliothekId);
			eDC_BibliothekInfo.PRO_lstProgramme.SUB_AddRange(i_enuElemente);
			return eDC_BibliothekInfo;
		}

		private async Task<IList<EDC_ProgrammInfo>> FUN_lstProgrammeLadenAsync(long i_i64MaschinenId, long i_i64BibId, string i_strSuchbegriff)
		{
			List<EDC_ProgrammInfo> lstPrgs = new List<EDC_ProgrammInfo>();
			if (i_i64BibId == 0L)
			{
				return lstPrgs;
			}
			List<EDC_LoetprogrammInfoDataAbfrage> source = (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleNichtGeloeschteLoetprgInfoListeVonBibliotheksIdAsync(i_i64BibId, i_i64MaschinenId, i_strSuchbegriff).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			foreach (long i64ProgrammId in (from i_edcItem in source
			select i_edcItem.PRO_i64ProgrammId).Distinct())
			{
				try
				{
					EDC_ProgrammInfo eDC_ProgrammInfo = FUN_edcErstelleProgrammInfoAusInfoAbfrage((from i_edcItem in source
					where i_edcItem.PRO_i64ProgrammId == i64ProgrammId
					select i_edcItem).ToList());
					if (eDC_ProgrammInfo != null)
					{
						lstPrgs.Add(eDC_ProgrammInfo);
					}
				}
				catch (Exception i_fdcEx)
				{
					string i_strMessage = $"Error loading a soldering program: Programm with Id {i64ProgrammId} cannot be loaded.";
					SUB_FehlerLoggen(i_fdcEx, MethodBase.GetCurrentMethod().Name, i_strMessage);
				}
			}
			return lstPrgs;
		}

		private EDC_ProgrammInfo FUN_edcErstelleProgrammInfoAusInfoAbfrage(IEnumerable<EDC_LoetprogrammInfoDataAbfrage> i_enuAbfrage)
		{
			List<EDC_LoetprogrammInfoDataAbfrage> source = i_enuAbfrage.ToList();
			if (!source.Any())
			{
				return null;
			}
			Tuple<ENUM_LoetprogrammStatus, ENUM_LoetprogrammFreigabeStatus, bool>[] array = (from i_edcItem in source
			select new Tuple<ENUM_LoetprogrammStatus, ENUM_LoetprogrammFreigabeStatus, bool>(i_edcItem.PRO_enmProgrammStatus, i_edcItem.PRO_enmFreigabeStatus, !i_edcItem.PRO_blnValide)).Distinct().ToArray();
			List<ENUM_LoetprogrammStatus> list = new List<ENUM_LoetprogrammStatus>();
			List<ENUM_LoetprogrammFreigabeStatus> list2 = new List<ENUM_LoetprogrammFreigabeStatus>();
			List<bool> list3 = new List<bool>();
			Tuple<ENUM_LoetprogrammStatus, ENUM_LoetprogrammFreigabeStatus, bool>[] array2 = array;
			foreach (Tuple<ENUM_LoetprogrammStatus, ENUM_LoetprogrammFreigabeStatus, bool> tuple in array2)
			{
				list.Add(tuple.Item1);
				list2.Add(tuple.Item2);
				list3.Add(tuple.Item3);
			}
			return EDC_ProgrammInfoKonvertierungsHelfer.FUN_edcProgrammInfoKonvertieren(source.FirstOrDefault(), list, list2, list3);
		}

		private async Task<EDC_VersionsInfo> FUN_edcVersionKonvertierenAsync(EDC_LoetprogrammVersionAbfrageData i_edcVersion)
		{
			EDC_VersionsInfo edcVersion = EDC_LoetprogrammVersionsInfoKonvertierungsHelfer.FUN_edcKonvertieren(i_edcVersion, PRO_edcSerialisierer);
			EDC_VersionsInfo eDC_VersionsInfo = edcVersion;
			string text2 = eDC_VersionsInfo.PRO_strBenutzer = await FUN_strBenutzerNameErmittelnAsync(i_edcVersion.PRO_i64AngelegtVon).ConfigureAwait(continueOnCapturedContext: false);
			eDC_VersionsInfo = edcVersion;
			text2 = (eDC_VersionsInfo.PRO_strBearbeitungsBenutzer = await FUN_strBenutzerNameErmittelnAsync(i_edcVersion.PRO_i64BearbeitetVon).ConfigureAwait(continueOnCapturedContext: false));
			return edcVersion;
		}

		private async Task FUN_fdcPruefeObPrgNameInBibBereitsExistiertAsync(long i_i64BibliotheksId, string i_strProgrammName, IDbTransaction i_fdcTransaktion)
		{
			if (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_strProgrammName, i_i64BibliotheksId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != null)
			{
				throw new EDC_LoetprogrammBereitsInBibliothekEnthaltenException(i_strProgrammName, (await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(i_i64BibliotheksId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_strName);
			}
		}

		private async Task<string> FUN_strBenutzerNameErmittelnAsync(long i_i64BenutzerId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_BenutzerAbfrageData eDC_BenutzerAbfrageData = await m_edcBenutzerVerwaltungDataAccess.Value.FUN_fdcMaschinenBenutzerLadenAsync(i_i64BenutzerId, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			return (eDC_BenutzerAbfrageData != null) ? eDC_BenutzerAbfrageData.PRO_strBenutzername : "unknown";
		}
	}
}
