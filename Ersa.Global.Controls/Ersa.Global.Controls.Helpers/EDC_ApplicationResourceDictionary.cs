using System;
using System.Windows;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_ApplicationResourceDictionary : EDC_RapidResourceDictionary
	{
		protected override void SUB_SetSource(Uri i_fdcUri, string i_strSourceIdentifier)
		{
			if (Application.Current.Resources.MergedDictionaries.Contains(this))
			{
				Application.Current.Resources.MergedDictionaries.Remove(this);
			}
			EDC_SharedResourceDictionary item = new EDC_SharedResourceDictionary
			{
				Source = i_fdcUri
			};
			Application.Current.Resources.MergedDictionaries.Add(item);
			EDC_RapidResourceDictionary.ms_lstAddedApplicationSources.Add(i_strSourceIdentifier);
		}
	}
}
