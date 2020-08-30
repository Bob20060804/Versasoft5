using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Loetprotokoll;
using Ersa.Platform.Dienste.Loetprotokoll.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Export(typeof(INF_LoetprotokollSerialisierer))]
	public class EDC_LoetprotokollZveiSerialisierer : INF_LoetprotokollSerialisierer
	{
		[Import("Ersa.XmlSerialisierer", typeof(INF_SerialisierungsDienst))]
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

		[Import]
		public EDC_ProtokollParameterNamenMapper PRO_edcProtokolLParameterNamenMapper
		{
			get;
			set;
		}

		public string PRO_strSerialisiererName => "ZVEI";

		public string PRO_strDefaultDateiEndung => ".XML";

		public void SUB_LoetprotokollSerialisieren(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			string text = FUN_strErstellerSerialisierteDaten(i_edcLoetprotokoll, i_edcDateiEinstellungen);
			PRO_edcIoDienst.SUB_DateiInhaltSchreiben(i_edcDateiEinstellungen.PRO_strPfadUndDateiName, text);
			if (i_edcDateiEinstellungen.PRO_blnErgebnisValidieren)
			{
				PRO_edcSerialisierungsDienst.SUB_ValidiereGegenSchemaDatei(text, i_edcDateiEinstellungen.PRO_strSchemaDatei);
			}
		}

		public void SUB_LoetprotokollSerialisieren(IEnumerable<EDC_LoetprotokollDaten> i_enmLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			string text = FUN_strErstellerSerialisierteDaten(i_enmLoetprotokoll, i_edcDateiEinstellungen);
			PRO_edcIoDienst.SUB_DateiInhaltSchreiben(i_edcDateiEinstellungen.PRO_strPfadUndDateiName, text);
			if (i_edcDateiEinstellungen.PRO_blnErgebnisValidieren)
			{
				PRO_edcSerialisierungsDienst.SUB_ValidiereGegenSchemaDatei(text, i_edcDateiEinstellungen.PRO_strSchemaDatei);
			}
		}

		private string FUN_strErstellerSerialisierteDaten(IEnumerable<EDC_LoetprotokollDaten> i_enmLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			EDC_LoetprotokollZveiUnitData i_objObjekt = new EDC_LoetprotokollZveiUnitDataMapper(PRO_edcProtokolLParameterNamenMapper).FUN_edcVerarbeiteDaten(i_enmLoetprotokoll, i_edcDateiEinstellungen);
			return PRO_edcSerialisierungsDienst.FUN_strSerialisieren(i_objObjekt);
		}

		private string FUN_strErstellerSerialisierteDaten(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			EDC_LoetprotokollZveiUnitData i_objObjekt = new EDC_LoetprotokollZveiUnitDataMapper(PRO_edcProtokolLParameterNamenMapper).FUN_edcVerarbeiteDaten(i_edcLoetprotokoll, i_edcDateiEinstellungen);
			return PRO_edcSerialisierungsDienst.FUN_strSerialisieren(i_objObjekt);
		}
	}
}
