using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_FTPDienst
	{
		void SUB_Initialisieren(EDC_FtpKonfiguration i_edcFtpKonfiguration);

		Task SUB_DownloadAsync(string i_strQuellPfad, string i_strZielPfad);

		void SUB_Download(string i_strQuellPfad, string i_strZielPfad);

		void SUB_DownloadAlleFiles(string i_strQuellPfad, string i_strZielPfad);

		Task SUB_UploadAsync(string i_strQuellPfad, string i_strZielPfad);

		Task SUB_UploadAsync(IList<string> i_lstDateiInhalt, string i_strZielPfad);

		void SUB_Upload(string i_strQuellPfad, string i_strZielPfad);

		void SUB_Upload(IList<string> i_lstDateiInhalt, string i_strZielPfad);

		void SUB_DateiLoeschen(string i_strDateiPfad);

		Task FUN_fdcDateiLoeschen(string i_strDateiPfad);

		void SUB_VerzeichnisLeeren(string i_strVerzeichnisPfad);

		Task FUN_fdcVerzeichnisLeerenAsync(string i_strVerzeichnisPfad);
	}
}
