namespace Prism.Regions
{
	public interface IRegionNavigationJournal
	{
		bool CanGoBack
		{
			get;
		}

		bool CanGoForward
		{
			get;
		}

		IRegionNavigationJournalEntry CurrentEntry
		{
			get;
		}

		INavigateAsync NavigationTarget
		{
			get;
			set;
		}

		void GoBack();

		void GoForward();

		void RecordNavigation(IRegionNavigationJournalEntry entry);

		void Clear();
	}
}
