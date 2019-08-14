using System;
using System.Globalization;

namespace Prism.Regions
{
	public class RegionNavigationJournalEntry : IRegionNavigationJournalEntry
	{
		public Uri Uri
		{
			get;
			set;
		}

		public NavigationParameters Parameters
		{
			get;
			set;
		}

		public override string ToString()
		{
			if (Uri != null)
			{
				return string.Format(CultureInfo.CurrentCulture, "RegionNavigationJournalEntry:'{0}'", new object[1]
				{
					Uri.ToString()
				});
			}
			return base.ToString();
		}
	}
}
