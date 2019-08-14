using System;

namespace Prism.Interactivity.InteractionRequest
{
	public class InteractionRequestedEventArgs : EventArgs
	{
		public INotification Context
		{
			get;
			private set;
		}

		public Action Callback
		{
			get;
			private set;
		}

		public InteractionRequestedEventArgs(INotification context, Action callback)
		{
			Context = context;
			Callback = callback;
		}
	}
}
