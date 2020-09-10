using Ersa.Platform.Common;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Loetprogramm.Helfer;
using Ersa.Platform.Dienste.Loetprogramm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;

namespace Ersa.Platform.Dienste.Loetprogramm
{
	[Export(typeof(INF_ProgrammInfoSerialisierer))]
	public class EDC_ProgrammInfoSerialisierer : INF_ProgrammInfoSerialisierer
	{
		private const int mC_i32ProgrammInfoVersion = 3;

		private const string mC_strDatumTag = "date";

		private const string mC_strBenutzerTag = "user";

		private const string mC_strNameTag = "name";

		private const string mC_strVersionTag = "version";

		private readonly INF_BenutzerInfoProvider m_edcBenutzerInfoProvider;

		[ImportingConstructor]
		public EDC_ProgrammInfoSerialisierer(INF_BenutzerInfoProvider i_edcBenutzerInfoProvider)
		{
			m_edcBenutzerInfoProvider = i_edcBenutzerInfoProvider;
		}

		public bool FUN_blnKannSerialisieren(EDC_ProgrammInfo i_edcProgrammInfo)
		{
			return i_edcProgrammInfo != null;
		}

		public IList<string> FUN_lstProgrammInfoSerialisieren(EDC_ProgrammInfo i_edcProgrammInfo, EDC_ElementVersion i_edcVisuVersion)
		{
			i_edcProgrammInfo.PRO_dtmDatum = DateTime.Now;
			i_edcProgrammInfo.PRO_strBenutzername = m_edcBenutzerInfoProvider.PRO_strAktiverBenutzer;
			i_edcProgrammInfo.PRO_strAnwendungsVersionsInfo = EDC_SerialisierungsVersionsHelfer.FUN_strSerialisierungsVersionErmitteln(3, i_edcVisuVersion);
			IList<string> list = new List<string>();
			list.Add(i_edcProgrammInfo.PRO_strFirmenname);
			list.Add(i_edcProgrammInfo.PRO_strAnwendungsVersionsInfo);
			list.Add(string.Format("{0};{1}", "date", i_edcProgrammInfo.PRO_dtmDatum.ToString("dd.MM.yyyy HH:mm:ss")));
			list.Add(string.Format("{0};{1}", "user", i_edcProgrammInfo.PRO_strBenutzername));
			list.Add(string.Format("{0};{1}", "name", i_edcProgrammInfo.PRO_strProgrammName));
			list.Add(string.Format("{0};{1}", "version", i_edcProgrammInfo.PRO_i32SetNummer.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')));
			list.Add(string.Empty);
			list.Add(string.Empty);
			list.Add(string.Empty);
			foreach (string item in (IEnumerable<string>)i_edcProgrammInfo.PRO_strKommentar.Split(new string[1]
			{
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				list.Add(item);
			}
			return list;
		}
	}
}
