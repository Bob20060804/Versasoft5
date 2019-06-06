using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(bool), typeof(bool))]
	public class EDC_BooleanNachBooleanConverter : EDC_UniversalToBoolConverter<bool>
	{
		protected override Func<bool, bool> PRO_delPruefung => (bool i_blnWert) => i_blnWert;

		protected override Func<bool, bool> PRO_delBackPruefung => (bool i_blnWert) => i_blnWert;
	}
}
