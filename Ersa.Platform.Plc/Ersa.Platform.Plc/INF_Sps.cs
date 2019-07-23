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

		void SUB_VerbindungLoesen();

		string FUN_strWertLesen(string i_strVarName);

		float FUN_sngWertLesen(string i_strVarName);

		uint FUN_u32WertLesen(string i_strVarName);

		int FUN_i32WertLesen(string i_strVarName);

		short FUN_i16WertLesen(string i_strVarName);

		ushort FUN_u16WertLesen(string i_strVarName);

		byte FUN_bytWertLesen(string i_strVarName);

		bool FUN_blnWertLesen(string i_strVarName);

		void SUB_WertSchreiben(string i_strVarName, string i_strWert);

		IDisposable FUN_fdcEventHandlerRegistrieren(string i_strVarName, Action i_delHandler);

		Task FUN_fdcVariablenAnmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

		Task FUN_fdcVariablenAbmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken);

		void SUB_EventGruppeAktivieren();

		void SUB_EventGruppeDeaktivieren();

		Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<string> i_enmVariablen, string i_strGruppenName, int i_i32CycleTime = 100);

		Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGruppenName);

		Task<IEnumerable<EDC_SpsListenElement>> FUN_fdcGruppeLesenAsync(string i_strGruppenName);

		Task FUN_fdcGruppeAktivierenAsync(string i_strGruppenName);

		Task FUN_fdcGruppeDeaktivierenAsync(string i_strGruppenName);
	}
}
