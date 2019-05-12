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

		Task<bool> FUN_fdcResetMessageAsync(INF_Meldung i_edcMessage);
	}
}
