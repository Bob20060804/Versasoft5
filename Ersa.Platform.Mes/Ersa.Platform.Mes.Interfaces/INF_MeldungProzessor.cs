using Ersa.Platform.Common.Meldungen;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
    /// <summary>
    /// 消息处理器 message processor
    /// </summary>
	public interface INF_MeldungProzessor
	{
		Task<bool> FUN_fdcAcknowledgeMessageAsync(INF_Meldung i_edcMessage);

        /// <summary>
        /// 重置消息
        /// </summary>
        /// <param name="i_edcMessage"></param>
        /// <returns></returns>
		Task<bool> FUN_fdcResetMessageAsync(INF_Meldung i_edcMessage);
	}
}
