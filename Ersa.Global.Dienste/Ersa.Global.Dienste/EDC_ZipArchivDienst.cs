using Ersa.Global.Dienste.Interfaces;
using Ionic.Zip;
using System.ComponentModel.Composition;
using System.IO;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_ZipArchivDienst))]
	public class EDC_ZipArchivDienst : INF_ZipArchivDienst
	{
		private readonly INF_IODienst m_edcIoDienst;

		[ImportingConstructor]
		public EDC_ZipArchivDienst(INF_IODienst i_edcIoDienst)
		{
			m_edcIoDienst = i_edcIoDienst;
		}

		public void SUB_ZipArchivErstellen(string i_strQuellPfad, EDC_ZipErstellungsOptionen i_edcErstellungsOptionen)
		{
			if (!i_edcErstellungsOptionen.PRO_blnElementeHinzufuegen && m_edcIoDienst.FUN_blnDateiExistiert(i_edcErstellungsOptionen.PRO_strZielDateiPfad))
			{
				m_edcIoDienst.SUB_DateiLoeschen(i_edcErstellungsOptionen.PRO_strZielDateiPfad);
			}
			string directoryName = Path.GetDirectoryName(i_edcErstellungsOptionen.PRO_strZielDateiPfad);
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(directoryName))
			{
				m_edcIoDienst.SUB_VerzeichnisErstellen(directoryName);
			}
			using (ZipFile zipFile = FUN_fdcZipFileFuerErstellungErzeugen(i_edcErstellungsOptionen))
			{
				zipFile.AddDirectory(i_strQuellPfad);
				zipFile.Save();
			}
		}

		public void SUB_ZipArchivEntpacken(string i_strQuellDateiPfad, string i_strZielPfad)
		{
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strZielPfad))
			{
				m_edcIoDienst.SUB_VerzeichnisErstellen(i_strZielPfad);
			}
			using (ZipFile zipFile = ZipFile.Read(i_strQuellDateiPfad))
			{
				zipFile.ExtractAll(i_strZielPfad);
			}
		}

		private ZipFile FUN_fdcZipFileFuerErstellungErzeugen(EDC_ZipErstellungsOptionen i_edcOptionen)
		{
			return new ZipFile(i_edcOptionen.PRO_strZielDateiPfad)
			{
				ParallelDeflateThreshold = -1L,
				MaxOutputSegmentSize = ((i_edcOptionen.PRO_i32MaxGroesseInKb * 1024) ?? 0)
			};
		}
	}
}
