using Ersa.Platform.UI.Common.ViewModels;
using Ersa.Platform.UI.Interfaces;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Ersa.Platform.UI.ViewModels
{
	public abstract class EDC_NavigationsViewModel : EDC_NavigationsViewModelBasis
	{
		private INF_ShellViewModel m_edcShellViewmodel;

		[Import(AllowDefault = true)]
		public INF_ShellViewModel PRO_edcShellViewModel
		{
			protected get
			{
				return m_edcShellViewmodel;
			}
			set
			{
				if (m_edcShellViewmodel != value)
				{
					m_edcShellViewmodel = value;
					PropertyChangedEventManager.AddHandler(m_edcShellViewmodel, delegate
					{
						SUB_BerechtigungenAuswerten();
					}, "PRO_blnVisuIstPrimaer");
				}
			}
		}

		protected bool FUN_blnIstPrimaer()
		{
			INF_ShellViewModel pRO_edcShellViewModel = PRO_edcShellViewModel;
			if (pRO_edcShellViewModel != null && pRO_edcShellViewModel.PRO_blnVisuIstPrimaer.HasValue)
			{
				return PRO_edcShellViewModel.PRO_blnVisuIstPrimaer.Value;
			}
			return false;
		}
	}
}
