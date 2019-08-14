using System;
using System.Threading;

namespace Prism.Events
{
	public class DispatcherEventSubscription : EventSubscription
	{
		private readonly SynchronizationContext syncContext;

		public DispatcherEventSubscription(IDelegateReference actionReference, SynchronizationContext context)
			: base(actionReference)
		{
			syncContext = context;
		}

		public override void InvokeAction(Action action)
		{
			syncContext.Post(delegate
			{
				action();
			}, null);
		}
	}
	public class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
	{
		private readonly SynchronizationContext syncContext;

		public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context)
			: base(actionReference, filterReference)
		{
			syncContext = context;
		}

		public override void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			syncContext.Post(delegate(object o)
			{
				action((TPayload)o);
			}, argument);
		}
	}
}
