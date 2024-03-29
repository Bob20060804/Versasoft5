namespace Prism.Events
{
	public interface IEventAggregator
	{
		TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
	}
}
