using System;

namespace Prism.Interactivity.InteractionRequest
{
	public interface IInteractionRequestAware
	{
		INotification Notification
		{
			get;
			set;
		}

		Action FinishInteraction
		{
			get;
			set;
		}
	}
}
