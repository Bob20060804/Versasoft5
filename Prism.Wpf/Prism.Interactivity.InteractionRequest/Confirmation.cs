namespace Prism.Interactivity.InteractionRequest
{
	public class Confirmation : Notification, IConfirmation, INotification
	{
		public bool Confirmed
		{
			get;
			set;
		}
	}
}
