namespace Prism.Interactivity.InteractionRequest
{
	public interface IConfirmation : INotification
	{
		bool Confirmed
		{
			get;
			set;
		}
	}
}
