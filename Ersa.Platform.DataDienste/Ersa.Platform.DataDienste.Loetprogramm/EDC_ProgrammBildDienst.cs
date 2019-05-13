using Ersa.Global.Common.Helper;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using Ersa.Platform.DataDienste.Loetprogramm.Exceptions;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ersa.Platform.DataDienste.Loetprogramm
{
	[Export(typeof(INF_ProgrammBildDienst))]
	public class EDC_ProgrammBildDienst : INF_ProgrammBildDienst
	{
		private readonly INF_IODienst m_edcIoDienst;

		private readonly Lazy<INF_LoetprogrammBildDataAccess> m_edcProgrammBildDataAccess;

		[ImportingConstructor]
		public EDC_ProgrammBildDienst(INF_IODienst i_edcIoDienst, INF_DataAccessProvider i_edcDataAccessProvider)
		{
			m_edcIoDienst = i_edcIoDienst;
			m_edcProgrammBildDataAccess = new Lazy<INF_LoetprogrammBildDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammBildDataAccess>);
		}

		public string FUN_strFindeDefaultPlatinenBildImVerzeichnis(string i_strPfad)
		{
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strPfad))
			{
				throw new DirectoryNotFoundException($"Directory {i_strPfad} was not found");
			}
			DirectoryInfo @object = new DirectoryInfo(i_strPfad);
			List<FileInfo> source = EDC_ProgrammKonstanten.ms_lstDefaultDateiExtensionsBild.SelectMany(@object.GetFiles).ToList();
			try
			{
				FileInfo fileInfo = source.SingleOrDefault((FileInfo i_fdcBilddatei) => i_fdcBilddatei.Name.StartsWith("ERSA.", ignoreCase: true, CultureInfo.InvariantCulture));
				return (fileInfo != null) ? fileInfo.FullName : string.Empty;
			}
			catch (InvalidOperationException)
			{
				List<string> list = (from i_fdcFileInfo in source
				select i_fdcFileInfo.FullName).ToList();
				throw new EDC_MehrereBildDateienGefundenException(string.Format("Multiple image files were found {0}, could not decide which to use", string.Join(",", list)))
				{
					PRO_lstDateiNamen = list
				};
			}
		}

		public Task<Bitmap> FUN_fdcBildLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung)
		{
			return m_edcProgrammBildDataAccess.Value.FUN_fdcBildLadenAsync(i_i64LoetprogrammId, i_enmVerwendung);
		}

		public async Task<BitmapImage> FUN_fdcBildLadenUndNachBitmapImageKonvertierenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung)
		{
			if (i_i64LoetprogrammId == 0L)
			{
				return null;
			}
			byte[] array = await m_edcProgrammBildDataAccess.Value.FUN_fdcBildArrayLadenAsync(i_i64LoetprogrammId, i_enmVerwendung).ConfigureAwait(continueOnCapturedContext: true);
			if (array != null)
			{
				return EDC_BildConverterHelfer.FUN_edcByteArrayNachBitmapImageKonvertieren(array);
			}
			return null;
		}
	}
}
