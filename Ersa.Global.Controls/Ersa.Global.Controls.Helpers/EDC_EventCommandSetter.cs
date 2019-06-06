using Ersa.Global.Controls.Extensions;
using System;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_EventCommandSetter : EventSetter
	{
		private ICommand m_cmdCommand;

		public ICommand PRO_cmdCommand
		{
			get
			{
				return m_cmdCommand;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (base.Event == null)
				{
					throw new ArgumentException("Event must be set");
				}
				base.Handler = Delegate.CreateDelegate(base.Event.HandlerType, this, new EventHandler(SUB_OnEvent).Method);
				m_cmdCommand = value;
			}
		}

		public bool PRO_blnEventHandeln
		{
			get;
			set;
		}

		public bool PRO_blnDataContextAlsParameter
		{
			get;
			set;
		}

		private void SUB_OnEvent(object i_objSender, EventArgs i_fdcArgs)
		{
			if (m_cmdCommand == null)
			{
				throw new ArgumentException("EventCommandSetter.Command must not be null!");
			}
			object obj = PRO_blnDataContextAlsParameter ? FUN_objDataContextErmitteln(i_objSender) : new EDC_EventCommandArgs(i_objSender, i_fdcArgs);
			if (!m_cmdCommand.CanExecute(obj))
			{
				return;
			}
			m_cmdCommand.SUB_Execute(obj, i_objSender as IInputElement);
			if (PRO_blnEventHandeln)
			{
				RoutedEventArgs routedEventArgs = i_fdcArgs as RoutedEventArgs;
				if (routedEventArgs != null)
				{
					routedEventArgs.Handled = true;
				}
			}
		}

		private object FUN_objDataContextErmitteln(object i_objSender)
		{
			return (i_objSender as FrameworkElement)?.DataContext;
		}
	}
}
