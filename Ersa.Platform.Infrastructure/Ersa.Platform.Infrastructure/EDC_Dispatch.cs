using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ersa.Platform.Infrastructure
{
	public static class EDC_Dispatch
	{
		private const DispatcherPriority mC_fdcDefaultPriority = DispatcherPriority.Input;

		public static Dispatcher PRO_fdcHauptDispatcher
		{
			get
			{
				if (Application.Current == null)
				{
					return Dispatcher.CurrentDispatcher;
				}
				return Application.Current.Dispatcher;
			}
		}

		public static void SUB_AktionStarten(Action i_delAktion, DispatcherPriority i_fdcPriority = DispatcherPriority.Input)
		{
			FUN_delAktionErstellen(i_delAktion, i_fdcPriority)();
		}

		public static Task FUN_fdcAwaitableAktion(Action i_delAktion)
		{
			return FUN_fdcAwaitableAktionErstellen(i_delAktion, DispatcherPriority.Input);
		}

		public static Task FUN_fdcAwaitableAktionErstellen(Action i_delAktion, DispatcherPriority i_fdcPriority)
		{
			TaskCompletionSource<bool> fdcTsc = new TaskCompletionSource<bool>();
			Action action = delegate
			{
				try
				{
					i_delAktion();
					fdcTsc.SetResult(result: true);
				}
				catch (Exception exception)
				{
					fdcTsc.SetException(exception);
				}
			};
			if (PRO_fdcHauptDispatcher.CheckAccess())
			{
				action();
			}
			else
			{
				PRO_fdcHauptDispatcher.BeginInvoke(i_fdcPriority, action);
			}
			return fdcTsc.Task;
		}

		public static Task FUN_fdcAwaitableAktion(Func<Task> i_delFunction)
		{
			return FUN_fdcAwaitableAktionErstellen(i_delFunction, DispatcherPriority.Input);
		}

		public static Task FUN_fdcAwaitableAktionErstellen(Func<Task> i_delFunction, DispatcherPriority i_fdcPriority)
		{
			TaskCompletionSource<bool> fdcTsc = new TaskCompletionSource<bool>();
			Action action = async delegate
			{
				try
				{
					await i_delFunction();
					fdcTsc.SetResult(result: true);
				}
				catch (Exception exception)
				{
					fdcTsc.SetException(exception);
				}
			};
			if (PRO_fdcHauptDispatcher.CheckAccess())
			{
				action();
			}
			else
			{
				PRO_fdcHauptDispatcher.BeginInvoke(i_fdcPriority, action);
			}
			return fdcTsc.Task;
		}

		public static EventHandler<T> FUN_delHandler<T>(EventHandler<T> i_delHandler) where T : EventArgs
		{
			return FUN_delHandlerErstellen(i_delHandler, DispatcherPriority.Input);
		}

		public static EventHandler<T> FUN_delHandlerErstellen<T>(EventHandler<T> i_delHandler, DispatcherPriority i_fdcPriority) where T : EventArgs
		{
			return delegate(object sender, T args)
			{
				if (PRO_fdcHauptDispatcher.CheckAccess())
				{
					i_delHandler(sender, args);
				}
				else
				{
					PRO_fdcHauptDispatcher.BeginInvoke(i_fdcPriority, i_delHandler, sender, args);
				}
			};
		}

		private static Action FUN_delAktionErstellen(Action i_delAktion, DispatcherPriority i_fdcPriority)
		{
			return delegate
			{
				if (FUN_blnUnitTestAktiv())
				{
					i_delAktion();
				}
				else
				{
					PRO_fdcHauptDispatcher.BeginInvoke(i_fdcPriority, i_delAktion);
				}
			};
		}

		private static bool FUN_blnUnitTestAktiv()
		{
			return Application.Current == null;
		}
	}
}
