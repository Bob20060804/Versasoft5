using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Prism.Commands
{
	public class CompositeCommand : ICommand
	{
		private readonly List<ICommand> _registeredCommands = new List<ICommand>();

		private readonly bool _monitorCommandActivity;

		private readonly EventHandler _onRegisteredCommandCanExecuteChangedHandler;

		private SynchronizationContext _synchronizationContext;

		public IList<ICommand> RegisteredCommands
		{
			get
			{
				lock (_registeredCommands)
				{
					return _registeredCommands.ToList();
				}
			}
		}

		public virtual event EventHandler CanExecuteChanged;

		public CompositeCommand()
		{
			_onRegisteredCommandCanExecuteChangedHandler = OnRegisteredCommandCanExecuteChanged;
			_synchronizationContext = SynchronizationContext.Current;
		}

		public CompositeCommand(bool monitorCommandActivity)
			: this()
		{
			_monitorCommandActivity = monitorCommandActivity;
		}

		public virtual void RegisterCommand(ICommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command == this)
			{
				throw new ArgumentException(Resources.CannotRegisterCompositeCommandInItself);
			}
			lock (_registeredCommands)
			{
				if (_registeredCommands.Contains(command))
				{
					throw new InvalidOperationException(Resources.CannotRegisterSameCommandTwice);
				}
				_registeredCommands.Add(command);
			}
			command.CanExecuteChanged += _onRegisteredCommandCanExecuteChangedHandler;
			OnCanExecuteChanged();
			if (_monitorCommandActivity)
			{
				IActiveAware activeAware = command as IActiveAware;
				if (activeAware != null)
				{
					activeAware.IsActiveChanged += Command_IsActiveChanged;
				}
			}
		}

		public virtual void UnregisterCommand(ICommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			bool flag;
			lock (_registeredCommands)
			{
				flag = _registeredCommands.Remove(command);
			}
			if (!flag)
			{
				return;
			}
			command.CanExecuteChanged -= _onRegisteredCommandCanExecuteChangedHandler;
			OnCanExecuteChanged();
			if (_monitorCommandActivity)
			{
				IActiveAware activeAware = command as IActiveAware;
				if (activeAware != null)
				{
					activeAware.IsActiveChanged -= Command_IsActiveChanged;
				}
			}
		}

		private void OnRegisteredCommandCanExecuteChanged(object sender, EventArgs e)
		{
			OnCanExecuteChanged();
		}

		public virtual bool CanExecute(object parameter)
		{
			bool result = false;
			ICommand[] array;
			lock (_registeredCommands)
			{
				array = _registeredCommands.ToArray();
			}
			ICommand[] array2 = array;
			foreach (ICommand command in array2)
			{
				if (ShouldExecute(command))
				{
					if (!command.CanExecute(parameter))
					{
						return false;
					}
					result = true;
				}
			}
			return result;
		}

		public virtual void Execute(object parameter)
		{
			Queue<ICommand> queue;
			lock (_registeredCommands)
			{
				queue = new Queue<ICommand>(_registeredCommands.Where(ShouldExecute).ToList());
			}
			while (queue.Count > 0)
			{
				queue.Dequeue().Execute(parameter);
			}
		}

		protected virtual bool ShouldExecute(ICommand command)
		{
			IActiveAware activeAware = command as IActiveAware;
			if (_monitorCommandActivity && activeAware != null)
			{
				return activeAware.IsActive;
			}
			return true;
		}

		protected virtual void OnCanExecuteChanged()
		{
			EventHandler handler = this.CanExecuteChanged;
			if (handler != null)
			{
				if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
				{
					_synchronizationContext.Post(delegate
					{
						handler(this, EventArgs.Empty);
					}, null);
				}
				else
				{
					handler(this, EventArgs.Empty);
				}
			}
		}

		private void Command_IsActiveChanged(object sender, EventArgs e)
		{
			OnCanExecuteChanged();
		}
	}
}
