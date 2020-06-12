using Ersa.Platform.Common.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.Interfaces
{
	public interface INF_KommunikationsDienst : IDisposable
	{
        /// <summary>
        /// 建立连接
        /// Establish connection to control
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <returns></returns>
		Task FUN_fdcVerbindungZurSteuerungAufbauen(bool i_blnOnline);

        /// <summary>
        /// Disconnect
        /// </summary>
		void SUB_VerbindungLoesen();

		void SUB_WertLesen(EDC_PrimitivParameter i_edcParameter);

		Task FUN_fdcWerteLesenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

		void SUB_WertSchreiben(EDC_PrimitivParameter i_edcParameter);

		Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGruppenName, int i_i32CycleTime = 100);

		Task FUN_fdcGruppeLesenAsync(string i_strGruppenName);

		Task FUN_fdcGruppeAktivierenAsync(string i_strGruppenName);

		Task FUN_fdcGruppeDeaktivierenAsync(string i_strGruppenName);

		Task FUN_fdcWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

		Task FUN_fdcGruppenWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGruppenName);

		Task FUN_fdcPhysischeAdressenRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

		Task FUN_fdcPhysischeAdressenDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

		Task FUN_fdcSPSEventHandlerRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

		Task FUN_fdcSPSEventHandlerDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

		Task FUN_fdcParameterAbbildAdressenRegistrierenAsync(IEnumerable<string> i_enuAdressen);

		void SUB_ParameterAbbildSchreiben(IEnumerable<KeyValuePair<string, object>> i_dicAbbild);
	}
}
