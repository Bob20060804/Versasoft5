using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Betriebsfall
{
	public interface INF_BetriebsfallmportExportIDataAccess : INF_DataAccess
	{
		Task FUN_fdcExportBetriebsdatenDataAsync(string i_strExportpfad, string i_strDateiname, long i_i64MaschinenId);

		Task FUN_fdcImportBetriebsdatenDataAsync(string i_strImportDatei);
	}
}
