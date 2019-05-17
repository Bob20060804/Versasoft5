using System;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters.UniversalConverter
{
	[ValueConversion(typeof(bool), typeof(object))]
	public abstract class EDC_UniversalBoolConverter<T> : EDC_UniversalConverter<bool, T>
	{
		protected override Func<bool, bool> PRO_delPruefung => (bool i_blnWert) => i_blnWert;
	}
}
