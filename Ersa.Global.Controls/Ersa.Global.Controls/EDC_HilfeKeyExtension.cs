using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public static class EDC_HilfeKeyExtension
	{
		public static readonly RoutedCommand PRO_cmdExpliziteHilfeAnfordernCommand = new RoutedCommand();

		public static readonly DependencyProperty PRO_strHilfeKeyProperty = DependencyProperty.RegisterAttached("PRO_strHilfeKey", typeof(string), typeof(EDC_HilfeKeyExtension));

		public static string GetPRO_strHilfeKey(DependencyObject obj)
		{
			return (string)obj.GetValue(PRO_strHilfeKeyProperty);
		}

		public static void SetPRO_strHilfeKey(DependencyObject obj, string value)
		{
			obj.SetValue(PRO_strHilfeKeyProperty, value);
		}
	}
}
