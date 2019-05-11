using System;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_UriHelfer
	{
		public static bool FUN_blnIstAbsoluteHttpUriKorrekt(string i_strUri)
		{
			return FUN_blnIstUriKorrekt(i_strUri, UriKind.Absolute, ENUM_UriSchema.Http);
		}

		public static bool FUN_blnIstAbsoluteHttpsUriKorrekt(string i_strUri)
		{
			return FUN_blnIstUriKorrekt(i_strUri, UriKind.Absolute, ENUM_UriSchema.Https);
		}

		public static bool FUN_blnIstAbsoluteFtpUriKorrekt(string i_strUri)
		{
			return FUN_blnIstUriKorrekt(i_strUri, UriKind.Absolute, ENUM_UriSchema.Ftp);
		}

		public static bool FUN_blnIstRelativeDateiUriKorrekt(string i_strUri)
		{
			return FUN_blnIstUriKorrekt(i_strUri, UriKind.Relative, ENUM_UriSchema.Datei);
		}

		public static bool FUN_blnIstAbsoluteDateiUriKorrekt(string i_strUri)
		{
			return FUN_blnIstUriKorrekt(i_strUri, UriKind.Absolute, ENUM_UriSchema.Datei);
		}

		private static bool FUN_blnIstUriKorrekt(string i_strUri, UriKind i_fdcUriKind, ENUM_UriSchema i_edcUriSchema)
		{
			if (string.IsNullOrEmpty(i_strUri))
			{
				throw new ArgumentException("URI should not be null or emtpy", i_strUri);
			}
			if (!Uri.TryCreate(i_strUri, i_fdcUriKind, out Uri result))
			{
				return false;
			}
			if (i_fdcUriKind == UriKind.Absolute && FUN_blnIstUriSchemaKorrekt(result, i_edcUriSchema))
			{
				return true;
			}
			return false;
		}

		private static bool FUN_blnIstUriSchemaKorrekt(Uri i_fdcUri, ENUM_UriSchema i_edcUriSchema)
		{
			switch (i_edcUriSchema)
			{
			case ENUM_UriSchema.Http:
				if (i_fdcUri.Scheme == Uri.UriSchemeHttp)
				{
					return true;
				}
				break;
			case ENUM_UriSchema.Https:
				if (i_fdcUri.Scheme == Uri.UriSchemeHttps)
				{
					return true;
				}
				break;
			case ENUM_UriSchema.Ftp:
				if (i_fdcUri.Scheme == Uri.UriSchemeFtp)
				{
					return true;
				}
				break;
			case ENUM_UriSchema.Datei:
				if (i_fdcUri.Scheme == Uri.UriSchemeFile)
				{
					return true;
				}
				break;
			}
			return false;
		}
	}
}
