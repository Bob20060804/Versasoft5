using Ersa.Global.Common;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
    /// <summary>
    /// 通讯服务包装
    /// Communication service wrapper
    /// </summary>
	[Export(typeof(Inf_CommunicationService))]
	[Export(typeof(INF_KommunikationsDienstWrapper))]
	public class EDC_KommunikationsDienstWrapper : EDC_DisposableObject, Inf_CommunicationService, IDisposable, INF_KommunikationsDienstWrapper
	{
		private Inf_CommunicationService m_edcInstanz;

		[ImportingConstructor]
		public EDC_KommunikationsDienstWrapper(EDC_KommunikationsDienst i_edcKommunikationsDienst)
		{
			m_edcInstanz = i_edcKommunikationsDienst;
		}

		public Task Fun_fdcConnect(bool i_blnOnline)
		{
			return m_edcInstanz.Fun_fdcConnect(i_blnOnline);
		}

		public void SUB_VerbindungLoesen()
		{
			m_edcInstanz.SUB_VerbindungLoesen();
		}

		public void Sub_ReadValue(EDC_PrimitivParameter i_edcParameter)
		{
			m_edcInstanz.Sub_ReadValue(i_edcParameter);
		}

		public Task Fun_fdcReadValueAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return m_edcInstanz.Fun_fdcReadValueAsync(i_lstPrimitivParameter, i_fdcCancellationToken);
		}

		public Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName, int i_i32CycleTime = 100)
		{
			return m_edcInstanz.FUN_fdcVariablenGruppeErstellenAsync(i_lstPrimitivParameter, i_strGroupName, i_i32CycleTime);
		}

		public Task FUN_fdcGruppeLesenAsync(string i_strGroupName)
		{
			return m_edcInstanz.FUN_fdcGruppeLesenAsync(i_strGroupName);
		}

		public Task FUN_fdcGruppeAktivierenAsync(string i_strGroupName)
		{
			return m_edcInstanz.FUN_fdcGruppeAktivierenAsync(i_strGroupName);
		}

		public Task FUN_fdcGruppeDeaktivierenAsync(string i_strGroupName)
		{
			return m_edcInstanz.FUN_fdcGruppeDeaktivierenAsync(i_strGroupName);
		}

		public void SUB_WertSchreiben(EDC_PrimitivParameter i_edcParameter)
		{
			m_edcInstanz.SUB_WertSchreiben(i_edcParameter);
		}

		public Task FUN_fdcWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return m_edcInstanz.FUN_fdcWerteSchreibenAsync(i_lstPrimitivParameter, i_fdcCancellationToken);
		}

		public Task FUN_fdcGroupValueWriteAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName)
		{
			return m_edcInstanz.FUN_fdcGroupValueWriteAsync(i_lstPrimitivParameter, i_strGroupName);
		}

		public Task FUN_fdcPhysischeAdressenRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return m_edcInstanz.FUN_fdcPhysischeAdressenRegistrierenAsync(i_lstPrimitivParameter, i_fdcCancellationToken);
		}

		public Task FUN_fdcPhysischeAdressenDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			return m_edcInstanz.FUN_fdcPhysischeAdressenDeRegistrierenAsync(i_lstPrimitivParameter, i_fdcToken);
		}

		public Task FUN_fdcSPSEventHandlerRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			return m_edcInstanz.FUN_fdcSPSEventHandlerRegistrierenAsync(i_lstPrimitivParameter, i_fdcToken);
		}

		public Task FUN_fdcSPSEventHandlerDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			return m_edcInstanz.FUN_fdcSPSEventHandlerDeRegistrierenAsync(i_lstPrimitivParameter, i_fdcToken);
		}

		public Task FUN_fdcParameterAbbildAdressenRegistrierenAsync(IEnumerable<string> i_enuAdressen)
		{
			return m_edcInstanz.FUN_fdcParameterAbbildAdressenRegistrierenAsync(i_enuAdressen);
		}

		public void SUB_ParameterAbbildSchreiben(IEnumerable<KeyValuePair<string, object>> i_dicAbbild)
		{
			m_edcInstanz.SUB_ParameterAbbildSchreiben(i_dicAbbild);
		}

		public void SUB_KommunikationsDienstInstanzSetzen(Inf_CommunicationService i_edcKommunikationsDienst)
		{
			m_edcInstanz = i_edcKommunikationsDienst;
		}

		protected override void SUB_InternalDispose()
		{
			m_edcInstanz.Dispose();
		}
	}
}
