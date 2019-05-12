using Ersa.Global.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_CsvSerialisierungsDienst))]
	public class EDC_CsvSerialisierungsDienst : INF_CsvSerialisierungsDienst
	{
		public string FUN_strSerialisieren(EDC_CsvDaten i_edcDaten)
		{
			string text = string.Join(Environment.NewLine, i_edcDaten.PRO_enuDateikopf);
			string text2 = string.Join(i_edcDaten.PRO_strTrennzeichen, i_edcDaten.PRO_enuHeaderSpalten);
			string text3 = string.Join(Environment.NewLine, from i_edcDatensatz in i_edcDaten.PRO_enuDaten
			select string.Join(i_edcDaten.PRO_strTrennzeichen, i_edcDatensatz.PRO_enuDaten));
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				list.Add(text2);
			}
			if (!string.IsNullOrEmpty(text3))
			{
				list.Add(text3);
			}
			return string.Join(Environment.NewLine, list);
		}
	}
}
