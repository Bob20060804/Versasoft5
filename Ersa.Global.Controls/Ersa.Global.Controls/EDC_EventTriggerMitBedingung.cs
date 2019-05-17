using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Ersa.Global.Controls
{
	public class EDC_EventTriggerMitBedingung : System.Windows.Interactivity.EventTrigger
	{
		public static readonly DependencyProperty PRO_blnBedingungProperty = DependencyProperty.Register("PRO_blnBedingung", typeof(bool), typeof(EDC_EventTriggerMitBedingung));

		public bool PRO_blnBedingung
		{
			get
			{
				return (bool)GetValue(PRO_blnBedingungProperty);
			}
			set
			{
				SetValue(PRO_blnBedingungProperty, value);
			}
		}

		protected override void OnEvent(EventArgs i_fdcArgs)
		{
			if (PRO_blnBedingung)
			{
				base.OnEvent(i_fdcArgs);
			}
		}
	}
}
