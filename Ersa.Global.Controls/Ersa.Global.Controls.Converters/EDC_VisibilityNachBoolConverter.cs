using Ersa.Global.Controls.Converters.UniversalConverter;
using System;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	[ValueConversion(typeof(Visibility), typeof(bool?))]
	public class EDC_VisibilityNachBoolConverter : EDC_UniversalToBoolConverter<Visibility>
	{
		protected override Func<Visibility, bool> PRO_delPruefung => (Visibility i_enmVisibility) => i_enmVisibility == Visibility.Visible;

		protected override Func<bool, Visibility> PRO_delBackPruefung => delegate(bool i_blnWert)
		{
			if (!i_blnWert)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		};
	}
}
