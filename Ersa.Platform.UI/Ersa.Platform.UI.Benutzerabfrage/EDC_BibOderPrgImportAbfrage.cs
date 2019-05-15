using Ersa.Platform.Common.Loetprogramm;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_BibOderPrgImportAbfrage : EDC_BenutzerAbfrage<bool>
	{
		private readonly Dictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageFormatItem> m_dicAbfrageItems = new Dictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageFormatItem>();

		public string PRO_strStandardVerzeichnis
		{
			get;
			set;
		}

		public IEnumerable<EDC_BibliothekIdentifier> PRO_enuExistierendeBibs
		{
			get;
			set;
		}

		public void SUB_FuegeAbfrageFormatItemHinzu(ENUM_ImportFormat i_enmFormat, EDC_BibOderPrgImportAbfrageFormatItem i_edcAbfrageFormatItem)
		{
			if (!m_dicAbfrageItems.ContainsKey(i_enmFormat))
			{
				m_dicAbfrageItems.Add(i_enmFormat, i_edcAbfrageFormatItem);
			}
		}

		public EDC_BibOderPrgImportAbfrageFormatItem FUN_edcHoleBibOderPrgAbfrageFuerFormat(ENUM_ImportFormat i_enmFormat)
		{
			if (m_dicAbfrageItems.TryGetValue(i_enmFormat, out EDC_BibOderPrgImportAbfrageFormatItem value))
			{
				return value;
			}
			return null;
		}

		public IEnumerable<ENUM_ImportFormat> FUN_enuHoleVerfuegbareImportFormate()
		{
			return m_dicAbfrageItems.Keys;
		}
	}
}
