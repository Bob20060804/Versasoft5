using System.Windows.Interactivity;

namespace Prism.Interactivity.InteractionRequest
{
	public class InteractionRequestTrigger : EventTrigger
	{
		protected override string GetEventName()
		{
			return "Raised";
		}
	}
}
