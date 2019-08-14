namespace Prism.Interactivity.InteractionRequest
{
	public interface INotification
	{
		string Title
		{
			get;
			set;
		}

		object Content
		{
			get;
			set;
		}
	}
}
