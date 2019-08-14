using System;
using System.Collections.Generic;

namespace Prism.Regions
{
	public class RegionNavigationJournal : IRegionNavigationJournal
	{
		private Stack<IRegionNavigationJournalEntry> backStack = new Stack<IRegionNavigationJournalEntry>();

		private Stack<IRegionNavigationJournalEntry> forwardStack = new Stack<IRegionNavigationJournalEntry>();

		private bool isNavigatingInternal;

		public INavigateAsync NavigationTarget
		{
			get;
			set;
		}

		public IRegionNavigationJournalEntry CurrentEntry
		{
			get;
			private set;
		}

		public bool CanGoBack => backStack.Count > 0;

		public bool CanGoForward => forwardStack.Count > 0;

		public void GoBack()
		{
			if (CanGoBack)
			{
				IRegionNavigationJournalEntry entry = backStack.Peek();
				InternalNavigate(entry, delegate(bool result)
				{
					if (result)
					{
						if (CurrentEntry != null)
						{
							forwardStack.Push(CurrentEntry);
						}
						backStack.Pop();
						CurrentEntry = entry;
					}
				});
			}
		}

		public void GoForward()
		{
			if (CanGoForward)
			{
				IRegionNavigationJournalEntry entry = forwardStack.Peek();
				InternalNavigate(entry, delegate(bool result)
				{
					if (result)
					{
						if (CurrentEntry != null)
						{
							backStack.Push(CurrentEntry);
						}
						forwardStack.Pop();
						CurrentEntry = entry;
					}
				});
			}
		}

		public void RecordNavigation(IRegionNavigationJournalEntry entry)
		{
			if (!isNavigatingInternal)
			{
				if (CurrentEntry != null)
				{
					backStack.Push(CurrentEntry);
				}
				forwardStack.Clear();
				CurrentEntry = entry;
			}
		}

		public void Clear()
		{
			CurrentEntry = null;
			backStack.Clear();
			forwardStack.Clear();
		}

		private void InternalNavigate(IRegionNavigationJournalEntry entry, Action<bool> callback)
		{
			isNavigatingInternal = true;
			NavigationTarget.RequestNavigate(entry.Uri, delegate(NavigationResult nr)
			{
				isNavigatingInternal = false;
				if (nr.Result.HasValue)
				{
					callback(nr.Result.Value);
				}
			}, entry.Parameters);
		}
	}
}
