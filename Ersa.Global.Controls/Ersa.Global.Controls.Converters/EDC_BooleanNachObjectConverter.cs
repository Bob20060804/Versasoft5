using Ersa.Global.Controls.Converters.UniversalConverter;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(bool), typeof(object))]
	public class EDC_BooleanNachObjectConverter : EDC_UniversalBoolConverter<object>
	{
	}
}
