using System;
using System.Windows.Input;

namespace Ersa.Global.Controls.Eingabe.ViewModels
{
	public class EDC_DelegateCommand : ICommand
	{
		private readonly Predicate<object> m_delCanExecute;

		private readonly Action<object> m_delExecute;

		public event EventHandler CanExecuteChanged;

		public EDC_DelegateCommand(Action<object> i_delExecute, Predicate<object> i_delCanExecute = null)
		{
			m_delCanExecute = i_delCanExecute;
			m_delExecute = i_delExecute;
		}

		public bool CanExecute(object i_objParameter)
		{
			if (m_delCanExecute != null)
			{
				return m_delCanExecute(i_objParameter);
			}
			return true;
		}

		public void Execute(object i_objParameter)
		{
			m_delExecute(i_objParameter);
		}

		public void SUB_OnCanExecuteChanged()
		{
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
