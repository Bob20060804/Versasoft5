using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace Ersa.Global.Controls
{
	public abstract class EDU_WindowBasis : Window
	{
		private static readonly FieldInfo ms_fdcMenuDropAlignmentField;

		static EDU_WindowBasis()
		{
			ms_fdcMenuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.Static | BindingFlags.NonPublic);
			SUB_StandardPopupAlignmentSicherstellen();
			SystemParameters.StaticPropertyChanged += SUB_SystemParameterGeaendert;
		}

		private static void SUB_SystemParameterGeaendert(object i_objSender, PropertyChangedEventArgs i_fdcArgs)
		{
			SUB_StandardPopupAlignmentSicherstellen();
		}

		private static void SUB_StandardPopupAlignmentSicherstellen()
		{
			if (SystemParameters.MenuDropAlignment && ms_fdcMenuDropAlignmentField != null)
			{
				ms_fdcMenuDropAlignmentField.SetValue(null, false);
			}
		}
	}
}
