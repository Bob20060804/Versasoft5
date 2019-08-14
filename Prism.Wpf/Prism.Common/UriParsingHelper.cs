using Prism.Regions;
using System;

namespace Prism.Common
{
	public static class UriParsingHelper
	{
		public static string GetQuery(Uri uri)
		{
			return EnsureAbsolute(uri).Query;
		}

		public static string GetAbsolutePath(Uri uri)
		{
			return EnsureAbsolute(uri).AbsolutePath;
		}

		public static NavigationParameters ParseQuery(Uri uri)
		{
			return new NavigationParameters(GetQuery(uri));
		}

		private static Uri EnsureAbsolute(Uri uri)
		{
			if (uri.IsAbsoluteUri)
			{
				return uri;
			}
			if (uri != null && !uri.OriginalString.StartsWith("/", StringComparison.Ordinal))
			{
				return new Uri("http://localhost/" + uri, UriKind.Absolute);
			}
			return new Uri("http://localhost" + uri, UriKind.Absolute);
		}
	}
}
