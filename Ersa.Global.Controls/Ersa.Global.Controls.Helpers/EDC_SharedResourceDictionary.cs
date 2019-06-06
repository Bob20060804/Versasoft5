using System;
using System.Collections.Generic;
using System.Windows;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_SharedResourceDictionary : EDC_RapidResourceDictionary
	{
		private static readonly Dictionary<string, ResourceDictionary> ms_dicShared = new Dictionary<string, ResourceDictionary>();

		protected override void SUB_SetSource(Uri i_fdcUri, string i_strSourceIdentifier)
		{
			if (!ms_dicShared.ContainsKey(i_strSourceIdentifier))
			{
				SUB_SetBaseSource(i_fdcUri);
				ms_dicShared.Add(i_strSourceIdentifier, this);
			}
			else
			{
				base.MergedDictionaries.Add(ms_dicShared[i_strSourceIdentifier]);
			}
		}
	}
}
