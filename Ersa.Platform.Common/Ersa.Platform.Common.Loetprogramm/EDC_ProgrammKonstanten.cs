using System.Collections.Generic;

namespace Ersa.Platform.Common.Loetprogramm
{
	public static class EDC_ProgrammKonstanten
	{
		public const string mC_strDefaultDateiNameBild = "ERSA";

		public const int mC_i32BildMaximalDimension = 500;

		public const string gC_strNameDefaultBibliothek = "[defaultLibrary]";

		public const string gC_strDefaultDateiNameProginfoDaten = "Proginfo.txt";

		public const string gC_strDatumsFormatFuerProgInfo = "dd.MM.yyyy HH:mm:ss";

		public const string gC_strProgrammInfoAlsFehlerhaftSpeichernAsync = "Ersa.Platform.Dienste.Loetprogramm.EDC_LoetprogrammVerwaltungsDienst.FUN_fdcProgInfoAlsFehlerhaftSpeichernAsync";

		public static readonly IList<string> ms_lstDefaultDateiExtensionsBild = new List<string>
		{
			"*.jpg",
			"*.png",
			"*.bmp"
		};
	}
}
