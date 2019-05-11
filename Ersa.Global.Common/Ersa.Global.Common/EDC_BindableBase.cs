using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ersa.Global.Common
{
	[Serializable]
	[Obsolete("Ersa.Global.Mvvm.BindableBase verwenden")]
	public abstract class EDC_BindableBase : INotifyPropertyChanged
	{
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected void SUB_OnPropertyChangedFuerCaller([CallerMemberName] string i_strPropertyName = null)
		{
			SUB_OnPropertyChanged(i_strPropertyName);
		}

		protected void SUB_OnPropertyChangedFuerAlle()
		{
			SUB_OnPropertyChanged(string.Empty);
		}

		protected virtual void SUB_OnPropertyChanged(string i_strPropertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(i_strPropertyName));
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value))
			{
				return false;
			}
			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		[Obsolete("Eine der drei neuen Möglichkeiten verwenden: SUB_OnPropertyChanged, SUB_OnPropertyChangedFuerCaller, SUB_OnPropertyChangedFuerAlle")]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
				propertyChanged(this, e);
			}
		}
	}
}
