using Ersa.Platform.Common.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.Interfaces
{
	public interface Inf_CommunicationService : IDisposable
	{
        /// <summary>
        /// ��������
        /// Establish connection to control
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <returns></returns>
		Task Fun_fdcConnect(bool i_blnOnline);

        /// <summary>
        /// Disconnect
        /// </summary>
		void SUB_VerbindungLoesen();

        /// <summary>
        /// Read Value
        /// </summary>
        /// <param name="i_edcParameter"></param>
		void Sub_ReadValue(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// Read Value Async
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		Task Fun_fdcReadValueAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

        /// <summary>
        /// Write value
        /// </summary>
        /// <param name="i_edcParameter"></param>
		void SUB_WertSchreiben(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// Variable group create async
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_strGroupName"></param>
        /// <param name="i_i32CycleTime"></param>
        /// <returns></returns>
		Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName, int i_i32CycleTime = 100);

        /// <summary>
        /// ��ȡGroup �첽
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeLesenAsync(string i_strGroupName);

        /// <summary>
        /// ����Group �첽
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeAktivierenAsync(string i_strGroupName);

        /// <summary>
        /// ������ �첽
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGruppeDeaktivierenAsync(string i_strGroupName);

        /// <summary>
        /// Write Value
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		Task FUN_fdcWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

        /// <summary>
        /// Group Write Value Async
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		Task FUN_fdcGroupValueWriteAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName);

        /// <summary>
        /// �����ַ�洢 �첽
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		Task FUN_fdcPhysischeAdressenRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken);

        /// <summary>
        /// �����ַע�� �첽
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task FUN_fdcPhysischeAdressenDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

        /// <summary>
        ///  BR plc ע���¼�
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task FUN_fdcSPSEventHandlerRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

        /// <summary>
        /// BR PLC ע���¼�
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		Task FUN_fdcSPSEventHandlerDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken);

        /// <summary>
        /// ע�����ͼ���ַ
        /// </summary>
        /// <param name="i_enuAdressen"></param>
        /// <returns></returns>
		Task FUN_fdcParameterAbbildAdressenRegistrierenAsync(IEnumerable<string> i_enuAdressen);

        /// <summary>
        /// д����ͼ
        /// </summary>
        /// <param name="i_dicAbbild"></param>
		void SUB_ParameterAbbildSchreiben(IEnumerable<KeyValuePair<string, object>> i_dicAbbild);
	}
}
