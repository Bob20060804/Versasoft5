using Ersa.Global.Mvvm;
using System.Collections.ObjectModel;

namespace Ersa.Platform.UI.Positionsanzeige
{
	public class EDC_Spalte : BindableBase
	{
		public string PRO_strIconUri
		{
			get;
			set;
		}

		public ObservableCollection<EDC_Zelle> PRO_lstZellen
		{
			get;
			private set;
		}

		public EDC_Spalte()
		{
			PRO_lstZellen = new ObservableCollection<EDC_Zelle>();
		}
	}
}
