using Ersa.Global.Common;
using Ersa.Global.Common.Extensions;
using Ersa.Global.Mvvm;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Prism;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Infrastructure
{
	public abstract class EDC_NotificationObjectMitSprachUmschaltung : BindableBase
	{
		private const int mC_i32AnzahlBuckets = 100;

		private static readonly List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>[] msa_lstNotificationObjekte = new List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>[100];

		private static readonly object[] msa_objLocks = new object[100];

		private static readonly object ms_objInitLock = new object();

		private static IEventAggregator ms_fdcEventAggregator;

		private static int ms_i32BucketCounter;

		private static bool ms_blnInitialized;

		private readonly int m_i32Bucket;

		private static bool PRO_blnIstSprachumschaltungAktiv => ms_fdcEventAggregator != null;

		protected EDC_NotificationObjectMitSprachUmschaltung()
		{
			if (!ms_blnInitialized)
			{
				FUN_fdcEventAggregatorInitialisieren();
			}
			if (PRO_blnIstSprachumschaltungAktiv)
			{
				m_i32Bucket = ms_i32BucketCounter;
				ms_i32BucketCounter = (ms_i32BucketCounter + 1) % 100;
				List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>> list = msa_lstNotificationObjekte[m_i32Bucket];
				lock (msa_objLocks[m_i32Bucket])
				{
					list.Add(new WeakReference<EDC_NotificationObjectMitSprachUmschaltung>(this));
				}
			}
		}

		~EDC_NotificationObjectMitSprachUmschaltung()
		{
			if (PRO_blnIstSprachumschaltungAktiv)
			{
				List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>> list = msa_lstNotificationObjekte[m_i32Bucket];
				List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>> list2 = new List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>();
				lock (msa_objLocks[m_i32Bucket])
				{
					foreach (WeakReference<EDC_NotificationObjectMitSprachUmschaltung> item in list)
					{
						if (!item.TryGetTarget(out EDC_NotificationObjectMitSprachUmschaltung _))
						{
							list2.Add(item);
						}
					}
					list.RemoveRange(list2);
				}
			}
		}

		internal static IDisposable FUN_fdcEventAggregatorFuerTestSetzen(IEventAggregator i_fdcAggregator)
		{
			SubscriptionToken fdcToken = FUN_fdcEventAggregatorInitialisieren(i_fdcAggregator);
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				if (fdcToken != null)
				{
					ms_fdcEventAggregator.GetEvent<EDC_SpracheGeaendertEvent>().Unsubscribe(fdcToken);
				}
				ms_fdcEventAggregator = null;
				foreach (Tuple<List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>, object> item in msa_lstNotificationObjekte.Zip(msa_objLocks, (List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>> i_lstList, object i_objLock) => new Tuple<List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>, object>(i_lstList, i_objLock)))
				{
					lock (item.Item2)
					{
						item.Item1.Clear();
					}
				}
			});
		}

		private static SubscriptionToken FUN_fdcEventAggregatorInitialisieren(IEventAggregator i_fdcEventAggregator = null)
		{
			lock (ms_objInitLock)
			{
				if (ms_blnInitialized)
				{
					return null;
				}
				for (int i = 0; i < 100; i++)
				{
					msa_lstNotificationObjekte[i] = new List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>();
					msa_objLocks[i] = new object();
				}
				ms_fdcEventAggregator = (i_fdcEventAggregator ?? EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<IEventAggregator>());
				SubscriptionToken result = ms_fdcEventAggregator?.GetEvent<EDC_SpracheGeaendertEvent>().Subscribe(delegate
				{
					SUB_SpracheGeaendert();
				});
				ms_blnInitialized = true;
				return result;
			}
		}

		private static void SUB_SpracheGeaendert()
		{
			if (PRO_blnIstSprachumschaltungAktiv)
			{
				foreach (Tuple<List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>, object> item in msa_lstNotificationObjekte.Zip(msa_objLocks, (List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>> i_lstList, object i_objLock) => new Tuple<List<WeakReference<EDC_NotificationObjectMitSprachUmschaltung>>, object>(i_lstList, i_objLock)))
				{
					lock (item.Item2)
					{
						foreach (WeakReference<EDC_NotificationObjectMitSprachUmschaltung> item2 in item.Item1)
						{
							if (item2.TryGetTarget(out EDC_NotificationObjectMitSprachUmschaltung target))
							{
								target.RaisePropertyChanged(string.Empty);
							}
						}
					}
				}
			}
		}
	}
}
