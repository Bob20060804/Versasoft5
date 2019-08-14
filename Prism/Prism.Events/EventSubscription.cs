using Prism.Properties;
using System;
using System.Globalization;

namespace Prism.Events
{
	public class EventSubscription : IEventSubscription
	{
		private readonly IDelegateReference _actionReference;

		public Action Action => (Action)_actionReference.Target;

		public SubscriptionToken SubscriptionToken
		{
			get;
			set;
		}

		public EventSubscription(IDelegateReference actionReference)
		{
			if (actionReference == null)
			{
				throw new ArgumentNullException("actionReference");
			}
			if (!(actionReference.Target is Action))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, new object[1]
				{
					typeof(Action).FullName
				}), "actionReference");
			}
			_actionReference = actionReference;
		}

		public virtual Action<object[]> GetExecutionStrategy()
		{
			Action action = Action;
			if (action != null)
			{
				return delegate
				{
					InvokeAction(action);
				};
			}
			return null;
		}

		public virtual void InvokeAction(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action();
		}
	}
	public class EventSubscription<TPayload> : IEventSubscription
	{
		private readonly IDelegateReference _actionReference;

		private readonly IDelegateReference _filterReference;

		public Action<TPayload> Action => (Action<TPayload>)_actionReference.Target;

		public Predicate<TPayload> Filter => (Predicate<TPayload>)_filterReference.Target;

		public SubscriptionToken SubscriptionToken
		{
			get;
			set;
		}

		public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
		{
			if (actionReference == null)
			{
				throw new ArgumentNullException("actionReference");
			}
			if (!(actionReference.Target is Action<TPayload>))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, new object[1]
				{
					typeof(Action<TPayload>).FullName
				}), "actionReference");
			}
			if (filterReference == null)
			{
				throw new ArgumentNullException("filterReference");
			}
			if (!(filterReference.Target is Predicate<TPayload>))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, new object[1]
				{
					typeof(Predicate<TPayload>).FullName
				}), "filterReference");
			}
			_actionReference = actionReference;
			_filterReference = filterReference;
		}

		public virtual Action<object[]> GetExecutionStrategy()
		{
			Action<TPayload> action = Action;
			Predicate<TPayload> filter = Filter;
			if (action != null && filter != null)
			{
				return delegate(object[] arguments)
				{
					TPayload val = default(TPayload);
					if (arguments != null && arguments.Length != 0 && arguments[0] != null)
					{
						val = (TPayload)arguments[0];
					}
					if (filter(val))
					{
						InvokeAction(action, val);
					}
				};
			}
			return null;
		}

		public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action(argument);
		}
	}
}
