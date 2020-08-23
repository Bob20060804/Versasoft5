using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Meldungen
{
	public interface INF_MeldungProduzentCapability
	{
		/// <summary>
		/// ��Ϣ������
		/// message producer
		/// </summary>
		ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
		}

		/// <summary>
		/// �Ƿ�����
		/// </summary>
		/// <returns></returns>
		bool FUN_blnIstKonfiguriert();

		/// <summary>
		/// ��������Ϣ�첽
		/// Handle Message Request Async
		/// </summary>
		/// <param name="i_edcMeldung"></param>
		/// <param name="i_enmAktion"></param>
		/// <returns></returns>
		Task FUN_fdcMeldungBehandelnAnfordernAsync(INF_Meldung i_edcMeldung, ENUM_MeldungAktionen i_enmAktion);

		/// <summary>
		/// ȷ����Ϣ�첽
		/// Determine messages to be acknowledged Async
		/// </summary>
		/// <param name="i_enuNichQuittierteMeldungen"></param>
		/// <returns></returns>
		Task<IEnumerable<INF_Meldung>> FUN_fdcErmittleZuQuittierendeMeldungenAsync(IEnumerable<INF_Meldung> i_enuNichQuittierteMeldungen);
	}
}
