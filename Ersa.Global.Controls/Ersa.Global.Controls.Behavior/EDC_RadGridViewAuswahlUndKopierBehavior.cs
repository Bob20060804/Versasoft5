using Ersa.Global.Controls.Extensions;
using Ersa.Global.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Ersa.Global.Controls.Behavior
{
	public class EDC_RadGridViewAuswahlUndKopierBehavior : Behavior<RadGridView>
	{
		public static readonly DependencyProperty ms_fdcRechteckigeAuswahlProperty = DependencyProperty.Register("PRO_blnRechteckigeAuswahl", typeof(bool), typeof(EDC_RadGridViewAuswahlUndKopierBehavior));

		public static readonly DependencyProperty ms_fdcAuswahlProperty = DependencyProperty.Register("PRO_dicAuswahl", typeof(IDictionary<Tuple<int, int>, EDC_GetterSetterPaar>), typeof(EDC_RadGridViewAuswahlUndKopierBehavior));

		private readonly CommandBinding m_fdcCopyCommandBinding;

		private readonly CommandBinding m_fdcPasteCommandBinding;

		public bool PRO_blnRechteckigeAuswahl
		{
			get
			{
				return (bool)GetValue(ms_fdcRechteckigeAuswahlProperty);
			}
			set
			{
				SetValue(ms_fdcRechteckigeAuswahlProperty, value);
			}
		}

		public IDictionary<Tuple<int, int>, EDC_GetterSetterPaar> PRO_dicAuswahl
		{
			get
			{
				return (IDictionary<Tuple<int, int>, EDC_GetterSetterPaar>)GetValue(ms_fdcAuswahlProperty);
			}
			set
			{
				SetValue(ms_fdcAuswahlProperty, value);
			}
		}

		public bool PRO_blnNurZahlenEinfuegen
		{
			get;
			set;
		}

		public EDC_RadGridViewAuswahlUndKopierBehavior()
		{
			m_fdcCopyCommandBinding = new CommandBinding(ApplicationCommands.Copy);
			m_fdcCopyCommandBinding.CanExecute += SUB_KannKopierenErmittelnUndSetzen;
			m_fdcPasteCommandBinding = new CommandBinding(ApplicationCommands.Paste);
			m_fdcPasteCommandBinding.CanExecute += SUB_KannEinfuegenErmittelnUndSetzen;
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			PRO_dicAuswahl = new Dictionary<Tuple<int, int>, EDC_GetterSetterPaar>();
			base.AssociatedObject.SelectedCellsChanged += SUB_RadGridViewSelektionGeandert;
			base.AssociatedObject.CommandBindings.Add(m_fdcCopyCommandBinding);
			base.AssociatedObject.CommandBindings.Add(m_fdcPasteCommandBinding);
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.SelectedCellsChanged -= SUB_RadGridViewSelektionGeandert;
			base.AssociatedObject.CommandBindings.Remove(m_fdcCopyCommandBinding);
			base.AssociatedObject.CommandBindings.Remove(m_fdcPasteCommandBinding);
			base.OnDetaching();
		}

		private void SUB_RadGridViewSelektionGeandert(object i_blnSender, GridViewSelectedCellsChangedEventArgs i_blnGridViewSelectedCellsChangedEventArgs)
		{
			PRO_blnRechteckigeAuswahl = base.AssociatedObject.FUN_blnRechteckigeAuswahl();
			PRO_dicAuswahl.Clear();
			foreach (KeyValuePair<Tuple<int, int>, EDC_GetterSetterPaar> item in base.AssociatedObject.FUN_dicAuswahlTabelleErstellen())
			{
				PRO_dicAuswahl.Add(item);
			}
		}

		private void SUB_KannKopierenErmittelnUndSetzen(object i_objSender, CanExecuteRoutedEventArgs i_fdcArgs)
		{
			i_fdcArgs.CanExecute = PRO_blnRechteckigeAuswahl;
			i_fdcArgs.Handled = !PRO_blnRechteckigeAuswahl;
			i_fdcArgs.ContinueRouting = PRO_blnRechteckigeAuswahl;
		}

		private void SUB_KannEinfuegenErmittelnUndSetzen(object i_objSender, CanExecuteRoutedEventArgs i_fdcArgs)
		{
			bool flag2 = i_fdcArgs.CanExecute = FUN_blnGueltigerTextInZwischenablage();
			i_fdcArgs.Handled = !flag2;
			i_fdcArgs.ContinueRouting = flag2;
		}

		private bool FUN_blnGueltigerTextInZwischenablage()
		{
			if (!Clipboard.ContainsText())
			{
				return false;
			}
			string text = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			if (!text.EndsWith("\r\n"))
			{
				return false;
			}
			List<string[]> source = (from i_strZeile in text.Split(new string[1]
			{
				"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries)
			select i_strZeile.Split(new string[1]
			{
				"\t"
			}, StringSplitOptions.RemoveEmptyEntries)).ToList();
			if ((from i_enuEintraege in source
			select i_enuEintraege.Count()).Distinct().Count() != 1)
			{
				return false;
			}
			if (PRO_blnNurZahlenEinfuegen)
			{
				foreach (string item in source.SelectMany((string[] ia_strEintraege) => ia_strEintraege))
				{
					if (!double.TryParse(item, out double _))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
