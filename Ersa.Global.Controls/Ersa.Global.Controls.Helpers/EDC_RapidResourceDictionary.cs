using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Ersa.Global.Controls.Helpers
{
	public abstract class EDC_RapidResourceDictionary : ResourceDictionary
	{
		protected static readonly IList<string> ms_lstAddedApplicationSources = new List<string>();

		private static readonly object ms_objSyncObject = new object();

		private Uri m_fdcSourceUri;

		public new Uri Source
		{
			get
			{
				if (!FUN_blnIsInDesignMode())
				{
					return m_fdcSourceUri;
				}
				return base.Source;
			}
			set
			{
				if (FUN_blnIsInDesignMode())
				{
					base.Source = value;
				}
				else
				{
					lock (ms_objSyncObject)
					{
						m_fdcSourceUri = value;
						string text = FUN_strNormalizedSource(m_fdcSourceUri);
						if (!ms_lstAddedApplicationSources.Contains(text))
						{
							SUB_SetSource(value, text);
						}
					}
				}
			}
		}

		protected void SUB_SetBaseSource(Uri i_fdcUri)
		{
			base.Source = i_fdcUri;
		}

		protected abstract void SUB_SetSource(Uri i_fdcUri, string i_strSourceIdentifier);

		private static bool FUN_blnIsInDesignMode()
		{
			if (Application.Current != null)
			{
				return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;
			}
			return true;
		}

		private string FUN_strNormalizedSource(Uri i_fdcUri)
		{
			if (!i_fdcUri.OriginalString.StartsWith("pack://application:,,,/"))
			{
				return i_fdcUri.OriginalString;
			}
			return i_fdcUri.OriginalString.Replace("pack://application:,,,", string.Empty);
		}
	}
}
