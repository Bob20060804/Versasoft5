using Ersa.Global.Mvvm;
using Ersa.Platform.Mes.Konfiguration;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.ViewModels
{
	public class EDC_XamlListeFunktionsKonfiguration : BindableBase
	{
		private List<EDC_MesFunktionenKonfiguration> m_lstFunks;

		public List<EDC_MesFunktionenKonfiguration> PRO_lstFunks
		{
			get
			{
				return m_lstFunks;
			}
			set
			{
				SetProperty(ref m_lstFunks, value, "PRO_lstFunks");
			}
		}

		public EDC_XamlListeFunktionsKonfiguration(List<EDC_MesFunktionenKonfiguration> i_lstFunktions)
		{
			PRO_lstFunks = i_lstFunktions;
		}
	}
}
