using Ersa.Platform.Common.Loetprotokoll;

namespace Ersa.Platform.Dienste.Loetprotokoll.Interfaces
{
	public interface INF_LoetprotokollSerialisierungsDienst
	{
		void SUB_LoetprotokollSerialisieren(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen);
	}
}
