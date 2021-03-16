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
		/// �Ƿ���Timeout����ɲ���
		/// ��������Task�Ƚ���, ����֮ǰ���߳�
		/// DelayTask�Ƚ��� ִ�д����Action
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
		/// ��Timeout֮���������
		/// ���DelayTask�����,�����ȴ�������߳�
		/// ��������Task�����, �׳��쳣
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
