using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class EDC_BooleanNachVisibilityConverter : EDC_UniversalToVisibilityConverter<bool>
	{
		protected override Func<bool, bool> PRO_delPruefung => (bool i_blnWert) => i_blnWert;

		protected override Func<bool, bool> PRO_delBackPruefung => (bool i_blnWert) => i_blnWert;
	}
}
