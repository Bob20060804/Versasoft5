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
	[Export(typeof(INF_KommunikationsDienst))]
	[Export(typeof(INF_KommunikationsDienstWrapper))]
	public class EDC_KommunikationsDienstWrapper : EDC_DisposableObject, INF_KommunikationsDienst, IDisposable, INF_KommunikationsDienstWrapper
	{
		private INF_KommunikationsDienst m_edcInstanz;

		[ImportingConstructor]
		public EDC_KommunikationsDienstWrapper(EDC_KommunikationsDienst i_edcKommunikationsDienst)
		{
			m_edcInstanz = i_edcKommunikationsDienst;
		}

		public Task FUN_fdcVerbindungZurSteuerungAufbauen(bool i_blnOnline)
		{
			return m_edcInstanz.FUN_fdcVerbindungZurSteuerungAufbauen(i_blnOnline);
		}

		public void SUB_VerbindungLoesen()
		{
			m_edcInstanz.SUB_VerbindungLoesen();
		}

		public void SUB_WertLesen(EDC_PrimitivParameter i_edcParameter)
		{
			m_edcInstanz.SUB_WertLesen(i_edcParameter);
		}

		public Task FUN_fdcWerteLesenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return m_edcInstanz.FUN_fdcWerteLesenAsync(i_lstPrimitivParameter, i_fdcCancellationToken);
		}

		public Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGruppenName, int i_i32CycleTime = 100)
		{
			return m_edcInstanz.FUN_fdcVariablenGruppeErstellenAsync(i_lstPrimitivParameter, i_strGruppenName, i_i32CycleTime);
		}

		public Task FUN_fdcGruppeLesenAsync(string i_strGruppenName)
		{
			return m_edcInstanz.FUN_fdcGruppeLesenAsync(i_strGruppenName);
		}

		public Task FUN_fdcGruppeAktivierenAsync(string i_strGruppenName)
		{
			return m_edcInstanz.FUN_fdcGruppeAktivierenAsync(i_strGruppenName);
		}

		public Task FUN_fdcGruppeDeaktivierenAsync(string i_strGruppenName)
		{
			return m_edcInstanz.FUN_fdcGruppeDeaktivierenAsync(i_strGruppenName);
		}

		public void SUB_WertSchreiben(EDC_PrimitivParameter i_edcParameter)
		{
			m_edcInstanz.SUB_WertSchreiben(i_edcParameter);
		}

		public Task FUN_fdcWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return m_edcInstanz.FUN_fdcWerteSchreibenAsync(i_lstPrimitivParameter, i_fdcCancellationToken);
		}

		public Task FUN_fdcGruppenWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGruppenName)
		{
			return m_edcInstanz.FUN_fdcGruppenWerteSchreibenAsync(i_lstPrimitivParameter, i_strGruppenName);
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

		public void SUB_KommunikationsDienstInstanzSetzen(INF_KommunikationsDienst i_edcKommunikationsDienst)
		{
			m_edcInstanz = i_edcKommunikationsDienst;
		}

		protected override void SUB_InternalDispose()
		{
			m_edcInstanz.Dispose();
		}
	}
}
