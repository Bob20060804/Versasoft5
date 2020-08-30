using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Loetprotokoll;
using Ersa.Platform.Dienste.Loetprotokoll.Interfaces;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Export(typeof(INF_LoetprotokollSerialisierer))]
	public class EDC_LoetprotokollJsonSerialisierer : INF_LoetprotokollSerialisierer
	{
		[Import("Ersa.JsonSerialisierer", typeof(INF_SerialisierungsDienst))]
		public INF_SerialisierungsDienst PRO_edcSerialisierungsDienst
		{
			get;
			set;
		}

		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		public string PRO_strSerialisiererName => "default";

		public string PRO_strDefaultDateiEndung => ".json";

		public void SUB_LoetprotokollSerialisieren(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			string text = PRO_edcSerialisierungsDienst.FUN_strSerialisieren(i_edcLoetprotokoll);
			PRO_edcIoDienst.SUB_DateiInhaltSchreiben(i_edcDateiEinstellungen.PRO_strPfadUndDateiName, text);
			if (i_edcDateiEinstellungen.PRO_blnErgebnisValidieren)
			{
				PRO_edcSerialisierungsDienst.SUB_ValidiereGegenSchemaDatei(text, i_edcDateiEinstellungen.PRO_strSchemaDatei);
			}
		}
	}
}
