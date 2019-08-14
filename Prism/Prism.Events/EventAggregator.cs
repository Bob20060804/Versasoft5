using System;
using System.Collections.Generic;
using System.Threading;

namespace Prism.Events
{
	public class EventAggregator : IEventAggregator
	{
		private readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();

		private readonly SynchronizationContext syncContext = SynchronizationContext.Current;

		public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
		{
			lock (events)
			{
				EventBase value = null;
				if (!events.TryGetValue(typeof(TEventType), out value))
				{
					TEventType val = new TEventType();
					val.SynchronizationContext = syncContext;
					events[typeof(TEventType)] = val;
					return val;
				}
				return (TEventType)value;
			}
		}
	}
}
