using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class EDC_ObjectNachIstObjectNullVisibilityConverter : EDC_UniversalToVisibilityConverter<object>
	{
		protected override Func<object, bool> PRO_delPruefung => (object i_objObject) => i_objObject == null;
	}
}
