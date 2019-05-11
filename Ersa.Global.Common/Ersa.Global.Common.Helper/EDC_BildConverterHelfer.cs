using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_BildConverterHelfer
	{
		public static Image FUN_fdcBildGroesseAendern(Image i_fdcBild, int i_i32NeueBreite, int i_i32NeueHoehe)
		{
			if (i_fdcBild.Width == i_i32NeueBreite && i_fdcBild.Height == i_i32NeueHoehe)
			{
				return i_fdcBild;
			}
			Image image = new Bitmap(i_i32NeueBreite, i_i32NeueHoehe);
			using (Graphics graphics = Graphics.FromImage(image))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(i_fdcBild, 0, 0, i_i32NeueBreite, i_i32NeueHoehe);
				return image;
			}
		}

		public static Image FUN_fdcPasseBildAnMaximaleGroesseAn(Image i_fdcBild, int i_i32MaximaleBreite, int i_i32MaximaleHoehe)
		{
			double val = (double)i_fdcBild.Width / (double)i_i32MaximaleBreite;
			double val2 = (double)i_fdcBild.Height / (double)i_i32MaximaleHoehe;
			double num = Math.Max(val, val2);
			if (num <= 1.0)
			{
				return i_fdcBild;
			}
			return FUN_fdcBildGroesseAendern(i_fdcBild, (int)((double)i_fdcBild.Width / num), (int)((double)i_fdcBild.Height / num));
		}

		public static byte[] FUNa_bytImageToByteArray(Bitmap i_fdcBild)
		{
			if (i_fdcBild == null)
			{
				return null;
			}
			Bitmap bitmap = new Bitmap(i_fdcBild);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bitmap.Save(memoryStream, ImageFormat.Jpeg);
				memoryStream.Close();
				return memoryStream.ToArray();
			}
		}

		public static byte[] FUNa_bytBitmapImageToByteArray(BitmapImage i_fdcBitmapImage)
		{
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(i_fdcBitmapImage));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				pngBitmapEncoder.Save(memoryStream);
				return memoryStream.ToArray();
			}
		}

		public static Bitmap FUN_fdcByteArrayToImage(byte[] ia_bytArray)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(ia_bytArray, 0, ia_bytArray.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return (Bitmap)Image.FromStream(memoryStream);
		}

		public static Bitmap FUN_fdcWriteableBitmapToBitmap(WriteableBitmap i_fdcWriteableBitmap)
		{
			MemoryStream stream = new MemoryStream();
			BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
			bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(i_fdcWriteableBitmap));
			bmpBitmapEncoder.Save(stream);
			return new Bitmap(stream);
		}

		public static BitmapImage FUN_edcByteArrayNachBitmapImageKonvertieren(byte[] ia_bytArray)
		{
			MemoryStream streamSource = new MemoryStream(ia_bytArray);
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = streamSource;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		public static BitmapImage FUN_edcBitmapNachBitmapImageKonvertieren(Bitmap i_fdcBitmap)
		{
			MemoryStream memoryStream = new MemoryStream();
			new Bitmap(i_fdcBitmap).Save(memoryStream, ImageFormat.Jpeg);
			memoryStream.Position = 0L;
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		public static BitmapImage FUN_fdcWriteableBitmapNachBitmapImage(WriteableBitmap i_fdcWriteableBitmap)
		{
			BitmapImage bitmapImage = new BitmapImage();
			MemoryStream memoryStream = new MemoryStream();
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(i_fdcWriteableBitmap));
			pngBitmapEncoder.Save(memoryStream);
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
			return bitmapImage;
		}

		public static BitmapImage FUN_fdcBitmapSourceNachBitmapImage(BitmapSource i_fdcBitmapSource)
		{
			JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
			MemoryStream memoryStream = new MemoryStream();
			BitmapImage bitmapImage = new BitmapImage();
			jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(i_fdcBitmapSource));
			jpegBitmapEncoder.Save(memoryStream);
			memoryStream.Position = 0L;
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.EndInit();
			return bitmapImage;
		}
	}
}
