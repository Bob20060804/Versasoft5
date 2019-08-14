using System;
using System.Windows;
using System.Windows.Input;

namespace Prism.Interactivity
{
	public class CommandBehaviorBase<T> where T : UIElement
	{
		private ICommand _command;

		private object _commandParameter;

		private readonly WeakReference _targetObject;

		private readonly EventHandler _commandCanExecuteChangedHandler;

		private bool _autoEnabled = true;

		public bool AutoEnable
		{
			get
			{
				return _autoEnabled;
			}
			set
			{
				_autoEnabled = value;
				UpdateEnabledState();
			}
		}

		public ICommand Command
		{
			get
			{
				return _command;
			}
			set
			{
				if (_command != null)
				{
					_command.CanExecuteChanged -= _commandCanExecuteChangedHandler;
				}
				_command = value;
				if (_command != null)
				{
					_command.CanExecuteChanged += _commandCanExecuteChangedHandler;
					UpdateEnabledState();
				}
			}
		}

		public object CommandParameter
		{
			get
			{
				return _commandParameter;
			}
			set
			{
				if (_commandParameter != value)
				{
					_commandParameter = value;
					UpdateEnabledState();
				}
			}
		}

		protected T TargetObject => _targetObject.Target as T;

		public CommandBehaviorBase(T targetObject)
		{
			_targetObject = new WeakReference(targetObject);
			_commandCanExecuteChangedHandler = CommandCanExecuteChanged;
		}

		protected virtual void UpdateEnabledState()
		{
			if (TargetObject == null)
			{
				Command = null;
				CommandParameter = null;
			}
			else if (Command != null && AutoEnable)
			{
				TargetObject.IsEnabled = Command.CanExecute(CommandParameter);
			}
		}

		private void CommandCanExecuteChanged(object sender, EventArgs e)
		{
			UpdateEnabledState();
		}

		protected virtual void ExecuteCommand(object parameter)
		{
			if (Command != null)
			{
				Command.Execute(CommandParameter ?? parameter);
			}
		}
	}
}
