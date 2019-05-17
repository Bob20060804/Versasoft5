using Ersa.Global.Common.Helper;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.Infrastructure.Prism;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Ersa.Platform.UI.Converters
{
	[ValueConversion(typeof(long), typeof(BitmapImage))]
	public class EDC_ProgrammIdNachBildConverter : IValueConverter
	{
		private static readonly object ms_objSyncObject = new object();

		private static INF_ProgrammBildDienst ms_edcProgrammBildDienst;

		private static INF_ProgrammBildDienst PRO_edcProgrammBildDienst
		{
			get
			{
				lock (ms_objSyncObject)
				{
					return ms_edcProgrammBildDienst ?? (ms_edcProgrammBildDienst = FUN_edcProgrammBildDienstHolen());
				}
			}
		}

		public object Convert(object i_objValue, Type i_objTargetType, object i_objParameter, CultureInfo i_objCultureInfo)
		{
			if (!(i_objValue is long))
			{
				return null;
			}
			long i64Id = (long)i_objValue;
			if (i64Id == 0L)
			{
				return null;
			}
			return EDC_TaskHelfer.FUN_objRunSync(() => FUN_fdcBildLadenAsync(i64Id, (i_objParameter as ENUM_BildVerwendung?) ?? ENUM_BildVerwendung.enmThumbnail));
		}

		public object ConvertBack(object i_objValue, Type i_objTargetType, object i_objParameter, CultureInfo i_objCultureInfo)
		{
			return Binding.DoNothing;
		}

		private static INF_ProgrammBildDienst FUN_edcProgrammBildDienstHolen()
		{
			return EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<INF_ProgrammBildDienst>();
		}

		private async Task<object> FUN_fdcBildLadenAsync(long i_i64Id, ENUM_BildVerwendung i_enmVerwendung)
		{
			BitmapImage bitmapImage = await PRO_edcProgrammBildDienst.FUN_fdcBildLadenUndNachBitmapImageKonvertierenAsync(i_i64Id, i_enmVerwendung).ConfigureAwait(continueOnCapturedContext: true);
			if (bitmapImage != null)
			{
				bitmapImage.Freeze();
				return bitmapImage;
			}
			return null;
		}
	}
}
