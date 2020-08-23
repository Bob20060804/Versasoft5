using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Meldungen
{
	public interface INF_MeldungProduzentCapability
	{
		/// <summary>
		/// 消息产生者
		/// message producer
		/// </summary>
		ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
		}

		/// <summary>
		/// 是否配置
		/// </summary>
		/// <returns></returns>
		bool FUN_blnIstKonfiguriert();

		/// <summary>
		/// 请求处理消息异步
		/// Handle Message Request Async
		/// </summary>
		/// <param name="i_edcMeldung"></param>
		/// <param name="i_enmAktion"></param>
		/// <returns></returns>
		Task FUN_fdcMeldungBehandelnAnfordernAsync(INF_Meldung i_edcMeldung, ENUM_MeldungAktionen i_enmAktion);

		/// <summary>
		/// 确认消息异步
		/// Determine messages to be acknowledged Async
		/// </summary>
		/// <param name="i_enuNichQuittierteMeldungen"></param>
		/// <returns></returns>
		Task<IEnumerable<INF_Meldung>> FUN_fdcErmittleZuQuittierendeMeldungenAsync(IEnumerable<INF_Meldung> i_enuNichQuittierteMeldungen);
	}
}
