using System.IO;
using System.Windows.Media.Imaging;

namespace Ersa.Global.Controls.Helpers
{
	public static class EDC_BitmapImageHelfer
	{
		public static BitmapImage FUN_fdcBitmapImageErzeugen(Stream i_fdcBildStream)
		{
			return FUN_fdcBitmapImageErzeugen(i_fdcBildStream, null, null);
		}

		public static BitmapImage FUN_fdcBitmapImageMitFestgelegterBreiteErzeugen(Stream i_fdcBildStream, int i_i32Breite)
		{
			return FUN_fdcBitmapImageErzeugen(i_fdcBildStream, i_i32Breite, null);
		}

		public static BitmapImage FUN_fdcBitmapImageMitFestgelegterHoeheErzeugen(Stream i_fdcBildStream, int i_i32Hoehe)
		{
			return FUN_fdcBitmapImageErzeugen(i_fdcBildStream, null, i_i32Hoehe);
		}

		private static BitmapImage FUN_fdcBitmapImageErzeugen(Stream i_fdcBildStream, int? i_i32Breite, int? i_i32Hoehe)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = i_fdcBildStream;
			if (i_i32Breite.HasValue)
			{
				bitmapImage.DecodePixelWidth = i_i32Breite.Value;
			}
			if (i_i32Hoehe.HasValue)
			{
				bitmapImage.DecodePixelHeight = i_i32Hoehe.Value;
			}
			bitmapImage.EndInit();
			return bitmapImage;
		}
	}
}
