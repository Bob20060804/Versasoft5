using System;

namespace Prism.Interactivity.InteractionRequest
{
	public interface IInteractionRequest
	{
		event EventHandler<InteractionRequestedEventArgs> Raised;
	}
}
