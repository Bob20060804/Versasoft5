using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor
{
	public static class EDC_BildEditorKonstanten
	{
		public const string gC_strDefaultFontFamilyName = "Arial";

		public const double gC_dblDefaultSkalierung = 1.0;

		public const double gC_dblDefaultStrichStaerke = 2.0;

		public const double gC_dblDefaultFontSize = 20.0;

		public const double gC_blnHitTestWidth = 15.0;

		public const double gC_blnHandleSize = 10.0;

		public static readonly Color ms_fdcDefaultGrafikFarbe = Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue);

		public static readonly Color ms_fdcDefaultTrackerFarbeExtern = Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0);

		public static readonly Color ms_fdcDefaultTrackerFarbeMitte = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color ms_fdcDefaultTrackerFarbeIntern = Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue);

		public static readonly FontStyle ms_fdcDefaultFontStyle = FontStyles.Normal;

		public static readonly FontWeight ms_fdcDefaultFontWeight = FontWeights.Normal;

		public static readonly FontStretch ms_fdcDefaultFontStretch = FontStretches.Normal;
	}
}
