using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ersa.Platform.DataDienste.Loetprogramm.Interfaces
{
	public interface INF_ProgrammBildDienst
	{
		string FUN_strFindeDefaultPlatinenBildImVerzeichnis(string i_strPfad);

		Task<Bitmap> FUN_fdcBildLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung);

		Task<BitmapImage> FUN_fdcBildLadenUndNachBitmapImageKonvertierenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung);
	}
}
