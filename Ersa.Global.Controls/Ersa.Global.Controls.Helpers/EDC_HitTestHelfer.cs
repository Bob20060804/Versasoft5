using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ersa.Global.Controls.Helpers
{
	public static class EDC_HitTestHelfer
	{
		public static IInputElement FUN_fdcHitTestNurSichtbare(UIElement i_fdcReferenzObjekt, Point i_sttPosition)
		{
			return i_fdcReferenzObjekt?.InputHitTest(i_sttPosition);
		}

		public static Task<bool> FUN_fdcPruefeObPositionUeberButtonAsync(Visual i_fdcReferenzObjekt, Point i_sttPosition)
		{
			if (i_fdcReferenzObjekt == null)
			{
				return Task.FromResult(result: false);
			}
			TaskCompletionSource<bool> fdcTsc = new TaskCompletionSource<bool>();
			VisualTreeHelper.HitTest(i_fdcReferenzObjekt, FUN_fdcHitTestFilterSichtbarkeit, delegate(HitTestResult i_fdcHitTestResult)
			{
				if (i_fdcHitTestResult != null && i_fdcHitTestResult.VisualHit != null)
				{
					bool result = FUN_blnIstInnerhalbEinesButtons(i_fdcHitTestResult.VisualHit);
					fdcTsc.SetResult(result);
				}
				return HitTestResultBehavior.Stop;
			}, new PointHitTestParameters(i_sttPosition));
			return fdcTsc.Task;
		}

		public static void SUB_PruefeObPositionUeberButton(Visual i_fdcReferenzObjekt, Point i_sttPosition, Action<bool> i_delCallback)
		{
			if (i_fdcReferenzObjekt != null && i_delCallback != null)
			{
				VisualTreeHelper.HitTest(i_fdcReferenzObjekt, FUN_fdcHitTestFilterSichtbarkeit, delegate(HitTestResult i_fdcHitTestResult)
				{
					if (i_fdcHitTestResult != null && i_fdcHitTestResult.VisualHit != null)
					{
						bool obj = FUN_blnIstInnerhalbEinesButtons(i_fdcHitTestResult.VisualHit);
						i_delCallback(obj);
					}
					return HitTestResultBehavior.Stop;
				}, new PointHitTestParameters(i_sttPosition));
			}
		}

		private static HitTestFilterBehavior FUN_fdcHitTestFilterSichtbarkeit(DependencyObject i_fdcPotentiellesHitTestTarget)
		{
			bool flag = false;
			bool flag2 = false;
			UIElement uIElement = i_fdcPotentiellesHitTestTarget as UIElement;
			if (uIElement != null)
			{
				flag = uIElement.IsVisible;
				if (flag)
				{
					flag2 = uIElement.IsHitTestVisible;
				}
			}
			else
			{
				UIElement3D uIElement3D = i_fdcPotentiellesHitTestTarget as UIElement3D;
				if (uIElement3D != null)
				{
					flag = uIElement3D.IsVisible;
					if (flag)
					{
						flag2 = uIElement3D.IsHitTestVisible;
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					return HitTestFilterBehavior.ContinueSkipSelf;
				}
				return HitTestFilterBehavior.Continue;
			}
			return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
		}

		private static bool FUN_blnIstInnerhalbEinesButtons(DependencyObject i_fdcDependencyObject)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(i_fdcDependencyObject);
			if (parent != null && !FUN_blnIstElementKlickbarerButton(parent))
			{
				return FUN_blnIstInnerhalbEinesButtons(parent);
			}
			return parent != null;
		}

		private static bool FUN_blnIstElementKlickbarerButton(DependencyObject i_fdcElement)
		{
			if (i_fdcElement is DataGridRowHeader)
			{
				return false;
			}
			ButtonBase buttonBase = i_fdcElement as ButtonBase;
			if (buttonBase != null && buttonBase.IsVisible)
			{
				return buttonBase.IsHitTestVisible;
			}
			return false;
		}
	}
}
