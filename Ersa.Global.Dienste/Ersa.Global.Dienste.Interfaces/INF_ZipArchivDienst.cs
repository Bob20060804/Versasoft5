namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_ZipArchivDienst
	{
		void SUB_ZipArchivErstellen(string i_strQuellPfad, EDC_ZipErstellungsOptionen i_edcErstellungsOptionen);

		void SUB_ZipArchivEntpacken(string i_strQuellDateiPfad, string i_strZielPfad);
	}
}
