using Ersa.Global.Controls.Helpers;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Ersa.Platform.UI.Common
{
	[Export(typeof(INF_BildschirmVerwaltungsDienst))]
	public class EDC_BildschirmVerwaltungsDienst : INF_BildschirmVerwaltungsDienst
	{
		public EDC_Bildschirm PRO_edcPrimaerBildschirm => FUN_edcInBildschirmKonvertieren(Screen.PrimaryScreen);

		public IEnumerable<EDC_Bildschirm> FUN_enuAlleBildschirmeErmitteln()
		{
			return from i_edcBildschirm in Screen.AllScreens.Select(FUN_edcInBildschirmKonvertieren)
			orderby i_edcBildschirm.PRO_fdcBereich.Left
			select i_edcBildschirm;
		}

		public EDC_Bildschirm FUN_edcErmittleBildschirm(Window i_fdcFenster)
		{
			Screen i_fdcScreen = Screen.FromHandle(new WindowInteropHelper(i_fdcFenster).Handle);
			return FUN_edcInBildschirmKonvertieren(i_fdcScreen);
		}

		public EDC_Bildschirm FUN_edcErmittleBildschirm(System.Windows.Point i_fdcPunkt)
		{
			int x = (int)Math.Round(i_fdcPunkt.X);
			int y = (int)Math.Round(i_fdcPunkt.Y);
			Screen i_fdcScreen = Screen.FromPoint(new System.Drawing.Point(x, y));
			return FUN_edcInBildschirmKonvertieren(i_fdcScreen);
		}

		public DependencyObject FUN_fdcErmittleObjektAnPosition(System.Windows.Point i_fdcPosition)
		{
			return EDC_HitTestHelfer.FUN_fdcHitTestNurSichtbare(System.Windows.Application.Current.MainWindow, i_fdcPosition) as DependencyObject;
		}

		public DependencyObject FUN_fdcErmittleObjektImAnwendungsMittelpunkt()
		{
			Window mainWindow = System.Windows.Application.Current.MainWindow;
			System.Windows.Point i_fdcPosition = new System.Windows.Point(mainWindow.ActualWidth / 2.0, mainWindow.ActualHeight / 2.0);
			return FUN_fdcErmittleObjektAnPosition(i_fdcPosition);
		}

		private EDC_Bildschirm FUN_edcInBildschirmKonvertieren(Screen i_fdcScreen)
		{
			string deviceName = i_fdcScreen.DeviceName;
			int result = 0;
			if (deviceName.StartsWith("\\\\.\\DISPLAY"))
			{
				int.TryParse(deviceName.Substring(11), out result);
			}
			return new EDC_Bildschirm(result, deviceName, i_fdcScreen.Primary, FUN_fdcRectKonvertieren(i_fdcScreen.Bounds), FUN_fdcRectKonvertieren(i_fdcScreen.WorkingArea));
		}

		private Rect FUN_fdcRectKonvertieren(Rectangle i_fdcRect)
		{
			Rect result = default(Rect);
			result.X = i_fdcRect.X;
			result.Y = i_fdcRect.Y;
			result.Width = i_fdcRect.Width;
			result.Height = i_fdcRect.Height;
			return result;
		}
	}
}
