using Ersa.Platform.Plc.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc
{
	public interface INF_Sps : IDisposable
	{
        /// <summary>
        /// 异步建立链接
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <param name="i_strAdresse"></param>
        /// <returns></returns>
		Task FUN_fdcVerbindungAufbauenAsync(bool i_blnOnline, string i_strAdresse);

        /// <summary>
        /// DisConnect
        /// </summary>
		void SUB_VerbindungLoesen();

        /// <summary>
        /// Read Value
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		string FUN_strWertLesen(string i_strVarName);
		float FUN_sngWertLesen(string i_strVarName);
		uint FUN_u32WertLesen(string i_strVarName);
		int FUN_i32WertLesen(string i_strVarName);
		short FUN_i16WertLesen(string i_strVarName);
		ushort FUN_u16WertLesen(string i_strVarName);
		byte FUN_bytWertLesen(string i_strVarName);
		bool FUN_blnWertLesen(string i_strVarName);

        /// <summary>
        /// Write value
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <param name="i_strWert"></param>
		void SUB_WertSchreiben(string i_strVarName, string i_strWert);

        /// <summary>
        /// 事件注册
        /// EventHandler Register
        /// </summary>
        /// <param name="i_strVarName">变量名</param>
        /// <param name="i_delHandler">事件</param>
        /// <returns></returns>
		IDisposable FUN_fdcEventHandlerRegistrieren(string i_strVarName, Action i_delHandler);

        /// <summary>
        /// 变量 注册
        /// Variables Register async
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task FUN_fdcVariablenAnmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

        /// <summary>
        /// 变量 登出
        /// Variables Unregister async
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task FUN_fdcVariablenAbmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

        /// <summary>
        /// 组事件注册
        /// Activate event group
        /// </summary>
		void SUB_EventGruppeAktivieren();

        /// <summary>
        /// 组事件注销
        /// Deactivate event group
        /// </summary>
		void SUB_EventGruppeDeaktivieren();

        /// <summary>
        /// 创建Group变量 
        /// Create Group Variable Async
        /// </summary>
        /// <param name="i_enmVariablen"></param>
        /// <param name="i_strGruppenName"></param>
        /// <param name="i_i32CycleTime"></param>
        /// <returns></returns>
		Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<string> i_enmVariablen, string i_strGruppenName, int i_i32CycleTime = 100);

        /// <summary>
        /// Group writing Async
        /// </summary>
        /// <param name="i_enmParameter"></param>
        /// <param name="i_strGruppenName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGruppenName);

        /// <summary>
        /// Reading group Async
        /// </summary>
        /// <param name="i_strGruppenName"></param>
        /// <returns></returns>
		Task<IEnumerable<EDC_SpsListenElement>> FUN_fdcGruppeLesenAsync(string i_strGruppenName);

        /// <summary>
        /// Activate group Async
        /// </summary>
        /// <param name="i_strGruppenName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeAktivierenAsync(string i_strGruppenName);

        /// <summary>
        /// Disable group Async
        /// </summary>
        /// <param name="i_strGruppenName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeDeaktivierenAsync(string i_strGruppenName);
	}
}
