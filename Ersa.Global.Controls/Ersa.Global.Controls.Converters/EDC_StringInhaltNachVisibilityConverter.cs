using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(string), typeof(Visibility))]
	public class EDC_StringInhaltNachVisibilityConverter : EDC_UniversalToVisibilityConverter<string>
	{
		protected override Func<string, bool> PRO_delPruefung => (string i_strString) => !string.IsNullOrEmpty(i_strString);
	}
}
