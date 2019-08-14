using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Data.Loetprotokoll;
using System.Collections.Generic;

namespace Ersa.Platform.DataContracts.Loetprotokoll
{
	public interface INF_LoetParameterIstSollConverter
	{
		void SUB_SetzteLoetprogrammSollwerte(Dictionary<long, Dictionary<string, EDC_LoetprogrammParameterData>> i_dicSollwerte, List<EDC_LoetprotokollAbfrageErgebnis> i_edcAbfrageErgebnisse, Dictionary<long, string> i_dicRuestGruppenDaten);
	}
}
