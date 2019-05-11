using System;
using System.Threading.Tasks;

namespace Ersa.Global.Mvvm.Commands
{
	public sealed class NotifyTaskCompletion : BindableBase
	{
		public Task Task
		{
			get;
		}

		public Task TaskCompletion
		{
			get;
		}

		public TaskStatus Status => Task.Status;

		public bool IsCompleted => Task.IsCompleted;

		public bool IsNotCompleted => !Task.IsCompleted;

		public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

		public bool IsCanceled => Task.IsCanceled;

		public bool IsFaulted => Task.IsFaulted;

		public AggregateException Exception => Task.Exception;

		public Exception InnerException => Exception?.InnerException;

		public string ErrorMessage => InnerException?.Message;

		public NotifyTaskCompletion(Task task)
		{
			Task = task;
			TaskCompletion = WatchTaskAsync(task);
		}

		private async Task WatchTaskAsync(Task task)
		{
			try
			{
				await task;
			}
			catch
			{
			}
			RaisePropertyChanged("Status");
			RaisePropertyChanged("IsCompleted");
			RaisePropertyChanged("IsNotCompleted");
			if (task.IsCanceled)
			{
				RaisePropertyChanged("IsCanceled");
			}
			else if (task.IsFaulted)
			{
				RaisePropertyChanged("IsFaulted");
				RaisePropertyChanged("Exception");
				RaisePropertyChanged("InnerException");
				RaisePropertyChanged("ErrorMessage");
			}
			else
			{
				RaisePropertyChanged("IsSuccessfullyCompleted");
			}
		}
	}
}
