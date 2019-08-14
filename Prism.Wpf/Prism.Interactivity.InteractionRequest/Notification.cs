namespace Prism.Interactivity.InteractionRequest
{
	public class Notification : INotification
	{
		public string Title
		{
			get;
			set;
		}

		public object Content
		{
			get;
			set;
		}
	}
}
