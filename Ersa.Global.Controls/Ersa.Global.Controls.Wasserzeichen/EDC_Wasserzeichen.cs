using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Ersa.Global.Controls.Wasserzeichen
{
	public static class EDC_Wasserzeichen
	{
		public static readonly DependencyProperty PRO_strTextProperty = DependencyProperty.RegisterAttached("PRO_strText", typeof(object), typeof(EDC_Wasserzeichen), new FrameworkPropertyMetadata(null, SUB_WasserzeichenGeaendert));

		public static readonly DependencyProperty PRO_fdcResourcesProperty = DependencyProperty.RegisterAttached("PRO_fdcResources", typeof(ResourceDictionary), typeof(EDC_Wasserzeichen), new FrameworkPropertyMetadata(null, SUB_WasserzeichenGeaendert));

		public static object GetPRO_strText(DependencyObject i_fdcObjekt)
		{
			return i_fdcObjekt.GetValue(PRO_strTextProperty);
		}

		public static void SetPRO_strText(DependencyObject i_fdcObjekt, object i_objWert)
		{
			i_fdcObjekt.SetValue(PRO_strTextProperty, i_objWert);
		}

		public static ResourceDictionary GetPRO_fdcResources(DependencyObject i_fdcObjekt)
		{
			return (ResourceDictionary)i_fdcObjekt.GetValue(PRO_fdcResourcesProperty);
		}

		public static void SetPRO_fdcResources(DependencyObject i_fdcObjekt, ResourceDictionary i_fdcWert)
		{
			i_fdcObjekt.SetValue(PRO_fdcResourcesProperty, i_fdcWert);
		}

		private static void SUB_WasserzeichenGeaendert(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			Control obj = (Control)i_objSender;
			ComboBox comboBox = obj as ComboBox;
			if (comboBox != null)
			{
				comboBox.SelectionChanged -= SUB_ComboBoxSelectionChanged;
				comboBox.SelectionChanged += SUB_ComboBoxSelectionChanged;
			}
			SUB_WasserzeichenAktualisieren(obj);
		}

		private static void SUB_ComboBoxSelectionChanged(object i_objSender, SelectionChangedEventArgs i_fdcArgs)
		{
			SUB_WasserzeichenAktualisieren((ComboBox)i_objSender);
		}

		private static void SUB_WasserzeichenAktualisieren(Control i_fdcControl)
		{
			if (FUN_blnSollWasserzeichenAngezeigtWerden(i_fdcControl))
			{
				SUB_WasserzeichenAnzeigen(i_fdcControl);
			}
			else
			{
				SUB_WasserzeichenEntfernen(i_fdcControl);
			}
		}

		private static void SUB_WasserzeichenEntfernen(UIElement i_fdcControl)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(i_fdcControl);
			if (adornerLayer == null)
			{
				return;
			}
			System.Windows.Documents.Adorner[] adorners = adornerLayer.GetAdorners(i_fdcControl);
			if (adorners == null)
			{
				return;
			}
			System.Windows.Documents.Adorner[] array = adorners;
			foreach (System.Windows.Documents.Adorner adorner in array)
			{
				if (adorner is EDU_WasserzeichenAdorner)
				{
					adorner.Visibility = Visibility.Hidden;
					adornerLayer.Remove(adorner);
				}
			}
		}

		private static void SUB_WasserzeichenAnzeigen(Control i_fdcControl)
		{
			SUB_WasserzeichenEntfernen(i_fdcControl);
			AdornerLayer.GetAdornerLayer(i_fdcControl)?.Add(new EDU_WasserzeichenAdorner(i_fdcControl, GetPRO_strText(i_fdcControl), GetPRO_fdcResources(i_fdcControl)));
		}

		private static bool FUN_blnSollWasserzeichenAngezeigtWerden(Control i_fdcControl)
		{
			if (i_fdcControl is ComboBox)
			{
				return (i_fdcControl as ComboBox).SelectedItem == null;
			}
			return false;
		}
	}
}
