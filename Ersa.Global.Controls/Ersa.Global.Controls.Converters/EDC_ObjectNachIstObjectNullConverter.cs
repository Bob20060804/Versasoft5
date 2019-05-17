using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(bool))]
	public class EDC_ObjectNachIstObjectNullConverter : EDC_UniversalToBoolConverter<object>
	{
		protected override Func<object, bool> PRO_delPruefung => (object i_objObject) => i_objObject == null;
	}
}
