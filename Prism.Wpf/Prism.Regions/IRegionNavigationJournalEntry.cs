using System;

namespace Prism.Regions
{
	public interface IRegionNavigationJournalEntry
	{
		Uri Uri
		{
			get;
			set;
		}

		NavigationParameters Parameters
		{
			get;
			set;
		}
	}
}
