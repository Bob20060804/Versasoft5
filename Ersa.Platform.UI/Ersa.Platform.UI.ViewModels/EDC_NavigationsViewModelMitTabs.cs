using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.UI.Common.TabItem;
using Ersa.Platform.UI.Common.ViewModels;
using Ersa.Platform.UI.Interfaces;
using System.ComponentModel.Composition;

namespace Ersa.Platform.UI.ViewModels
{
	public abstract class EDC_NavigationsViewModelMitTabs : EDC_NavigationsViewModelMitTabsBasis
	{
		[Import]
		public INF_AutorisierungsDienst PRO_edcAutorisierungsDienst
		{
			get;
			set;
		}

		[Import(AllowDefault = true)]
		public INF_InteractionViewModel PRO_edcInteractionViewModel
		{
			get;
			set;
		}

		public override void OnImportsSatisfied()
		{
			base.OnImportsSatisfied();
			if (PRO_edcAutorisierungsDienst != null)
			{
				SUB_BerechtigungenAuswerten();
			}
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			foreach (EDC_TabItem pRO_lstTabItem in base.PRO_lstTabItems)
			{
				pRO_lstTabItem.PRO_blnIstZugriffEingeschraenkt = (!string.IsNullOrEmpty(pRO_lstTabItem.PRO_strRecht) && !PRO_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert(pRO_lstTabItem.PRO_strRecht));
			}
		}

		protected override void SUB_TabRechteUeberpruefen(EDC_TabItem i_edcTab)
		{
			if (PRO_edcInteractionViewModel != null && i_edcTab.PRO_blnIstZugriffEingeschraenkt && PRO_edcAutorisierungsDienst.FUN_blnBenutzerUeberFehlendeRechteInformieren())
			{
				PRO_edcInteractionViewModel.SUB_LoginDialogAnzeigen(i_edcTab.PRO_strRechtNameKey);
			}
		}
	}
}
