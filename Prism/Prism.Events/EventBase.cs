using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prism.Events
{
	public abstract class EventBase
	{
		private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

		public SynchronizationContext SynchronizationContext
		{
			get;
			set;
		}

		protected ICollection<IEventSubscription> Subscriptions => _subscriptions;

		protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
		{
			if (eventSubscription == null)
			{
				throw new ArgumentNullException("eventSubscription");
			}
			eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);
			lock (Subscriptions)
			{
				Subscriptions.Add(eventSubscription);
			}
			return eventSubscription.SubscriptionToken;
		}

		protected virtual void InternalPublish(params object[] arguments)
		{
			foreach (Action<object[]> item in PruneAndReturnStrategies())
			{
				item(arguments);
			}
		}

		public virtual void Unsubscribe(SubscriptionToken token)
		{
			lock (Subscriptions)
			{
				IEventSubscription eventSubscription = Subscriptions.FirstOrDefault((IEventSubscription evt) => evt.SubscriptionToken == token);
				if (eventSubscription != null)
				{
					Subscriptions.Remove(eventSubscription);
				}
			}
		}

		public virtual bool Contains(SubscriptionToken token)
		{
			lock (Subscriptions)
			{
				return Subscriptions.FirstOrDefault((IEventSubscription evt) => evt.SubscriptionToken == token) != null;
			}
		}

		private List<Action<object[]>> PruneAndReturnStrategies()
		{
			List<Action<object[]>> list = new List<Action<object[]>>();
			lock (Subscriptions)
			{
				for (int num = Subscriptions.Count - 1; num >= 0; num--)
				{
					Action<object[]> executionStrategy = _subscriptions[num].GetExecutionStrategy();
					if (executionStrategy == null)
					{
						_subscriptions.RemoveAt(num);
					}
					else
					{
						list.Add(executionStrategy);
					}
				}
				return list;
			}
		}
	}
}
