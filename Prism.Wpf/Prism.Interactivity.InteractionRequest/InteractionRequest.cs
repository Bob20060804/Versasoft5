using System;

namespace Prism.Interactivity.InteractionRequest
{
	public class InteractionRequest<T> : IInteractionRequest where T : INotification
	{
		public event EventHandler<InteractionRequestedEventArgs> Raised;

		public void Raise(T context)
		{
			Raise(context, delegate
			{
			});
		}

		public void Raise(T context, Action<T> callback)
		{
			this.Raised?.Invoke(this, new InteractionRequestedEventArgs(context, delegate
			{
				if (callback != null)
				{
					callback(context);
				}
			}));
		}
	}
}
