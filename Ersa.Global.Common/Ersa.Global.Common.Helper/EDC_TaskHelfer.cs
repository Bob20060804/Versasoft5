using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Global.Common.Helper
{
	[Obsolete("Dieser Helfer hat in der Vergangenheit mehr Ärger gemacht als geholfen und wird daher wieder abgeschafft")]
	public static class EDC_TaskHelfer
	{
		private class EDC_ExclusiveSynchronizationContext : SynchronizationContext
		{
			private readonly AutoResetEvent m_fdcWorkItemsWaiting = new AutoResetEvent(initialState: false);

			private readonly Queue<Tuple<SendOrPostCallback, object>> m_lstItems = new Queue<Tuple<SendOrPostCallback, object>>();

			private bool m_blnFertig;

			public Exception PRO_fdcInnerException
			{
				get;
				set;
			}

			public override void Send(SendOrPostCallback i_fdcItem, object i_fdcState)
			{
				throw new NotSupportedException("We cannot send to our same thread");
			}

			public override void Post(SendOrPostCallback i_fdcItem, object i_fdcState)
			{
				lock (m_lstItems)
				{
					m_lstItems.Enqueue(Tuple.Create(i_fdcItem, i_fdcState));
				}
				m_fdcWorkItemsWaiting.Set();
			}

			public void SUB_EndMessageLoop()
			{
				Post(delegate
				{
					m_blnFertig = true;
				}, null);
			}

			public void SUB_BeginMessageLoop()
			{
				while (true)
				{
					if (m_blnFertig)
					{
						return;
					}
					Tuple<SendOrPostCallback, object> tuple = null;
					lock (m_lstItems)
					{
						if (m_lstItems.Count > 0)
						{
							tuple = m_lstItems.Dequeue();
						}
					}
					if (tuple != null)
					{
						tuple.Item1(tuple.Item2);
						if (PRO_fdcInnerException != null)
						{
							break;
						}
					}
					else
					{
						m_fdcWorkItemsWaiting.WaitOne();
					}
				}
				throw new AggregateException("AsyncHelpers.Run method threw an exception.", PRO_fdcInnerException);
			}

			public override SynchronizationContext CreateCopy()
			{
				return this;
			}
		}

		public static T FUN_objRunSync<T>(Func<Task<T>> i_delFunc)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			EDC_ExclusiveSynchronizationContext fdcNeuerContext = new EDC_ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(fdcNeuerContext);
			T objErgebnis = (T)default(T);
			fdcNeuerContext.Post(async delegate
			{
				try
				{
					T val2 = objErgebnis;
					T val = (T)(objErgebnis = (T)(await i_delFunc()));
				}
				catch (Exception pRO_fdcInnerException)
				{
					fdcNeuerContext.PRO_fdcInnerException = pRO_fdcInnerException;
					throw;
				}
				finally
				{
					fdcNeuerContext.SUB_EndMessageLoop();
				}
			}, null);
			fdcNeuerContext.SUB_BeginMessageLoop();
			SynchronizationContext.SetSynchronizationContext(current);
			return (T)objErgebnis;
		}

		public static void SUB_RunSync(Func<Task> i_delFunc)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			EDC_ExclusiveSynchronizationContext fdcNeuerContext = new EDC_ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(fdcNeuerContext);
			fdcNeuerContext.Post(async delegate
			{
				try
				{
					await i_delFunc();
				}
				catch (Exception pRO_fdcInnerException)
				{
					fdcNeuerContext.PRO_fdcInnerException = pRO_fdcInnerException;
					throw;
				}
				finally
				{
					fdcNeuerContext.SUB_EndMessageLoop();
				}
			}, null);
			fdcNeuerContext.SUB_BeginMessageLoop();
			SynchronizationContext.SetSynchronizationContext(current);
		}
	}
}
