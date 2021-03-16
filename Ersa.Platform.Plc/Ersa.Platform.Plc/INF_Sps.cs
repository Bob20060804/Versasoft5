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
		Task Fun_ConnectAsync(bool i_blnOnline, string i_strAdresse);

        /// <summary>
        /// 断开连接
        /// DisConnect
        /// </summary>
		void Sub_DisConnect();

        /// <summary>
        /// Read Value
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		string Fun_strReadValue(string i_strVarName);
		float Fun_sngReadValue(string i_strVarName);
		uint Fun_u32ReadValue(string i_strVarName);
		int Fun_i32ReadValue(string i_strVarName);
		short Fun_i16ReadValue(string i_strVarName);
		ushort Fun_u16ReadValue(string i_strVarName);
		byte Fun_bytReadValue(string i_strVarName);
		bool Fun_blnReadValue(string i_strVarName);

        /// <summary>
        /// Write value
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <param name="i_strWert"></param>
		void Sub_WriteValue(string i_strVarName, string i_strWert);

        /// <summary>
        /// 事件 注册
        /// EventHandler Register
        /// </summary>
        /// <param name="i_strVarName">变量名</param>
        /// <param name="i_delHandler">事件</param>
        /// <returns></returns>
		IDisposable Fun_fdcRegisterEventHandler(string i_strVarName, Action i_delHandler);

        /// <summary>
        /// 变量 注册
        /// Variables Register async
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

        /// <summary>
        /// 变量 注销
        /// Variables Unregister async
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

        /// <summary>
        /// 组 事件注册
        /// Activate event group
        /// </summary>
		void Sub_GroupEventActive();

        /// <summary>
        /// 组 事件注销
        /// Deactivate event group
        /// </summary>
		void Sub_GroupEventDisactivate();

        /// <summary>
        /// 创建Group变量 
        /// Create Group Variable Async
        /// </summary>
        /// <param name="i_enmVariablen"></param>
        /// <param name="i_strGroupName"></param>
        /// <param name="i_i32CycleTime"></param>
        /// <returns></returns>
		Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enmVariablen, string i_strGroupName, int i_i32CycleTime = 100);

        /// <summary>
        /// Group writing Async
        /// </summary>
        /// <param name="i_enmParameter"></param>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGroupName);

        /// <summary>
        /// 整个Group读取
        /// Reading group Async
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task<IEnumerable<EDC_SpsListenElement>> Fun_fdcGroupReadAsync(string i_strGroupName);

        /// <summary>
        /// Activate group Async
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task Fun_fdcGroupActiveAsync(string i_strGroupName);

        /// <summary>
        /// Disable group Async
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGroupDisableAsync(string i_strGroupName);
	}
}
