using System;

namespace Prism.Events
{
	public interface IEventSubscription
	{
		SubscriptionToken SubscriptionToken
		{
			get;
			set;
		}

		Action<object[]> GetExecutionStrategy();
	}
}
