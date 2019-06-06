using System.Windows;

namespace Ersa.Global.Controls.Converters.UniversalConverter
{
	public abstract class EDC_UniversalToVisibilityConverter<TValue> : EDC_UniversalConverter<TValue, Visibility>
	{
		protected EDC_UniversalToVisibilityConverter()
		{
			base.PRO_objTrueWert = Visibility.Visible;
			base.PRO_objFalseWert = Visibility.Collapsed;
		}
	}
}
