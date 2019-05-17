using Prism.Interactivity.InteractionRequest;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public abstract class EDC_BenutzerAbfrage<T> : INotification
	{
		public T PRO_edcErgebnis
		{
			get;
			set;
		}

		public string PRO_strText
		{
			get;
			set;
		}

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
