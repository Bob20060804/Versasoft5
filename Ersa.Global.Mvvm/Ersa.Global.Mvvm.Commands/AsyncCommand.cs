using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Ersa.Global.Mvvm.Commands
{
	public class AsyncCommand : AsyncCommand<object>
	{
		public AsyncCommand(Func<Task> command, Func<bool> canExecuteMethod = null)
			: base((Func<object, Task>)((object obj) => command()), (Func<object, bool>)((object obj) => canExecuteMethod?.Invoke() ?? true))
		{
		}

		public Task ExecuteAsync()
		{
			return ExecuteAsync(null);
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}
	}
	public class AsyncCommand<T> : AsyncCommandBase, INotifyPropertyChanged
	{
		private readonly Func<T, Task> _command;

		private readonly Func<T, bool> _canExecuteMethod;

		private NotifyTaskCompletion _execution;

		public NotifyTaskCompletion Execution
		{
			get
			{
				return _execution;
			}
			private set
			{
				_execution = value;
				OnPropertyChanged("Execution");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public AsyncCommand(Func<T, Task> command, Func<T, bool> canExecuteMethod = null)
		{
			_command = command;
			_canExecuteMethod = (canExecuteMethod ?? ((Func<T, bool>)((T obj) => true)));
		}

		public override bool CanExecute(object parameter)
		{
			if (_canExecuteMethod((T)parameter))
			{
				if (Execution != null)
				{
					return Execution.IsCompleted;
				}
				return true;
			}
			return false;
		}

		public override async Task ExecuteAsync(object parameter)
		{
			Task task = _command((T)parameter);
			Execution = new NotifyTaskCompletion(task);
			RaiseCanExecuteChanged();
			await Execution.TaskCompletion;
			RaiseCanExecuteChanged();
			if (Execution.IsFaulted)
			{
				throw Execution.Exception;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
