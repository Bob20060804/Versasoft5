using Ersa.Platform.Common.Data.Maschinenverwaltung;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces
{
	public interface INF_MaschinenVerwaltungsDienst
	{
		Task<string> FUN_strMaschinenNummerErmittelnAsync();

		Task<string> FUN_strHoleDefaultGruppenNameAsync();

		Task<long[]> FUNa_i64AktiveGruppenIdsErmittelnAsync();

		Task<EDC_MaschineData> FUN_edcMaschinenDatenErmittelnAsync();

		Task FUN_fdcMaschinenDatenSpeichernAsync(EDC_MaschineData i_edcMaschinenDaten);

		Task<bool> FUN_fdcSindMehrereGleicheMaschinentypenRegistriertAsync();

		Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_enuGruppenFuerMaschinenTypErmittelnAsync();

		Task<long> FUN_i64GruppeErstellenAsync(string i_strGruppenName);

		Task FUN_fdcGruppeUmbenennenAsync(long i_i64GruppenId, string i_strNeuerName);

		Task FUN_fdcAktiveGruppenIdsSetzenAsync(long[] ia_i64GruppenIds);

		IEnumerable<Func<Task>> FUN_enuMaschinenDatenSichernOperationenErstellen(string i_strPfad, long i_i64BenutzerId);

		IEnumerable<Func<Task>> FUN_enuDefaultMaschinenDatenSichernOperationenErstellen(string i_strPfad, long i_i64BenutzerId);

		IEnumerable<Func<Task>> FUN_enuMaschinenDatenLadenOperationenErstellen(string i_strPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcProgress = null);

		Task FUN_fdcMaschinenDatenLadenAsync(string i_strPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcProgress = null);
	}
}
