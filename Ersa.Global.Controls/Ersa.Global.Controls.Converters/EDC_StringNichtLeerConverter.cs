using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(string), typeof(bool))]
	public class EDC_StringNichtLeerConverter : EDC_UniversalToBoolConverter<string>
	{
		protected override Func<string, bool> PRO_delPruefung => (string i_strString) => !string.IsNullOrEmpty(i_strString);
	}
}
