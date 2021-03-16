using System;
using System.Threading.Tasks;
using System.Windows;

namespace Ersa.Platform.Infrastructure.Extensions
{
    /// <summary>
    /// Task Extend
    /// </summary>
	public static class EDC_TaskExtensions
	{
		public static TaskScheduler PRO_fdcTaskScheduler
		{
			get
			{
				if (Application.Current == null)
				{
					return new EDC_SynchronerTaskScheduler();
				}
				return TaskScheduler.Default;
			}
		}

		public static TaskScheduler PRO_fdcUITaskScheduler
		{
			get
			{
				if (Application.Current == null)
				{
					return new EDC_SynchronerTaskScheduler();
				}
				return TaskScheduler.FromCurrentSynchronizationContext();
			}
		}

		/// <summary>
		/// 是否在Timeout内完成操作
		/// 如果传入的Task先结束, 继续之前的线程
		/// DelayTask先结束 执行传入的Action
		/// </summary>
		/// <param name="i_fdcTask"></param>
		/// <param name="i_i32Timeout"></param>
		/// <param name="i_delOnTimeoutElapsed"></param>
		/// <returns></returns>
		public static async Task FUN_fdcMitTimeout(this Task i_fdcTask, int i_i32Timeout, Action i_delOnTimeoutElapsed)
		{
			Task fdcDelayTask = Task.Delay(i_i32Timeout);
			if (await Task.WhenAny(i_fdcTask, fdcDelayTask).ConfigureAwait(true) == fdcDelayTask)
			{
				i_delOnTimeoutElapsed();
			}
			await i_fdcTask.ConfigureAwait(true);
		}

		/// <summary>
		/// 在Timeout之后继续运行
		/// 如果DelayTask先完成,继续等待传入的线程
		/// 如果传入的Task先完成, 抛出异常
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="i_fdcTask"></param>
		/// <param name="i_i32TimeoutMilliseconds"></param>
		/// <returns></returns>
		public static async Task<T> FUN_fdcTimeoutAfterAsync<T>(this Task<T> i_fdcTask, int i_i32TimeoutMilliseconds)
		{
			if (i_fdcTask == await Task.WhenAny(i_fdcTask, Task.Delay(i_i32TimeoutMilliseconds)).ConfigureAwait(true))
			{
				return await i_fdcTask.ConfigureAwait(true);
			}
			throw new TimeoutException();
		}
	}
}
