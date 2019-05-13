using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.UI.Common.ViewModels;
using System;
using System.ComponentModel;

namespace Ersa.Platform.UI.Common.TabItem
{
	public class EDC_TabItem : EDC_NotificationObjectMitSprachUmschaltung, IEquatable<object>
	{
		private readonly EDC_TabItemSpezifikation m_edcTabItemSpezifikation;

		private bool m_blnIstZugriffEingeschraenkt;

		private bool m_blnIstAktiv;

		public string PRO_strNameKey => m_edcTabItemSpezifikation.PRO_strNameKey;

		public bool PRO_blnIstZugriffEingeschraenkt
		{
			get
			{
				return m_blnIstZugriffEingeschraenkt;
			}
			set
			{
				m_blnIstZugriffEingeschraenkt = value;
				RaisePropertyChanged("PRO_blnIstZugriffEingeschraenkt");
			}
		}

		public bool PRO_blnIstAktiv
		{
			get
			{
				return m_blnIstAktiv;
			}
			set
			{
				SetProperty(ref m_blnIstAktiv, value, "PRO_blnIstAktiv");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				if (PRO_edcNavigationsViewModel != null)
				{
					return PRO_edcNavigationsViewModel.PRO_blnIstFehlerhaft;
				}
				return false;
			}
		}

		public string PRO_strRecht => m_edcTabItemSpezifikation.PRO_strRecht;

		public string PRO_strRechtNameKey => m_edcTabItemSpezifikation.PRO_strRechtNameKey;

		public object PRO_objInhalt => m_edcTabItemSpezifikation.PRO_objTabView;

		public EDC_NavigationsViewModelBasis PRO_edcNavigationsViewModel => m_edcTabItemSpezifikation.PRO_edcNavigationsViewModel;

		public int PRO_i32Reihenfolge => m_edcTabItemSpezifikation.PRO_i32Reihenfolge;

		public EDC_TabItem(EDC_TabItemSpezifikation i_edcTabItemSpezifikation)
		{
			m_edcTabItemSpezifikation = i_edcTabItemSpezifikation;
			if (PRO_edcNavigationsViewModel != null)
			{
				PropertyChangedEventManager.AddHandler(PRO_edcNavigationsViewModel, delegate
				{
					RaisePropertyChanged("PRO_blnIstFehlerhaft");
				}, "PRO_blnIstFehlerhaft");
			}
		}

		public override bool Equals(object i_edcVergleichsItem)
		{
			EDC_TabItem eDC_TabItem = i_edcVergleichsItem as EDC_TabItem;
			if (eDC_TabItem == null)
			{
				return false;
			}
			if (eDC_TabItem.PRO_strNameKey == PRO_strNameKey)
			{
				return eDC_TabItem.PRO_i32Reihenfolge == PRO_i32Reihenfolge;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return PRO_strNameKey.GetHashCode();
		}

		public bool FUN_blnPruefeSoftwareFeature(ENUM_SoftwareFeatures i_enmFeature)
		{
			return m_edcTabItemSpezifikation.FUN_blnPruefeSoftwareFeature(i_enmFeature);
		}
	}
}
