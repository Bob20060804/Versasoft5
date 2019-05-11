using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ersa.Global.Common
{
	public abstract class EDC_ViewModelBasis : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void SUB_OnPropertyChanged([CallerMemberName] string i_strPropertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(i_strPropertyName));
		}
	}
}
