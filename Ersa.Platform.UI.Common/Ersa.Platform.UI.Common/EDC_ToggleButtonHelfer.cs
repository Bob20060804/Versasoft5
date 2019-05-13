using Ersa.Global.Controls;
using Ersa.Global.Controls.Extensions;
using Ersa.Platform.Common.Model;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.Platform.UI.Common
{
	public static class EDC_ToggleButtonHelfer
	{
		public static readonly DependencyProperty ms_intZustandProperty = DependencyProperty.RegisterAttached("PRO_intZustand", typeof(int), typeof(EDC_ToggleButtonHelfer), new PropertyMetadata(SUB_ButtonZustandGeaendert));

		public static readonly DependencyProperty ms_blnInfotextProperty = DependencyProperty.RegisterAttached("PRO_blnInfotext", typeof(bool), typeof(EDC_ToggleButtonHelfer), new PropertyMetadata(SUB_ButtonZustandGeaendert));

		public static readonly DependencyProperty ms_cmdCommandInfoTextProperty = DependencyProperty.RegisterAttached("PRO_cmdCommandInfoText", typeof(ICommand), typeof(EDC_ToggleButtonHelfer), new PropertyMetadata(SUB_ButtonZustandGeaendert));

		public static readonly DependencyProperty ms_objCommandParameterInfoTextProperty = DependencyProperty.RegisterAttached("PRO_objCommandParameterInfoText", typeof(object), typeof(EDC_ToggleButtonHelfer), new PropertyMetadata(SUB_ButtonZustandGeaendert));

		public static int GetPRO_intZustand(DependencyObject i_fdcDependencyObject)
		{
			return (int)i_fdcDependencyObject.GetValue(ms_intZustandProperty);
		}

		public static void SetPRO_intZustand(DependencyObject i_fdcDependencyObject, int i_lstZeilen)
		{
			i_fdcDependencyObject.SetValue(ms_intZustandProperty, i_lstZeilen);
		}

		public static bool GetPRO_blnInfotext(DependencyObject i_fdcDependencyObject)
		{
			return (bool)i_fdcDependencyObject.GetValue(ms_blnInfotextProperty);
		}

		public static void SetPRO_blnInfotext(DependencyObject i_fdcDependencyObject, bool i_lstZeilen)
		{
			i_fdcDependencyObject.SetValue(ms_blnInfotextProperty, i_lstZeilen);
		}

		public static ICommand GetPRO_cmdCommandInfoText(DependencyObject i_fdcDependencyObject)
		{
			return (ICommand)i_fdcDependencyObject.GetValue(ms_cmdCommandInfoTextProperty);
		}

		public static void SetPRO_cmdCommandInfoText(DependencyObject i_fdcDependencyObject, ICommand i_lstZeilen)
		{
			i_fdcDependencyObject.SetValue(ms_cmdCommandInfoTextProperty, i_lstZeilen);
		}

		public static object GetPRO_objCommandParameterInfoText(DependencyObject i_fdcDependencyObject)
		{
			return i_fdcDependencyObject.GetValue(ms_objCommandParameterInfoTextProperty);
		}

		public static void SetPRO_objCommandParameterInfoText(DependencyObject i_fdcDependencyObject, object i_lstZeilen)
		{
			i_fdcDependencyObject.SetValue(ms_objCommandParameterInfoTextProperty, i_lstZeilen);
		}

		private static void SUB_ButtonZustandGeaendert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_ToggleButton eDU_ToggleButton = i_fdcDependencyObject as EDU_ToggleButton;
			if (eDU_ToggleButton == null)
			{
				return;
			}
			int pRO_intZustand = GetPRO_intZustand(eDU_ToggleButton);
			bool flag = ((ENUM_ButtonZustand)pRO_intZustand).HasFlag(ENUM_ButtonZustand.enmEingabeGesperrt);
			bool flag2 = ((ENUM_ButtonZustand)pRO_intZustand).HasFlag(ENUM_ButtonZustand.enmTeilablaufAktiv);
			bool pRO_blnGesamtablaufAktiv = ((ENUM_ButtonZustand)pRO_intZustand).HasFlag(ENUM_ButtonZustand.enmGesamtablaufAktiv);
			bool pRO_blnInfotext = GetPRO_blnInfotext(eDU_ToggleButton);
			if (flag)
			{
				object commandParameter = eDU_ToggleButton.CommandParameter;
				if (commandParameter != null)
				{
					(eDU_ToggleButton as EDU_Taster)?.PRO_cmdTasterDeaktiviert.SUB_Execute(commandParameter, eDU_ToggleButton);
				}
			}
			eDU_ToggleButton.PRO_blnTeilablaufAktiv = flag2;
			eDU_ToggleButton.PRO_blnIstAktiv = flag2;
			eDU_ToggleButton.PRO_blnGesamtablaufAktiv = pRO_blnGesamtablaufAktiv;
			SUB_SetzeInfoTextFunktionalitaetWennNoetig(eDU_ToggleButton, flag, pRO_blnInfotext);
		}

		private static void SUB_SetzeInfoTextFunktionalitaetWennNoetig(EDU_ToggleButton i_eduToggleButton, bool i_blnEingabeGesperrt, bool i_blnIstInfoTextVerfuegbar)
		{
			ICommand pRO_cmdCommandInfoText = GetPRO_cmdCommandInfoText(i_eduToggleButton);
			object pRO_objCommandParameterInfoText = GetPRO_objCommandParameterInfoText(i_eduToggleButton);
			if (i_blnIstInfoTextVerfuegbar && i_blnEingabeGesperrt && pRO_cmdCommandInfoText != null && pRO_objCommandParameterInfoText != null)
			{
				if (i_eduToggleButton.PRO_fdcOrginalCommandParameter == null)
				{
					i_eduToggleButton.PRO_fdcOrginalCommandParameter = BindingOperations.GetBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty);
				}
				if (i_eduToggleButton.PRO_fdcOrginalCommandParameter != null)
				{
					BindingOperations.ClearBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty);
				}
				i_eduToggleButton.CommandParameter = pRO_objCommandParameterInfoText;
				Binding binding = BindingOperations.GetBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty);
				if (binding != null)
				{
					BindingOperations.SetBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty, binding);
				}
				if (i_eduToggleButton.PRO_cmdOrginalCommand == null)
				{
					i_eduToggleButton.PRO_cmdOrginalCommand = i_eduToggleButton.Command;
				}
				i_eduToggleButton.Command = pRO_cmdCommandInfoText;
				i_eduToggleButton.PRO_blnInfoTextAnzeigen = true;
				i_eduToggleButton.IsEnabled = true;
			}
			else
			{
				i_eduToggleButton.PRO_blnInfoTextAnzeigen = false;
				i_eduToggleButton.IsEnabled = !i_blnEingabeGesperrt;
				if (i_eduToggleButton.PRO_fdcOrginalCommandParameter != null)
				{
					BindingOperations.ClearBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty);
					BindingOperations.SetBinding(i_eduToggleButton, ButtonBase.CommandParameterProperty, i_eduToggleButton.PRO_fdcOrginalCommandParameter);
				}
				if (i_eduToggleButton.PRO_cmdOrginalCommand != null)
				{
					i_eduToggleButton.Command = i_eduToggleButton.PRO_cmdOrginalCommand;
				}
			}
		}
	}
}
