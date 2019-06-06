using Ersa.Global.Controls.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_CommandComboBox : ComboBox
	{
		public static readonly DependencyProperty PRO_cmdAuswahlGeandertProperty = DependencyProperty.Register("PRO_cmdAuswahlGeandert", typeof(ICommand), typeof(EDU_CommandComboBox));

		public static readonly DependencyProperty PRO_strAnzeigeTextProperty = DependencyProperty.Register("PRO_strAnzeigeText", typeof(string), typeof(EDU_CommandComboBox));

		public static readonly DependencyProperty PRO_blnInitialeAenderungIgnorierenProperty = DependencyProperty.Register("PRO_blnInitialeAenderungIgnorieren", typeof(bool), typeof(EDU_CommandComboBox), new PropertyMetadata(false));

		public ICommand PRO_cmdAuswahlGeandert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdAuswahlGeandertProperty);
			}
			set
			{
				SetValue(PRO_cmdAuswahlGeandertProperty, value);
			}
		}

		public string PRO_strAnzeigeText
		{
			get
			{
				return (string)GetValue(PRO_strAnzeigeTextProperty);
			}
			set
			{
				SetValue(PRO_strAnzeigeTextProperty, value);
			}
		}

		public bool PRO_blnInitialeAenderungIgnorieren
		{
			get
			{
				return (bool)GetValue(PRO_blnInitialeAenderungIgnorierenProperty);
			}
			set
			{
				SetValue(PRO_blnInitialeAenderungIgnorierenProperty, value);
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			if (e.AddedItems != null && e.AddedItems.Count > 0 && (!PRO_blnInitialeAenderungIgnorieren || e.RemovedItems.Count != 0))
			{
				PRO_cmdAuswahlGeandert?.SUB_Execute(base.SelectedItem, this);
			}
		}
	}
}
