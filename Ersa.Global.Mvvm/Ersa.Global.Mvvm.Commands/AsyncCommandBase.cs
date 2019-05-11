using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ersa.Global.Mvvm.Commands
{
	public abstract class AsyncCommandBase : IAsyncCommand, ICommand
	{
		public event EventHandler CanExecuteChanged;

		public abstract bool CanExecute(object parameter);

		public abstract Task ExecuteAsync(object parameter);

		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
