using Ersa.Platform.Common.Loetprotokoll;

namespace Ersa.Platform.Dienste.Loetprotokoll.Interfaces
{
	public interface INF_LoetprotokollSerialisierer
	{
		string PRO_strSerialisiererName
		{
			get;
		}

		string PRO_strDefaultDateiEndung
		{
			get;
		}

		void SUB_LoetprotokollSerialisieren(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen);
	}
}
