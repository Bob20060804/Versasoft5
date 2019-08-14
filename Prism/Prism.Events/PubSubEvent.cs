using Prism.Properties;
using System;
using System.Linq;

namespace Prism.Events
{
	public class PubSubEvent : EventBase
	{
		public SubscriptionToken Subscribe(Action action)
		{
			return Subscribe(action, ThreadOption.PublisherThread);
		}

		public SubscriptionToken Subscribe(Action action, ThreadOption threadOption)
		{
			return Subscribe(action, threadOption, keepSubscriberReferenceAlive: false);
		}

		public SubscriptionToken Subscribe(Action action, bool keepSubscriberReferenceAlive)
		{
			return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
		}

		public virtual SubscriptionToken Subscribe(Action action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
		{
			IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
			EventSubscription eventSubscription;
			switch (threadOption)
			{
			case ThreadOption.PublisherThread:
				eventSubscription = new EventSubscription(actionReference);
				break;
			case ThreadOption.BackgroundThread:
				eventSubscription = new BackgroundEventSubscription(actionReference);
				break;
			case ThreadOption.UIThread:
				if (base.SynchronizationContext == null)
				{
					throw new InvalidOperationException(Resources.EventAggregatorNotConstructedOnUIThread);
				}
				eventSubscription = new DispatcherEventSubscription(actionReference, base.SynchronizationContext);
				break;
			default:
				eventSubscription = new EventSubscription(actionReference);
				break;
			}
			return InternalSubscribe(eventSubscription);
		}

		public virtual void Publish()
		{
			InternalPublish();
		}

		public virtual void Unsubscribe(Action subscriber)
		{
			lock (base.Subscriptions)
			{
				IEventSubscription eventSubscription = base.Subscriptions.Cast<EventSubscription>().FirstOrDefault((EventSubscription evt) => evt.Action == subscriber);
				if (eventSubscription != null)
				{
					base.Subscriptions.Remove(eventSubscription);
				}
			}
		}

		public virtual bool Contains(Action subscriber)
		{
			IEventSubscription eventSubscription;
			lock (base.Subscriptions)
			{
				eventSubscription = base.Subscriptions.Cast<EventSubscription>().FirstOrDefault((EventSubscription evt) => evt.Action == subscriber);
			}
			return eventSubscription != null;
		}
	}
	public class PubSubEvent<TPayload> : EventBase
	{
		public SubscriptionToken Subscribe(Action<TPayload> action)
		{
			return Subscribe(action, ThreadOption.PublisherThread);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
		{
			return Subscribe(action, threadOption, keepSubscriberReferenceAlive: false);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
		{
			return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
		{
			return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
		}

		public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
		{
			IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
			IDelegateReference filterReference = (filter == null) ? new DelegateReference((Predicate<TPayload>)((TPayload _003Cobj_003E) => true), keepReferenceAlive: true) : new DelegateReference(filter, keepSubscriberReferenceAlive);
			EventSubscription<TPayload> eventSubscription;
			switch (threadOption)
			{
			case ThreadOption.PublisherThread:
				eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
				break;
			case ThreadOption.BackgroundThread:
				eventSubscription = new BackgroundEventSubscription<TPayload>(actionReference, filterReference);
				break;
			case ThreadOption.UIThread:
				if (base.SynchronizationContext == null)
				{
					throw new InvalidOperationException(Resources.EventAggregatorNotConstructedOnUIThread);
				}
				eventSubscription = new DispatcherEventSubscription<TPayload>(actionReference, filterReference, base.SynchronizationContext);
				break;
			default:
				eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
				break;
			}
			return InternalSubscribe(eventSubscription);
		}

		public virtual void Publish(TPayload payload)
		{
			InternalPublish(payload);
		}

		public virtual void Unsubscribe(Action<TPayload> subscriber)
		{
			lock (base.Subscriptions)
			{
				IEventSubscription eventSubscription = base.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault((EventSubscription<TPayload> evt) => evt.Action == subscriber);
				if (eventSubscription != null)
				{
					base.Subscriptions.Remove(eventSubscription);
				}
			}
		}

		public virtual bool Contains(Action<TPayload> subscriber)
		{
			IEventSubscription eventSubscription;
			lock (base.Subscriptions)
			{
				eventSubscription = base.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault((EventSubscription<TPayload> evt) => evt.Action == subscriber);
			}
			return eventSubscription != null;
		}
	}
}
