using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Dienste.Loetprogramm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;

namespace Ersa.Platform.Dienste.Loetprogramm
{
	[Export(typeof(INF_ProgrammInfoInterpreter))]
	public class EDC_ProgrammInfoInterpreter : INF_ProgrammInfoInterpreter
	{
		[ImportingConstructor]
		public EDC_ProgrammInfoInterpreter()
		{
		}

		public EDC_ProgrammInfo FUN_edcProgrammInfoInterpretieren(IList<string> i_lstDateiInhalt)
		{
			return new EDC_ProgrammInfo
			{
				PRO_strFirmenname = i_lstDateiInhalt[0],
				PRO_strAnwendungsVersionsInfo = i_lstDateiInhalt[1],
				PRO_dtmDatum = DateTime.ParseExact(i_lstDateiInhalt[2].Substring(5), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
				PRO_strBenutzername = i_lstDateiInhalt[3].Substring(5),
				PRO_strProgrammName = i_lstDateiInhalt[4],
				PRO_i32SetNummer = int.Parse(i_lstDateiInhalt[5].Substring(8)),
				PRO_strKommentar = string.Join(Environment.NewLine, i_lstDateiInhalt.Skip(9).Where(delegate(string i_strZeile)
				{
					if (!string.IsNullOrEmpty(i_strZeile))
					{
						return i_strZeile.Any((char i_chrZeichen) => i_chrZeichen != '\0');
					}
					return false;
				}))
			};
		}

		public bool FUN_blnKannInterpretieren(IList<string> i_lstDateiInhalt)
		{
			if (i_lstDateiInhalt != null && i_lstDateiInhalt.Any())
			{
				return i_lstDateiInhalt[1].StartsWith("V003");
			}
			return false;
		}
	}
}
