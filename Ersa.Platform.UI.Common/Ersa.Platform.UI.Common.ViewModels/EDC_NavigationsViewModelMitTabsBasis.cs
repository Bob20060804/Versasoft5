using Ersa.Global.Common;
using Ersa.Global.Controls.NavigationsTabControl;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.UI.Common.TabItem;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.ViewModels
{
	public abstract class EDC_NavigationsViewModelMitTabsBasis : EDC_NavigationsViewModelBasis, IPartImportsSatisfiedNotification
	{
		private readonly List<EDC_TabItem> m_lstAlleTabItems = new List<EDC_TabItem>();

		private readonly List<EDC_TabItem> m_lstSichtbareTabItems = new List<EDC_TabItem>();

		private readonly IDictionary<ENUM_SoftwareFeatures, bool> m_dicSoftwareFeatures;

		private EDC_NavigationsViewModelBasis m_edcAktivesViewModel;

		private IList<EDC_TabItemSpezifikation> m_lstTabSpezifikationen;

		public virtual IEnumerable<IEnumerable<EDC_TabItemSpezifikation>> PRO_enuTabItemSpezifikationen
		{
			get;
			set;
		}

		public EDC_SmartObservableCollection<EDC_TabItem> PRO_lstTabItems
		{
			get;
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				if (m_edcAktivesViewModel != null)
				{
					return m_edcAktivesViewModel.PRO_blnHatAenderung;
				}
				return false;
			}
		}

		protected EDC_NavigationsViewModelMitTabsBasis()
		{
			PRO_lstTabItems = new EDC_SmartObservableCollection<EDC_TabItem>();
			m_lstTabSpezifikationen = new List<EDC_TabItemSpezifikation>();
			m_dicSoftwareFeatures = Enum.GetValues(typeof(ENUM_SoftwareFeatures)).OfType<ENUM_SoftwareFeatures>().ToDictionary((ENUM_SoftwareFeatures i_enmFeature) => i_enmFeature, (ENUM_SoftwareFeatures i_enmFeature) => false);
		}

		public void SUB_RegistriereTabs(IEnumerable<EDC_TabItemSpezifikation> i_enuTabSpezifikationen)
		{
			m_lstTabSpezifikationen = (from i_edcTabSpezifikation in m_lstTabSpezifikationen.Union(i_enuTabSpezifikationen)
			orderby i_edcTabSpezifikation.PRO_i32Reihenfolge
			select i_edcTabSpezifikation).ToList();
			foreach (EDC_TabItemSpezifikation edcSpezifikation in m_lstTabSpezifikationen)
			{
				if (m_lstAlleTabItems.FindIndex(delegate(EDC_TabItem i_edcTab)
				{
					if (i_edcTab.PRO_strNameKey == edcSpezifikation.PRO_strNameKey && i_edcTab.PRO_i32Reihenfolge == edcSpezifikation.PRO_i32Reihenfolge)
					{
						return i_edcTab.PRO_strRecht == edcSpezifikation.PRO_strRecht;
					}
					return false;
				}) == -1)
				{
					m_lstAlleTabItems.Add(new EDC_TabItem(edcSpezifikation));
				}
			}
			SUB_ErstelleTabsNeu((from i_edcTab in m_lstAlleTabItems.AsEnumerable()
			orderby i_edcTab.PRO_i32Reihenfolge
			select i_edcTab).ToList());
			SUB_SoftwareFeaturesAuswerten();
		}

		public void SUB_ErstelleTabsNeu(List<EDC_TabItem> i_lstSichtbareTabItems)
		{
			m_lstSichtbareTabItems.Clear();
			m_lstSichtbareTabItems.AddRange(i_lstSichtbareTabItems);
			PRO_lstTabItems.SUB_Reset(i_lstSichtbareTabItems);
			if (PRO_lstTabItems.Count > 0)
			{
				m_edcAktivesViewModel = PRO_lstTabItems[0].PRO_edcNavigationsViewModel;
			}
		}

		public virtual void OnImportsSatisfied()
		{
			base.PRO_fdcEventAggregator.GetEvent<EDC_SoftwareFeatureGeaendertEvent>().Subscribe(SUB_SoftwareFeaturesGeaendert);
			if (PRO_enuTabItemSpezifikationen != null && PRO_enuTabItemSpezifikationen.Any())
			{
				SUB_RegistriereTabs(PRO_enuTabItemSpezifikationen.SelectMany((IEnumerable<EDC_TabItemSpezifikation> i_edcSpezifikation) => (i_edcSpezifikation as IList<EDC_TabItemSpezifikation>) ?? i_edcSpezifikation.ToList()));
			}
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			if (m_edcAktivesViewModel != null)
			{
				await m_edcAktivesViewModel.FUN_fdcAenderungenSpeichernAsync();
			}
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override async Task FUN_fdcAenderungenVerwerfenAsync()
		{
			if (m_edcAktivesViewModel != null)
			{
				await m_edcAktivesViewModel.FUN_fdcAenderungenVerwerfenAsync();
			}
			await base.FUN_fdcAenderungenVerwerfenAsync();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			await FUN_fdcZumAktivenTabNavigierenAsync(i_fdcNavigationContext);
		}

		public override async Task FUN_fdcOnNavigatedFromAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedFromAsync(i_fdcNavigationContext);
			await FUN_fdcVomAktivenTabNavigierenAsync(i_fdcNavigationContext);
		}

		protected async Task FUN_fdcTabGeandertAsync(EDC_NavigationsElementAenderungsDaten i_edcAenderungsDaten)
		{
			EDC_TabItem eDC_TabItem = i_edcAenderungsDaten.PRO_objElement as EDC_TabItem;
			bool flag2 = i_edcAenderungsDaten.PRO_blnIstAenderungAbgebrochen = FUN_blnSollNavigationAbbrechen(eDC_TabItem);
			if (!i_edcAenderungsDaten.PRO_blnIstAenderungAbgebrochen && eDC_TabItem != null)
			{
				await FUN_fdcTabNavigationBehandelnAsync(eDC_TabItem);
			}
		}

		protected abstract void SUB_TabRechteUeberpruefen(EDC_TabItem i_edcTab);

		private async Task FUN_fdcTabNavigationBehandelnAsync(EDC_TabItem i_edcNeuerTab)
		{
			if (i_edcNeuerTab != null && m_edcAktivesViewModel != i_edcNeuerTab.PRO_edcNavigationsViewModel)
			{
				await FUN_fdcVomAktivenTabNavigierenAsync(null);
				SUB_TabRechteUeberpruefen(i_edcNeuerTab);
				m_edcAktivesViewModel = i_edcNeuerTab.PRO_edcNavigationsViewModel;
				await FUN_fdcZumAktivenTabNavigierenAsync(null);
				foreach (EDC_TabItem pRO_lstTabItem in PRO_lstTabItems)
				{
					pRO_lstTabItem.PRO_blnIstAktiv = (pRO_lstTabItem == i_edcNeuerTab);
				}
			}
		}

		private bool FUN_blnSollNavigationAbbrechen(EDC_TabItem i_edcNeuerTab)
		{
			bool blnWurdeNavigationAbgebrochen = false;
			EDC_NavigationsViewModelBasis eDC_NavigationsViewModelBasis = i_edcNeuerTab?.PRO_edcNavigationsViewModel;
			if (m_edcAktivesViewModel != null && m_edcAktivesViewModel != eDC_NavigationsViewModelBasis)
			{
				m_edcAktivesViewModel.ConfirmNavigationRequest(null, delegate(bool i_blnNavigationErlaubt)
				{
					blnWurdeNavigationAbgebrochen = !i_blnNavigationErlaubt;
				});
			}
			return blnWurdeNavigationAbgebrochen;
		}

		private async Task FUN_fdcVomAktivenTabNavigierenAsync(NavigationContext i_fdcNavigationContext)
		{
			if (m_edcAktivesViewModel != null && m_edcAktivesViewModel.PRO_blnIsAktiv)
			{
				m_edcAktivesViewModel.SUB_SetzeIstAktiv(i_blnIstAktiv: false);
				await m_edcAktivesViewModel.FUN_fdcOnNavigatedFromAsync(i_fdcNavigationContext);
			}
		}

		private async Task FUN_fdcZumAktivenTabNavigierenAsync(NavigationContext i_fdcNavigationContext)
		{
			if (m_edcAktivesViewModel != null && !m_edcAktivesViewModel.PRO_blnIsAktiv)
			{
				m_edcAktivesViewModel.SUB_SetzeIstAktiv(i_blnIstAktiv: true);
				using (FUN_fdcFortschrittsAnzeigeEinblenden())
				{
					await m_edcAktivesViewModel.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
				}
			}
		}

		private void SUB_SoftwareFeaturesGeaendert(List<EDC_SoftwareFeatureGeaendertPayload> i_lstPayloads)
		{
			foreach (EDC_SoftwareFeatureGeaendertPayload i_lstPayload in i_lstPayloads)
			{
				if (m_dicSoftwareFeatures.ContainsKey(i_lstPayload.PRO_enmSoftwareFeature))
				{
					m_dicSoftwareFeatures[i_lstPayload.PRO_enmSoftwareFeature] = i_lstPayload.PRO_blnSoftwareFeatureVorhanden;
				}
			}
			SUB_SoftwareFeaturesAuswerten();
		}

		private void SUB_SoftwareFeaturesAuswerten()
		{
			List<EDC_TabItem> list = new List<EDC_TabItem>();
			list.AddRange(m_lstSichtbareTabItems);
			IEnumerable<ENUM_SoftwareFeatures> enumerable = from i_fdcKvp in m_dicSoftwareFeatures
			where !i_fdcKvp.Value
			select i_fdcKvp.Key;
			IEnumerable<ENUM_SoftwareFeatures> enumerable2 = from i_fdcKvp in m_dicSoftwareFeatures
			where i_fdcKvp.Value
			select i_fdcKvp.Key;
			foreach (ENUM_SoftwareFeatures enmFeature in enumerable)
			{
				IEnumerable<EDC_TabItem> second = from i_edcTabItem in m_lstAlleTabItems
				where i_edcTabItem.FUN_blnPruefeSoftwareFeature(enmFeature)
				select i_edcTabItem;
				list = list.Except(second).ToList();
			}
			foreach (ENUM_SoftwareFeatures enmFeature2 in enumerable2)
			{
				IEnumerable<EDC_TabItem> second2 = from i_edcTabItem in m_lstAlleTabItems
				where i_edcTabItem.FUN_blnPruefeSoftwareFeature(enmFeature2)
				select i_edcTabItem;
				list = list.Union(second2).ToList();
			}
			if (!list.SequenceEqual(m_lstSichtbareTabItems))
			{
				SUB_ErstelleTabsNeu((from i_edcTab in list.AsEnumerable()
				orderby i_edcTab.PRO_i32Reihenfolge
				select i_edcTab).ToList());
			}
		}
	}
}
