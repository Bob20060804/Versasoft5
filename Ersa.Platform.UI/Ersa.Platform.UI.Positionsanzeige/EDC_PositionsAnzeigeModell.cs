using Ersa.Global.Mvvm;
using System.Collections.ObjectModel;

namespace Ersa.Platform.UI.Positionsanzeige
{
	public class EDC_PositionsAnzeigeModell : BindableBase
	{
		public ObservableCollection<EDC_Spalte> PRO_lstSpalten
		{
			get;
			private set;
		}

		public EDC_PositionsAnzeigeModell()
		{
			PRO_lstSpalten = new ObservableCollection<EDC_Spalte>();
		}
	}
}
